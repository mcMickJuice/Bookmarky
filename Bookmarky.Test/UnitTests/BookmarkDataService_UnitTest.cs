using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DAL.Mapping;
using Bookmarky.DAL.ServiceImplementations;
using Bookmarky.DTO;
using Bookmarky.Utility.Extensions;
using Moq;
using NUnit.Framework;
using Tag_Dto = Bookmarky.DTO.Tag;
using Tag_Db = Bookmarky.DAL.EntityModels.Tag;
using Bookmark_Dto = Bookmarky.DTO.Bookmark;
using Bookmark_Db = Bookmarky.DAL.EntityModels.Bookmark;

namespace Bookmarky.Test.UnitTests
{
	
    [TestFixture]
	public class BookmarkDataService_UnitTest
    {
        private Mock<IBookmarkyContext> _mockContext;
        private Mock<DbSet<Bookmark_Db>> _mockSet;
        private IBookmarkyMapper _mapper;

        [SetUp]
        public void Init()
        {
            _mockContext = new Mock<IBookmarkyContext>();
            _mockSet = new Mock<DbSet<Bookmark_Db>>();
            _mapper = new BookmarkyMapper(new DefaultValueInjection());
        }                 

		[Test]
		public void SaveBookmark_UpdateExisting_NoTags()
		{
		    //_mockSet.Setup(s => s.Where(It.IsAny<Expression<Func<Bookmark_Db,bool>>>())).Returns(new List<Bookmark_Db> {new Bookmark_Db() { Tags = new List<DAL.EntityModels.Tag>()}}.AsQueryable);
            _mockContext.Setup(c => c.Set<Bookmark_Db>()).Returns(_mockSet.Object);
            _mockContext.Setup(c => c.SaveChanges());

			var bm = new DTO.Bookmark
			{
				Id = 1,
				Title = "test bm",
                Tags = new List<Tag_Dto>
                {
                    
                }
			};

            var service = new EFBookmarkDataService(_mockContext.Object, _mapper);

			service.UpdateBookmark(bm);

            _mockSet.Verify(s => s.Find(It.IsAny<int>()), Times.Once);
            _mockContext.Verify(s => s.SaveChanges(), Times.Once);

		}

		[Test]
		public void SaveBookmark_CreateNew_NoTags()
		{
            _mockSet.Setup(s => s.Find(It.IsAny<int>())).Returns(new Bookmark_Db());
            _mockSet.Setup(s => s.Add(It.IsAny<Bookmark_Db>()));
            _mockContext.Setup(c => c.Set<Bookmark_Db>()).Returns(_mockSet.Object);
            _mockContext.Setup(c => c.SaveChanges());

			var bm = new DTO.Bookmark
			{
				Title = "test bm",
                Tags = new List<Tag_Dto>()
			};

            var service = new EFBookmarkDataService(_mockContext.Object, _mapper);

			service.CreateBookmark(bm);

            _mockSet.Verify(s => s.Add(It.IsAny<Bookmark_Db>()), Times.Once);
            _mockContext.Verify(s => s.SaveChanges(), Times.Once);
		}

        [Test]
        public void SaveBookmark_Update_WithNewTags()
        {
            //Arrange
            var newTags = new List<Tag_Dto>
            {
                new Tag_Dto() {Id = 1, TagName = "C#"},
                new Tag_Dto() {Id = 2, TagName = "Microsoft"},
                new Tag_Dto() {Id = 9, TagName = "Microsoft"},
                new Tag_Dto() {Id = 199, TagName = "Microsoft"},
            };

            var existingTags = new List<Tag_Dto>()
            {
                new Tag_Dto() {Id = 5, TagName = "Async"}
            };

            var dtoBookmark = new Bookmark_Dto()
            {
                Gist = "This is a test", 
                Tags = newTags.Concat(existingTags)
            };
            var dbBookmark = new Bookmark_Db()
            {
                Tags = new List<Tag_Db>()
                {
                    new Tag_Db(){Id = 5}
                }
            };

            _mockContext.Setup(m => m.Set<Bookmark_Db>()).Returns(_mockSet.Object);
            _mockSet.Setup(m => m.Find(It.IsAny<int>())).Returns(dbBookmark);

            //Act
            var service = new EFBookmarkDataService(_mockContext.Object, _mapper);

            service.CreateBookmark(dtoBookmark);

            //Assert
            newTags.ForEach(newT => 
                Assert.That(dtoBookmark.Tags
                .Select(t => t.Id)
                .Contains(newT.Id)));
        }

        [Test]
        public void SaveBookmark_Update_WithDeletedTags()
        {
            //Arrange
            var dbTags = new List<Tag_Db>
            {
                new Tag_Db() {Id = 1, TagName = "C#"},
                new Tag_Db() {Id = 2, TagName = "Microsoft"},
                new Tag_Db() {Id = 9, TagName = "Microsoft"},
                new Tag_Db() {Id = 199, TagName = "Microsoft"},
            };

            var dtoTags = new List<Tag_Dto>
            {
                new Tag_Dto() {Id = 1, TagName = "C#"},
                new Tag_Dto() {Id = 9, TagName = "Microsoft"},
            };

            var deletedTags = dbTags.Where(dbT => !dtoTags.Select(t => t.Id).Contains(dbT.Id));

            var dtoBookmark = new Bookmark_Dto()
            {
                Gist = "This is a test",
                Tags = dtoTags
            };
            var dbBookmark = new Bookmark_Db()
            {
                Tags = dbTags
            };

            _mockContext.Setup(m => m.Set<Bookmark_Db>()).Returns(_mockSet.Object);
            _mockSet.Setup(m => m.Find(It.IsAny<int>())).Returns(dbBookmark);

            //Act
            var service = new EFBookmarkDataService(_mockContext.Object, _mapper);
            service.UpdateBookmark(dtoBookmark);

            //Assert
            deletedTags.ToList().ForEach(t =>
                Assert.That(!dbBookmark.Tags.Select(dbT => dbT.Id)
                    .Contains(t.Id)));

        }
	}
}
