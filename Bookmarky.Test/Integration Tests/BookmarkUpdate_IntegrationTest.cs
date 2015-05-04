using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DAL.Mapping;
using Bookmarky.DAL.Service;
using Bookmarky.DAL.ServiceImplementations;
using Bookmarky.Utility.Extensions;
using LinqKit;
using NUnit.Framework;
using Bookmark_DTO = Bookmarky.DTO.Bookmark;
using Tag_DTO = Bookmarky.DTO.Tag;

namespace Bookmarky.Test.Integration_Tests
{
    [TestFixture]
    public class BookmarkUpdate_IntegrationTest
    {
        private IBookmarkyContext _context;
        private IBookmarkDataService _service;
        private List<Tag> existingTags;
        private Bookmark existingBookmark;

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

            existingBookmark = new Bookmark
            {
                Title = "Original Title",
                Gist = "Original Gist",
                ResourceType = ResourceType.Reference,
                Tags = new Collection<Tag>
                {
                    tag1,
                    tag2
                }
            };

            using (var ctx = new BookmarkyContext("BookmarkyContextTest"))
            {
                existingBookmark.Tags.ForEach(t =>
                {
                    ctx.Set<Tag>().Attach(t);
                });

                ctx.Set<Bookmark>().Add(existingBookmark);
                ctx.SaveChanges();
            }

            _context = new BookmarkyContext("BookmarkyContextTest");
            var mapper = new BookmarkyMapper(new DefaultValueInjection());
            _service = new EFBookmarkDataService(_context, mapper);
        }

        [Test]
        public void UpdateBookmark_NoNewTags()
        {
            //Arrange
            var updatedBookmark = new Bookmark_DTO
            {
                Id = existingBookmark.Id,
                Title = "New Title",
                Gist = "New Gist",
                ResourceType = (int) ResourceType.Tutorial,
                Tags = new List<Tag_DTO>
                {
                    new Tag_DTO() {Id = existingTags[0].Id, TagName = existingTags[0].TagName},
                    new Tag_DTO() {Id = existingTags[1].Id, TagName = existingTags[1].TagName},
                }
            };

            //Act
            _service.UpdateBookmark(updatedBookmark);

            var dbBookmark = _context.Set<Bookmark>().Find(updatedBookmark.Id);

            //Assert
            Assert.That(updatedBookmark.Title == dbBookmark.Title);
            Assert.That(updatedBookmark.Gist == dbBookmark.Gist);
            Assert.That(updatedBookmark.ResourceType == (int)dbBookmark.ResourceType);

            Assert.That(updatedBookmark.Tags.Count() == dbBookmark.Tags.Count);
            var tagIds = updatedBookmark.Tags.Select(t => t.Id);
            var exceptions = dbBookmark.Tags.Select(t => t.Id)
                .Except(tagIds);
            Assert.IsFalse(exceptions.Any());
        }

        [Test]
        public void UpdateBookmark_NewTags()
        {
            //Arrange

            var newTag = new Tag_DTO() {Id = existingTags[2].Id, TagName = existingTags[2].TagName};
            var updatedBookmark = new Bookmark_DTO
            {
                Id = existingBookmark.Id,
                Title = "New Title",
                Gist = "New Gist",
                ResourceType = (int)ResourceType.Tutorial,
                Tags = new List<Tag_DTO>
                {
                    new Tag_DTO() {Id = existingTags[0].Id, TagName = existingTags[0].TagName},
                    new Tag_DTO() {Id = existingTags[1].Id, TagName = existingTags[1].TagName},
                    newTag,
                }
            };

            //Act
            _service.UpdateBookmark(updatedBookmark);

            var dbBookmark = _context.Set<Bookmark>().Find(updatedBookmark.Id);

            //Assert
            Assert.That(updatedBookmark.Tags.Count() == dbBookmark.Tags.Count);
            var tagIds = updatedBookmark.Tags.Select(t => t.Id);
            var exceptions = dbBookmark.Tags.Select(t => t.Id)
                .Except(tagIds);
            Assert.IsFalse(exceptions.Any());

            Assert.IsTrue(dbBookmark.Tags.Any(t => t.Id == newTag.Id));
        }

        [Test]
        public void UpdateBookmark_RemovedTags()
        {
            //Arrange

            var removedTag = new Tag_DTO() {Id = existingTags[1].Id, TagName = existingTags[1].TagName};
            var updatedBookmark = new Bookmark_DTO
            {
                Id = existingBookmark.Id,
                Title = "New Title",
                Gist = "New Gist",
                ResourceType = (int)ResourceType.Tutorial,
                Tags = new List<Tag_DTO>
                {
                    new Tag_DTO() {Id = existingTags[0].Id, TagName = existingTags[0].TagName},
                }
            };

            //Act
            _service.UpdateBookmark(updatedBookmark);

            var dbBookmark = _context.Set<Bookmark>().Find(updatedBookmark.Id);

            //Assert
            Assert.That(updatedBookmark.Tags.Count() == dbBookmark.Tags.Count);
            var tagIds = updatedBookmark.Tags.Select(t => t.Id);
            var exceptions = dbBookmark.Tags.Select(t => t.Id)
                .Except(tagIds);
            Assert.IsFalse(exceptions.Any());

            Assert.IsFalse(dbBookmark.Tags.Any(t => t.Id == removedTag.Id));
        }

        [Test]
        public void UpdateBookmark_NewAndRemovedTags()
        {
            //Arrange

            var removedTag = new Tag_DTO() { Id = existingTags[1].Id, TagName = existingTags[1].TagName };
            var newTag1 = new Tag_DTO() { Id = existingTags[2].Id, TagName = existingTags[2].TagName };
            var newTag2 = new Tag_DTO() { Id = existingTags[3].Id, TagName = existingTags[3].TagName };

            var updatedBookmark = new Bookmark_DTO
            {
                Id = existingBookmark.Id,
                Title = "New Title",
                Gist = "New Gist",
                ResourceType = (int)ResourceType.Tutorial,
                Tags = new List<Tag_DTO>
                {
                    new Tag_DTO() {Id = existingTags[0].Id, TagName = existingTags[0].TagName},
                    newTag1,
                    newTag2
                }
            };

            //Act
            _service.UpdateBookmark(updatedBookmark);

            var dbBookmark = _context.Set<Bookmark>().Find(updatedBookmark.Id);

            //Assert
            Assert.That(updatedBookmark.Tags.Count() == dbBookmark.Tags.Count);
            var tagIds = updatedBookmark.Tags.Select(t => t.Id);
            var dbTagIds = dbBookmark.Tags.Select(t => t.Id).ToList();
            var exceptions = dbTagIds
                .Except(tagIds);
            Assert.IsFalse(exceptions.Any());

            Assert.IsFalse(dbBookmark.Tags.Any(t => t.Id == removedTag.Id));
            Assert.IsTrue(dbTagIds.Contains(newTag1.Id) && dbTagIds.Contains(newTag2.Id));
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
