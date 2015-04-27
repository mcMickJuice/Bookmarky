using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bookmarky.DTO;
using LinqKit;
using Bookmark = Bookmarky.DAL.EntityModels.Bookmark;

namespace Bookmarky.DAL.ServiceImplementations
{
    /// <summary>
    /// Takes a BookmarkSearchCriteria Instance and decomposes it into bookmark_Db Predicates
    /// </summary>
    public class BookmarkSearchPredicateCreator
    {
        private readonly BookmarkSearchCriteria _criteria;
        private List<Expression<Func<Bookmark, bool>>> _predicates;

        public BookmarkSearchPredicateCreator(BookmarkSearchCriteria criteria)
        {
            _criteria = criteria;
            _predicates = new List<Expression<Func<Bookmark, bool>>>();
        }

        public IEnumerable<Expression<Func<Bookmark, bool>>> GetPredicateList()
        {
            _predicates.Add(titlePredicate());
            _predicates.Add(minimumRatingPredicate());
            _predicates.Add(tagsPredicate());
            _predicates.Add(resourceTypePredicate());

            return _predicates;
        }

        private Expression<Func<Bookmark, bool>> getBasePredicate()
        {
            //If this ISNT And, then Or is used downstream which will result in any separate True to return all bookmarks
            if (_criteria.AndOrFieldLogic == LogicType.And)
                return PredicateBuilder.True<Bookmark>();

            return PredicateBuilder.False<Bookmark>();
        }

        private Expression<Func<Bookmark, bool>> titlePredicate()
        {
            var title = _criteria.TitleText;

            if (String.IsNullOrEmpty(title) == true)
                return getBasePredicate();

            return (b) => b.Title.ToUpper().Contains(title.ToUpper());
        }

        private Expression<Func<Bookmark, bool>> minimumRatingPredicate()
        {
            var rating = _criteria.MinimumRating;

            if (rating.HasValue)
                return bm => bm.Rating != null && bm.Rating.Score >= rating.Value;

            return getBasePredicate();
        }

        private Expression<Func<Bookmark, bool>> tagsPredicate()
        {
            if (!_criteria.TagIds.Any())
                return getBasePredicate();

            if (_criteria.AnyAllTagLogic == InclusionType.All)
            {
                return bm => bm.Tags.Any()
                    && !_criteria.TagIds
                    .Except(bm.Tags.Select(t => t.Id)) //left hand side comparison. Any that don't match left side to right side, there will be remaining Ids.
                    .Any();
            }

            return bm => bm.Tags.Any() && _criteria.TagIds
                .Intersect(bm.Tags.Select(t => t.Id))
                .Any();

        }

        private Expression<Func<Bookmark, bool>> resourceTypePredicate()
        {
            if (!_criteria.ResourceTypeIds.Any())
                return getBasePredicate();

            return bm => _criteria
                .ResourceTypeIds
                .Contains((int)bm.ResourceType);
        }








    }
}