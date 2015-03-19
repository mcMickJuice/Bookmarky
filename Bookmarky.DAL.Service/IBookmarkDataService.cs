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
		IList<Bookmark> GetAllBookmarks();
		IList<Bookmark> GetBookmarksBySource(int sourceId);
		IList<Bookmark> GetBookmarkByExample(Bookmark bookmark);
		IList<Bookmark> GetUnreadBookmarks();
	    Bookmark GetBookmarkById(int id);
		/// <summary>
		/// Handles Creation and Updates of Bookmarks
		/// </summary>
		/// <param name="bookmark">Bookmark to Persist</param>
		/// <returns>Created or Updated Bookmark</returns>
		Bookmark SaveBookmark(Bookmark bookmark);
		void UpdateIsReadStatus(int bookmarkId,bool isRead);
	}
}
