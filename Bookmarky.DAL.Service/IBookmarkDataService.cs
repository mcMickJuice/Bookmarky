using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DTO;

namespace Bookmarky.DAL.Service
{
	public interface IBookmarkDataService : IDisposable
	{
		IEnumerable<Bookmark> GetAllBookmarks();
		IEnumerable<Bookmark> GetUnreadBookmarks();
	    Bookmark GetBookmarkById(int id);
		/// <summary>
		/// Handles Creation and Updates of Bookmarks
		/// </summary>
		/// <param name="bookmark">Bookmark to Persist</param>
		/// <returns>Created or Updated Bookmark</returns>
		Bookmark UpdateBookmark(Bookmark bookmark);

	    Bookmark CreateBookmark(Bookmark bookmark);
		void UpdateIsReadStatus(int bookmarkId,bool isRead);
        IEnumerable<Bookmark> SearchBookmarksByCriteria(BookmarkSearchCriteria searchCriteria);
        HomePageSummary GetHomePageSummary();
        BookmarkSearchInitialization CreateInitialSearchObject();

	}

    public interface IBookmarkSearch
    {
        IEnumerable<Bookmark> SearchBookmarksByCriteria(BookmarkSearchCriteria searchCriteria);
    }
}
