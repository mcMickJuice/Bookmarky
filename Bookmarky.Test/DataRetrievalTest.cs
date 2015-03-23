using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Bookmarky.DAL.EntityModels;

namespace Bookmarky.Test
{
	[TestClass]
	public class DataRetrievalTest
	{
		[TestMethod]
		public void GetData()
		{
			var context = new Bookmarky.DAL.EntityModels.BookmarkyContext();
			var bookmarks = context.Bookmarks.ToList();

			Assert.AreEqual(3, bookmarks.Count);
		}

	    [TestMethod]
	    public void GetStickiedLinks()
	    {
	        var context = new Bookmarky.DAL.EntityModels.BookmarkyContext();
	        var stickiedBm = context.Bookmarks.ToList().OfType<StickiedBookmark>();

            Assert.IsTrue(stickiedBm.Any());
	    }
	}
}
