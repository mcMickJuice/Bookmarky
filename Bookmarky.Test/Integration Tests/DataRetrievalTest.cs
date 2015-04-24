using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Bookmarky.DAL.EntityModels;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Bookmarky.Test.Integration_Tests
{
	[TestFixture]
	public class DataRetrievalTest
	{
	    private IBookmarkyContext _context;
	    [SetUp]
	    public void Init()
	    {
            _context = new BookmarkyContext("BookmarkyContext");
	        var bookmarks =  _context.Set<Bookmark>().ToList();
	        _context.Set<Bookmark>().RemoveRange(bookmarks);

	        _context.SaveChanges();
	    }

		[Test]
		public void GetData()
		{
		    var bms = new List<Bookmark>
		    {
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!"},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!"},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!"},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!"},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!"},
		    };

		    _context.Set<Bookmark>().AddRange(bms);
		    _context.SaveChanges();

			var bookmarks = _context.Set<Bookmark>().ToList();

			Assert.AreEqual(bms.Count, bookmarks.Count);
		}

	    [Test]
	    public void GetStickiedLinks()
	    {
	        var sbms = new List<StickiedBookmark>
	        {
                new StickiedBookmark(){Gist = "This is stickied",Title = "Check this sticky"},
                new StickiedBookmark(){Gist = "This is stickied",Title = "Check this sticky"},
	        };

	        _context.Set<StickiedBookmark>().AddRange(sbms);
	        _context.SaveChanges();

	        var stickiedBm = _context.Set<Bookmark>().ToList().OfType<StickiedBookmark>();

            Assert.AreEqual(sbms.Count, stickiedBm.Count());
	    }

	    [Test]
	    public void GetStickied_InsertedWithBookmarks()
	    {
            var bms = new List<Bookmark>
		    {
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
		        new Bookmark() {Gist = "Hi hi hi", IsRead = false, Title = "I'm a title!", CreatedDate = DateTime.Now.AddHours(-4)},
                new StickiedBookmark(){Gist = "This is stickied",Title = "Check this sticky", CreatedDate = DateTime.Now.AddHours(-2)},
                new StickiedBookmark(){Gist = "This is stickied",Title = "Check this sticky", CreatedDate = DateTime.Now.AddHours(-4)},
                new StickiedBookmark(){Gist = "This is stickied",Title = "Check this sticky", CreatedDate = DateTime.Now.AddHours(-4)},
                new StickiedBookmark(){Gist = "This is stickied",Title = "Check this sticky", CreatedDate = DateTime.Now.AddHours(-4)}
		    };

	        _context.Set<Bookmark>().AddRange(bms);
	        _context.SaveChanges();

	        var stickied = _context.Set<StickiedBookmark>().ToList();
            Assert.AreEqual(4,stickied.Count);

	    }

	    [TearDown]
	    public void Teardown()
	    {
            var bookmarks = _context.Set<Bookmark>().ToList();
            _context.Set<Bookmark>().RemoveRange(bookmarks);

            _context.SaveChanges();
	    }
	}
}
