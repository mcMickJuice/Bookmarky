using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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

			Assert.AreEqual(2, bookmarks.Count);
		}
	}
}
