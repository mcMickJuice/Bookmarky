using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DAL.Mapping;
using Bookmarky.DAL.Service;
using Bookmarky.DAL.ServiceImplementations;
using Bookmarky.Utility.Extensions;
using NUnit.Framework;
using Bookmark_DTO = Bookmarky.DTO.Bookmark;
using Tag_DTO = Bookmarky.DTO.Tag;

namespace Bookmarky.Test.Integration_Tests
{
    [TestFixture]
    public class BookmarkInsert_IntegrationTest
    {
        private IBookmarkyContext _context;
        private IBookmarkDataService _service;
        private List<Tag> existingTags;

        [SetUp]
        public void Init()
        {
            var tag1 = new Tag() { TagName = "Javascript" };
            var tag2 = new Tag() { TagName = "C#" };
            var tag3 = new Tag() { TagName = "Web" };
            var tag4 = new Tag() { TagName = "Server Side" };
            var tag5 = new Tag() { TagName = "Database" };

            existingTags = new List<Tag>
            {
                tag1,
                tag2,
                tag3,
                tag4,
                tag5
            };

            using (var ctx = new BookmarkyContext("BookmarkyContextTest"))
            {
                ctx.Set<Tag>().AddRange(existingTags);
                ctx.SaveChanges();
            }

            _context = new BookmarkyContext("BookmarkyContextTest");
            var mapper = new BookmarkyMapper(new DefaultValueInjection());
            _service = new EFBookmarkDataService(_context, mapper);
        }

        [Test]
        public void CreateBookmark_NoTags()
        {
            //Arrange
            var bookmark = new Bookmark_DTO
            {
                Title = "Sample Bookmark 1",
                Gist = "Just a Gist",
                ResourceType = (int)ResourceType.Article,
                Tags = new List<Tag_DTO>()
            };
            //Act
            var savedBookmark = _service.CreateBookmark(bookmark);

            var queriedBookmark = _context.Set<Bookmark>().Find(savedBookmark.Id);
            //Assert
            Assert.That(savedBookmark.Title == queriedBookmark.Title);
            Assert.That(savedBookmark.ResourceType == (int)queriedBookmark.ResourceType);
        }

        [Test]
        public void CreateBookmark_WithExistingTags()
        {
            //Arrange 
            var bookmark = new Bookmark_DTO
            {
                Title = "Sample Bookmark 1",
                Gist = "Just a Gist",
                ResourceType = (int) ResourceType.Video,
                Tags = new List<Tag_DTO>()
                {
                    new Tag_DTO(){Id = existingTags[0].Id, TagName = existingTags[0].TagName},
                    new Tag_DTO(){Id = existingTags[1].Id, TagName = existingTags[1].TagName},
                }
            };
            //Act
            var savedBookmark = _service.CreateBookmark(bookmark);

            var queriedBookmark = _context.Set<Bookmark>().Find(savedBookmark.Id);
            //Assert
            Assert.That(savedBookmark.Tags.Count() == queriedBookmark.Tags.Count);
            var tagIds = savedBookmark.Tags.Select(t => t.Id);
            var exceptions = queriedBookmark.Tags.Select(t => t.Id)
                .Except(tagIds);
            Assert.IsFalse(exceptions.Any());
        }

        [TearDown]
        public void Teardown()
        {
            var bmSet = _context.Set<Bookmark>();
            var bookmarks = bmSet.ToList();
            bmSet.RemoveRange(bookmarks);

            var tagSet = _context.Set<Tag>();
            var tags = tagSet.ToList();
            tagSet.RemoveRange(tags);

            _context.SaveChanges();
        }
    }
}
