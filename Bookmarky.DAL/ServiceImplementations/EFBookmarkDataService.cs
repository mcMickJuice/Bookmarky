using Bookmarky.DAL.Mapping;
using Bookmarky.DAL.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LinqKit;
using Bookmarky.DTO;
using Bookmark_DTO = Bookmarky.DTO.Bookmark;
using Bookmark_DB = Bookmarky.DAL.EntityModels.Bookmark;
using Tag_DTO = Bookmarky.DTO.Tag;
using Tag_DB = Bookmarky.DAL.EntityModels.Tag;
using Bookmarky.DAL.EntityModels;
using Bookmarky.Utility.Extensions;
using System.Linq.Expressions;

namespace Bookmarky.DAL.ServiceImplementations
{
    public class EFBookmarkDataService : IBookmarkDataService, ITagService, IReviewDataService
    {
        private IBookmarkyContext _context;
        private readonly IBookmarkyMapper _mapper;
        private bool _isDisposed = false;
        public EFBookmarkDataService(IBookmarkyContext context, IBookmarkyMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _context.Dispose();
                _isDisposed = true;
            }
        }

        public IEnumerable<Bookmark_DTO> GetAllBookmarks()
        {
            var dbBms = getBookmarks();

            return dbBms.Select(_mapper.MapToBookmarkDto).ToList();
        }

        public IEnumerable<Bookmark_DTO> GetUnreadBookmarks()
        {
            var predicate = PredicateBuilder.True<Bookmark_DB>()
                .And(b => !b.IsRead);

            var dbBms = getBookmarks(predicate);

            return dbBms.Select(_mapper.MapToBookmarkDto).ToList();
        }

        public Bookmark_DTO GetBookmarkById(int id)
        {
            var dbBm = getBookmarks(b => b.Id == id, b => b.Tags, b => b.Rating).FirstOrDefault();

            if (dbBm == null)
                return null;

            var bmDto = _mapper.MapToBookmarkDto(dbBm);

            return bmDto;
        }



        public void UpdateIsReadStatus(int bookmarkId, bool isRead)
        {
            var db = getBookmarks(b => b.Id == bookmarkId).FirstOrDefault();

            if (db == null)
                return;

            db.IsRead = isRead;
            _context.SaveChanges();
        }

        public HomePageSummary GetHomePageSummary()
        {
            var bookmarks = getBookmarks();

            var stickied = bookmarks.Where(bm => bm.IsStickied);

            var stickiedDto = stickied.Select(_mapper.MapToBookmarkDto);

            var recent = bookmarks
                .Take(10)
                .ToList();

            var recentDto = recent.Select(_mapper.MapToBookmarkDto);

            var homePage = new HomePageSummary
            {
                RecentBookmarks = recentDto,
                StickiedBookmarks = stickiedDto
            };

            return homePage;
        }

        public BookmarkSearchInitialization CreateInitialSearchObject()
        {
            var tags = getTags()
                .Select(t => t.MapTo<Tag_DTO>())
                .ToList();

            var resources = Enum.GetValues(typeof(ResourceType))
                .OfType<ResourceType>()
                .Select(t => new
                {
                    Id = (int)t,
                    ResourceName = t.ToString()
                });

            var logicTypes = Enum.GetValues(typeof(LogicType))
                .OfType<LogicType>()
                .Select(t => new
                {
                    Id = (int)t,
                    LogicName = t.ToString()
                });

            var inclusionTypes = Enum.GetValues(typeof(InclusionType))
                .OfType<InclusionType>()
                .Select(t => new
                {
                    Id = (int)t,
                    InclusionName = t.ToString()
                });

            var initialization = new BookmarkSearchInitialization
            {
                SearchCriteria = new BookmarkSearchCriteria(),
                FieldInitialization = new BookmarkSearchFieldInitialization
                {
                    AvailableTags = tags,
                    Resources = resources,
                    LogicTypes = logicTypes,
                    InclusionTypes = inclusionTypes
                }
            };

            return initialization;
        }

        public Bookmark_DTO CreateBookmark(Bookmark_DTO bookmark)
        {
            var dbBm = _mapper.MapToBookmarkDb(bookmark);

            dbBm.Tags.ForEach(t =>
            {
                _context.Set<Tag_DB>().Attach(t); //attached tag to context to mark as unchanged
            });

            _context.Set<Bookmark_DB>().Add(dbBm);
            _context.SaveChanges();	//Todo add error handling
            bookmark.Id = dbBm.Id;

            return bookmark;
        }

        public Bookmark_DTO UpdateBookmark(Bookmark_DTO bookmark)
        {
            //var existingBm = _context.Set<Bookmark_DB>().Find(bookmark.Id);
            var existingBm = getBookmarks(b => b.Id == bookmark.Id, b => b.Tags)
                .FirstOrDefault();

            if (existingBm == null)
                throw new InvalidOperationException("BookmarkId not found");

            existingBm.Gist = bookmark.Gist;
            existingBm.ResourceType = (ResourceType)bookmark.ResourceType;
           // existingBm.Source = bookmark.Source;
            existingBm.Title = bookmark.Title;
            existingBm.Url = bookmark.Url;
            existingBm.IsStickied = bookmark.IsStickied;

            var dtoTags = bookmark.Tags.ToList();

            var addedTags = getTagsToAdd(dtoTags, existingBm.Tags);
            addedTags.ForEach(t =>
            {
                _context.Set<Tag_DB>().Attach(t);
                existingBm.Tags.Add(t);
            });

            var deletedTags = getTagsToRemove(dtoTags,existingBm.Tags);
            deletedTags.ForEach(t =>
            {
                _context.Set<Tag_DB>().Attach(t);
                existingBm.Tags.Remove(t);
            });


            _context.SaveChanges();

            return bookmark;
        }

        private IEnumerable<Tag_DB> getTagsToRemove(IEnumerable<Tag_DTO> dtoTags, IEnumerable<Tag_DB> existingTags)
        {
            var tagsToDelete = existingTags
                .Where(dbT => !dtoTags
                    .Select(dtoT => dtoT.Id)
                    .Contains(dbT.Id))
                .ToList();

            return tagsToDelete;
        }

        private IEnumerable<Tag_DB> getTagsToAdd(IEnumerable<Tag_DTO> dtoTags, IEnumerable<Tag_DB> existingTags)
        {
            var newTags = dtoTags
                .Where(t => !existingTags
                    .Select(dbT => dbT.Id)
                    .Contains(t.Id))
                .Select(_mapper.MapToTagDb)
                .ToList();

            return newTags;
        }

        public IEnumerable<Bookmark_DTO> SearchBookmarksByCriteria(BookmarkSearchCriteria searchCriteria)
        {
            var isAndFieldLogic = searchCriteria.AndOrFieldLogic == LogicType.And;
            var searchCriteriaBuilder = new SearchCriteriaBuilder<Bookmark_DB>(isAndFieldLogic);

            var creator = new BookmarkSearchPredicateCreator(searchCriteria);
            creator.GetPredicateList().ForEach(p => searchCriteriaBuilder.AppendCriteria(p));

            var predicate = searchCriteriaBuilder.BuildPredicate();
            var bmDbs = getBookmarks(predicate);

            return bmDbs.Select(r => r.MapTo<Bookmark_DTO>());
        }

        public IEnumerable<Tag_DTO> GetAllTags()
        {
            var tags = getTags();

            return tags.Select(_mapper.MapToTagDto);
        }

        public Tag_DTO CreateTag(Tag_DTO tag)
        {
            var dbTag = _mapper.MapToTagDb(tag);
            
            _context.Set<Tag_DB>().Add(dbTag);

            _context.SaveChanges();

            return _mapper.MapToTagDto(dbTag);
        }

        
        public Review CreateReview(Review review)
        {
            var rating = _mapper.MapToRatingDb(review);
            rating.Id = review.BookmarkId;

            _context.Set<Rating>().Add(rating);

            _context.SaveChanges();

            return _mapper.MapToReviewDto(rating);
        }

        public Review UpdateReview(Review review)
        {
            var rating = getRatings(r => r.Id == review.Id)
                .FirstOrDefault();

            if (rating == null)
                return null;

            rating.Overview = review.Overview;
            rating.Score = review.Score;

            _context.SaveChanges();

            return review;
        }

        public Review GetReviewById(int id)
        {
            var rating = getRatings(r => r.Id == id).FirstOrDefault();

            if (rating == null)
                return null;

            var review = _mapper.MapToReviewDto(rating);

            return review;
        }

        private IList<Bookmark_DB> getBookmarks(Expression<Func<Bookmark_DB, bool>> predicate = null
           , params Expression<Func<Bookmark_DB, object>>[] includes)
        {
            var query = _context.Set<Bookmark_DB>().AsQueryable();

            includes.ForEach(i => query = query.Include(i));

            if (predicate != null)
            {
                query = query.AsExpandable().Where(predicate);
            }

            return query.ToList();
        }

        private IList<Tag_DB> getTags(Expression<Func<Tag_DB, bool>> predicate = null)
        {
            var query = _context.Set<Tag_DB>().AsQueryable();

            if (predicate != null)
            {
                query = query.AsExpandable().Where(predicate);
            }

            return query.ToList();
        }

        private IList<Rating> getRatings(Expression<Func<Rating, bool>> predicate = null
            , params Expression<Func<Rating, object>>[] includes)
        {
            var query = _context.Set<Rating>().AsQueryable();

            includes.ForEach(i =>
            {
                query = query.Include(i);
            });

            if (predicate != null)
            {
                query = query.AsExpandable().Where(predicate);
            }

            return query.ToList();
        }
    }
}
