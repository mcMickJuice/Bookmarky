using System.Collections.Generic;
using System.Linq;
using Bookmarky.DAL.ServiceImplementations;
using Bookmarky.DTO;
using Bookmark = Bookmarky.DAL.EntityModels.Bookmark;

namespace Bookmarky.Test.UnitTests.PredicateBuilderTests
{
    public class BasePredicateBuilderTest
    {
        protected List<Bookmark> applyPredicateToList(IEnumerable<Bookmark> bookmarks, BookmarkSearchCriteria criteria)
        {
            var predicates = new BookmarkSearchPredicateCreator(criteria).GetPredicateList().ToList();

            IEnumerable<Bookmark> criteriaList = bookmarks;

            predicates.ForEach(p =>
            {
                var b = p.Compile();
                criteriaList = criteriaList.Where(b);
            });

            return criteriaList.ToList();
        }
    }
}