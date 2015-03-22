using Bookmarky.DAL.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LinqKit;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DTO;
using Bookmark_DTO = Bookmarky.DTO.Bookmark;
using Bookmark_DB = Bookmarky.DAL.EntityModels.Bookmark;
using Bookmarky.DAL.EntityModels;
using Omu.ValueInjecter;
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

		public IList<Bookmark_DTO> GetAllBookmarks()
		{
			var dbBms = getBookmarks();

			return dbBms.Select(b => b.MapTo<Bookmark_DTO>()).ToList();
		}

		public IList<Bookmark_DTO> GetBookmarkByExample(Bookmark_DTO bookmark) //include properties???
		{
			throw new NotImplementedException();
		}

		public IList<Bookmark_DTO> GetBookmarksBySource(int sourceId)
		{
			var dbBms = getBookmarks(b => b.SourceId == sourceId);

			return dbBms.Select(b => b.MapTo<Bookmark_DTO>()).ToList();
		}

		public IList<Bookmark_DTO> GetUnreadBookmarks()
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

		private IEnumerable<Bookmark_DB> getBookmarks(Expression<Func<Bookmark_DB, bool>> predicate = null
			, params Expression<Func<Bookmark_DB, object>>[] includes)
		{
			var query = _context.Set<Bookmark_DB>().AsQueryable();

			includes.ForEach(i => query.Include(i));

			if (predicate != null)
			{
				query = query.AsExpandable().Where(predicate);
			}

			return query.ToList();
		}

		
	}
}
