﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bookmarky.DAL.Service;
using Bookmarky.DTO;

namespace Bookmarky.WebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:9000", headers: "*", methods: "*")]
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

            return bookmark;
        }

        // POST: api/Bookmark
        //how to return data?
        public Bookmark Post([FromBody]Bookmark bookmark)
        {
            var returnBm = _dataService.SaveBookmark(bookmark);

            return returnBm;
        }

        [AcceptVerbs("GET")]
        //[Route("api/Bookmark/GetHomePageDetails")]
        public HomePageSummary GetHomePageDetails()
        {
            var summary = _dataService.GetHomePageSummary();
            return summary;
        }

        //[AcceptVerbs("GET")]
        //[Route("api/Bookmark/")]

        //// DELETE: api/Bookmark/5
        //public void Delete(int id)
        //{
        //}
    }
}
