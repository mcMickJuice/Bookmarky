using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DTO;

namespace Bookmarky.DAL.Service
{
	public interface IBookmarkDataService : IBookmarkSearch,IDisposable
	{
		IEnumerable<Bookmark> GetAllBookmarks();
		IEnumerable<Bookmark> GetUnreadBookmarks();
	    Bookmark GetBookmarkById(int id);
		/// <summary>
		/// Handles Creation and Updates of Bookmarks
		/// </summary>
		/// <param name="bookmark">Bookmark to Persist</param>
		/// <returns>Created or Updated Bookmark</returns>
		Bookmark SaveBookmark(Bookmark bookmark);
		void UpdateIsReadStatus(int bookmarkId,bool isRead);
	    HomePageSummary GetHomePageSummary();


	}

    public interface IBookmarkSearch
    {
        IEnumerable<Bookmark> SearchBookmarksByCriteria(BookmarkSearchCriteria searchCriteria);
    }
}
