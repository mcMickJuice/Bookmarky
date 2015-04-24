using System;
using System.Collections.Generic;
using System.Linq;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DTO;
using NUnit.Framework;
using Bookmark = Bookmarky.DAL.EntityModels.Bookmark;

namespace Bookmarky.Test.UnitTests.PredicateBuilderTests
{
    [TestFixture]
    public class BookmarkSearchCriteriaPredicateBuilder_UnitTest : BasePredicateBuilderTest
    {
        [Test]
        public void SearchByTitle_CaseInsensitive_Found()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing"
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now}
            };

            var searchCriteria = new BookmarkSearchCriteria
            {
                TitleText = "mike",
            };
            
            var result = applyPredicateToList(bookmarkList, searchCriteria);

            Assert.AreEqual(2,result.Count);
            var ids = result.Select(b => b.Id);
            Assert.IsTrue(!ids.Except(new List<int>{1,34}).Any());
        }

        [Test]
        public void SearchByTitle_CaseInsensitive_NotFound()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing"
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now}
            };

            var searchCriteria = new BookmarkSearchCriteria
            {
                TitleText = "samantha jenkins",
            };

            var result = applyPredicateToList(bookmarkList, searchCriteria);

            Assert.That(!result.Any());
        }

        [Test]
        public void SearchByMinimumRating_Found()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing",
                    Rating = new Rating(){Score = 3}
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                MinimumRating = 2
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.IsTrue(results.Count == 1);
            Assert.That(results.FirstOrDefault().Id == 1);
        }

        [Test]
        public void SearchByMinimumRating_NotFound()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    Gist = "Just an awesome thing",
                    Rating = new Rating(){Score = 3}
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3)},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                MinimumRating = 5
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.IsTrue(!results.Any());
         
        }


        [Test]
        public void SearchByResourceType_Single_Found()
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
                    ResourceType = ResourceType.Video
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3),ResourceType = ResourceType.Reference},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                ResourceTypeIds = new List<int>() { 3}
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.AreEqual(1, results.Count);
            Assert.That(results.FirstOrDefault().Id == 1);
        }

        [Test]
        public void SearchByResourceType_Single_NotFound()
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
                    ResourceType = ResourceType.Video
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3),ResourceType = ResourceType.Reference},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                ResourceTypeIds = new List<int>() { 2 }
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.That(!results.Any());
        }


        [Test]
        public void SearchByResourceType_Multiple_Found()
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
                    ResourceType = ResourceType.Video
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3),ResourceType = ResourceType.Reference},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, ResourceType = ResourceType.Article}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                ResourceTypeIds = new List<int>() { 3, 1 }
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.AreEqual(2, results.Count);
            var ids = results.Select(r => r.Id);
            Assert.That(!ids.Except(new List<int>{1,34}).Any());
        }

        [Test]
        public void SearchByResourceType_Multiple_NotFound()
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
                    ResourceType = ResourceType.Video
                },
                new Bookmark() {Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3),},
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now, Rating = new Rating{Score = 1}, ResourceType = ResourceType.Article}
            };

            var criteria = new BookmarkSearchCriteria()
            {
                ResourceTypeIds = new List<int>() { 4, 2 }
            };

            var results = applyPredicateToList(bookmarkList, criteria);

            Assert.That(!results.Any());
        }
        
    }
}
