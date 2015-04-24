using Bookmarky.DAL.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LinqKit;
using Bookmarky.DTO;
using Bookmark_DTO = Bookmarky.DTO.Bookmark;
using Bookmark_DB = Bookmarky.DAL.EntityModels.Bookmark;
using Bookmarky.DAL.EntityModels;
using Bookmarky.Utility.Extensions;
using System.Linq.Expressions;

namespace Bookmarky.DAL.ServiceImplementations
{
    public class EFBookmarkDataService : IBookmarkDataService
    {
        private IBookmarkyContext _context;
        private bool _isDisposed = false;
        public EFBookmarkDataService(IBookmarkyContext context)
        {
            _context = context;
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

            return dbBms.Select(b => b.MapTo<Bookmark_DTO>()).ToList();
        }

        public IEnumerable<Bookmark_DTO> GetUnreadBookmarks()
        {
            var predicate = PredicateBuilder.True<Bookmark_DB>()
                .And(b => !b.IsRead);

            var dbBms = getBookmarks(predicate);

            return dbBms.Select(b => b.MapTo<Bookmark_DTO>()).ToList();
        }

        public Bookmark_DTO GetBookmarkById(int id)
        {
            var dbBm = getBookmarks(b => b.Id == id).FirstOrDefault();

            if (dbBm == null)
                return null;

            return dbBm.MapTo<Bookmark_DTO>();
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
            var stickied = _context.Set<Bookmark_DB>()
                .OfType<StickiedBookmark>()
                .ToList();

            var stickiedDto = stickied.Select(s => s.MapTo<Bookmark_DTO>());

            var recent = _context.Set<Bookmark_DB>().OrderByDescending(b => b.CreatedDate)
                .Take(10)
                .ToList();

            var recentDto = recent.Select(r => r.MapTo<Bookmark_DTO>());

            var homePage = new HomePageSummary
            {
                RecentBookmarks = recentDto,
                StickiedBookmarks = stickiedDto
            };

            return homePage;
        }

        public Bookmark_DTO SaveBookmark(Bookmark_DTO bookmark)
        {

            if (bookmark.Id == 0)
            {
                var dbBm = bookmark.MapTo<Bookmark_DB>();
                _context.Set<Bookmark_DB>().Add(dbBm);
                _context.SaveChanges();	//Todo add error handling
                bookmark.Id = dbBm.Id;
            }
            else
            {
                var existingBm = _context.Set<Bookmark_DB>().Find(bookmark.Id);
                existingBm.Gist = bookmark.Gist;
                existingBm.ResourceType = (ResourceType)bookmark.ResourceTypeId;
                existingBm.SourceId = bookmark.SourceId;
                existingBm.Title = bookmark.Title;
                existingBm.Url = bookmark.Url;

                _context.SaveChanges();
            }

            return bookmark;
        }

        public IEnumerable<Bookmark_DTO> SearchBookmarksByCriteria(BookmarkSearchCriteria searchCriteria)
        {
            var searchCriteriaBuilder = new SearchCriteriaBuilder<Bookmark_DB>(searchCriteria.IsAnd);

            var creator = new BookmarkSearchPredicateCreator(searchCriteria);
            creator.GetPredicateList().ForEach(p => searchCriteriaBuilder.AppendCriteria(p));

            var predicate = searchCriteriaBuilder.BuildPredicate();
            var bmDbs = getBookmarks(predicate);

            return bmDbs.Select(r => r.MapTo<Bookmark_DTO>());
        }

        private IQueryable<Bookmark_DB> getBookmarks(Expression<Func<Bookmark_DB, bool>> predicate = null
            , params Expression<Func<Bookmark_DB, object>>[] includes)
        {
            var query = _context.Set<Bookmark_DB>().AsQueryable();

            includes.ForEach(i => query.Include(i));

            if (predicate != null)
            {
                query = query.AsExpandable().Where(predicate);
            }

            return query;
        }

    }
}
