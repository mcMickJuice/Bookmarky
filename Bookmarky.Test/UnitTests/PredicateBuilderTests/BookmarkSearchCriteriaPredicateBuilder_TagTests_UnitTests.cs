using System;
using System.Collections.Generic;
using System.Linq;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DTO;
using NUnit.Framework;
using Bookmark = Bookmarky.DAL.EntityModels.Bookmark;
using Tag = Bookmarky.DAL.EntityModels.Tag;

namespace Bookmarky.Test.UnitTests.PredicateBuilderTests
{
    [TestFixture]
    public class BookmarkSearchCriteriaPredicateBuilder_TagTests_UnitTests: BasePredicateBuilderTest
    {
        [Test]
        public void SearchByTags_Single_Found()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing",
                    Rating = new Rating(){Score = 3},
                    Tags = new List<Tag>
                    {
                        new Tag(){Id = 3},
                        new Tag(){Id = 45},
                        new Tag(){Id = 1},
                    }
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)
                    , Tags = new List<Tag>
                    {
                        new Tag(){Id = 1},
                    }},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, Tags = new List<Tag>
                {
                    new Tag(){Id = 2},
                }}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                TagIds = new List<int>() { 1 }
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.That(results.Count == 2);
            var bmIds = results.Select(bm => bm.Id);
            Assert.That(!bmIds.Except(new List<int> { 1, 2 }).Any());
        }

        [Test]
        public void SearchByTags_Single_NotFound()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing",
                    Rating = new Rating(){Score = 3},
                    Tags = new List<Tag>
                    {
                        new Tag(){Id = 3},
                        new Tag(){Id = 45},
                        new Tag(){Id = 1},
                    }
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)
                    , Tags = new List<Tag>
                    {
                        new Tag(){Id = 1},
                    }},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, Tags = new List<Tag>
                {
                    new Tag(){Id = 2},
                }}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                TagIds = new List<int>() { 109 }
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.That(!results.Any());
        }

        [Test]
        public void SearchByTags_Multiple_AllTags_Found()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing",
                    Rating = new Rating(){Score = 3},
                    Tags = new List<Tag>
                    {
                        new Tag(){Id = 3},
                        new Tag(){Id = 45},
                        new Tag(){Id = 1},
                    }
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)
                    , Tags = new List<Tag>
                    {
                        new Tag(){Id = 1},
                        new Tag(){Id = 67},
                    }},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, Tags = new List<Tag>
                {
                    new Tag(){Id = 2},
                }}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                TagIds = new List<int>() { 1, 67 },
                AnyAllTagLogic = InclusionType.All
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.That(results.Count == 1);
            var bmIds = results.Select(bm => bm.Id);
            Assert.That(bmIds.FirstOrDefault() == 2);
        }

        [Test]
        public void SearchByTags_Multiple_AllTags_NotFound()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing",
                    Rating = new Rating(){Score = 3},
                    Tags = new List<Tag>
                    {
                        new Tag(){Id = 3},
                        new Tag(){Id = 45},
                        new Tag(){Id = 1},
                    }
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)
                    , Tags = new List<Tag>
                    {
                        new Tag(){Id = 1},
                        new Tag(){Id = 67},
                    }},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, Tags = new List<Tag>
                {
                    new Tag(){Id = 2},
                }}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                TagIds = new List<int>() { 1, 67,999 },
                AnyAllTagLogic = InclusionType.All
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.That(!results.Any());
        }

        [Test]
        public void SearchByTags_Multiple_AnyTags_Found()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing",
                    Rating = new Rating(){Score = 3},
                    Tags = new List<Tag>
                    {
                        new Tag(){Id = 3},
                        new Tag(){Id = 45},
                        new Tag(){Id = 1},
                    }
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)
                    , Tags = new List<Tag>
                    {
                        new Tag(){Id = 1},
                        new Tag(){Id = 67},
                    }},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, Tags = new List<Tag>
                {
                    new Tag(){Id = 2},
                }},
                new Bookmark() {Id = 4, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, Tags = new List<Tag>
                {
                    new Tag(){Id = 2},
                    new Tag(){Id = 67},
                }}
            };



            var criteria = new BookmarkSearchCriteria()
            {
                TagIds = new List<int>() { 1, 67 },
                AnyAllTagLogic = InclusionType.Any
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.That(results.Count == 3);
            var bmIds = results.Select(bm => bm.Id);
            Assert.That(!bmIds.Except(new List<int>{1,2,4}).Any());
        }

        [Test]
        public void SearchByTags_Multiple_AnyTags_NotFound()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing",
                    Rating = new Rating(){Score = 3},
                    Tags = new List<Tag>
                    {
                        new Tag(){Id = 3},
                        new Tag(){Id = 45},
                        new Tag(){Id = 1},
                    }
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)
                    , Tags = new List<Tag>
                    {
                        new Tag(){Id = 1},
                        new Tag(){Id = 67},
                    }},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, Tags = new List<Tag>
                {
                    new Tag(){Id = 2},
                }},
                new Bookmark() {Id = 4, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, Tags = new List<Tag>
                {
                    new Tag(){Id = 2},
                    new Tag(){Id = 67},
                }}
            };



            var criteria = new BookmarkSearchCriteria()
            {
                TagIds = new List<int>() { 11, 63,10987 },
                AnyAllTagLogic = InclusionType.Any
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.That(!results.Any());
        }
    }
}