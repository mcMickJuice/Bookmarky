using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bookmarky.DAL.Service;
using Bookmarky.DTO;

namespace Bookmarky.WebAPI.Controllers
{
    public class BookmarkController : ApiController
    {
        private readonly IBookmarkDataService _dataService;

        public BookmarkController(IBookmarkDataService dataService)
        {
            _dataService = dataService;
        }

        // GET: api/Bookmark
        public IEnumerable<Bookmark> Get()
        {
            var bookmarks = _dataService.GetAllBookmarks();
            return bookmarks;
        }

        // GET: api/Bookmark/5
        public Bookmark Get(int id)
        {
            var bookmark = _dataService.GetBookmarkById(id);

            return bookmark ?? null;
        }

        // POST: api/Bookmark
        //how to return data?
        public Bookmark Post([FromBody]Bookmark bookmark)
        {
            var returnBm = _dataService.SaveBookmark(bookmark);

            return returnBm;
        }

        //// DELETE: api/Bookmark/5
        //public void Delete(int id)
        //{
        //}
    }
}
