using System.Data.Entity;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DAL.ServiceImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bookmarky.Test.UnitTests
{
	[TestClass]
	public class BookmarkDataService_UnitTest
	{
		[TestMethod]
		public void SaveBookmark_UpdateExisting()
		{
			var mockContext = new Mock<IBookmarkyContext>();
			var mockSet = new Mock<DbSet<Bookmark>>();

			mockSet.Setup(s => s.Find(It.IsAny<int>())).Returns(new Bookmark());
			mockContext.Setup(c => c.Set<Bookmark>()).Returns(mockSet.Object);
			mockContext.Setup(c => c.SaveChanges());

			var bm = new DTO.Bookmark
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
			var mockSet = new Mock<DbSet<Bookmark>>();

			mockSet.Setup(s => s.Find(It.IsAny<int>())).Returns(new Bookmark());
			mockContext.Setup(c => c.Set<Bookmark>()).Returns(mockSet.Object);
			mockContext.Setup(c => c.SaveChanges());

			var bm = new DTO.Bookmark
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
