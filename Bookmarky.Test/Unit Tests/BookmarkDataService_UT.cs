using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Bookmark_DTO = Bookmarky.DTO.Bookmark;
using Bookmark_DB = Bookmarky.DAL.EntityModels.Bookmark;
using Bookmarky.DAL;
using Bookmarky.DAL.EntityModels;
using System.Data.Entity;
using Bookmarky.DAL.ServiceImplementations;

namespace Bookmarky.Test.Unit_Tests
{
	[TestClass]
	public class BookmarkDataService_UT
	{
		[TestMethod]
		public void SaveBookmark_UpdateExisting()
		{
			var mockContext = new Mock<IBookmarkyContext>();
			var mockSet = new Mock<DbSet<Bookmark_DB>>();

			mockSet.Setup(s => s.Find(It.IsAny<int>())).Returns(new Bookmark_DB());
			mockContext.Setup(c => c.Set<Bookmark_DB>()).Returns(mockSet.Object);
			mockContext.Setup(c => c.SaveChanges());

			var bm = new Bookmark_DTO
			{
				Id = 1,
				Title = "test bm"
			};

			var service = new EFBookmarkDataService(mockContext.Object);

			service.SaveBookmark(bm);

			mockSet.Verify(s => s.Find(It.IsAny<int>()), Times.Once);
			mockContext.Verify(s => s.SaveChanges(), Times.Once);

		}

		[TestMethod]
		public void SaveBookmark_CreateNew()
		{
			var mockContext = new Mock<IBookmarkyContext>();
			var mockSet = new Mock<DbSet<Bookmark_DB>>();

			mockSet.Setup(s => s.Find(It.IsAny<int>())).Returns(new Bookmark_DB());
			mockContext.Setup(c => c.Set<Bookmark_DB>()).Returns(mockSet.Object);
			mockContext.Setup(c => c.SaveChanges());

			var bm = new Bookmark_DTO
			{
				Title = "test bm"
			};

			var service = new EFBookmarkDataService(mockContext.Object);

			service.SaveBookmark(bm);

			mockSet.Verify(s => s.Find(It.IsAny<int>()), Times.Never);
			mockContext.Verify(s => s.SaveChanges(), Times.Once);
		}
	}
}
