using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DTO;
using NUnit.Framework;
using Bookmark = Bookmarky.DAL.EntityModels.Bookmark;

namespace Bookmarky.Test.UnitTests.PredicateBuilderTests
{
    [TestFixture]
    public class MultipleCriteriaBookmarkSearchCriteriaPredicateBuilder_UnitTest : BasePredicateBuilderTest
    {
        [Test]
        public void MultipleCriteria_Found()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    Rating = new Rating(){ Score = 4},
                    ResourceType = ResourceType.Reference,
                    CreatedDate = DateTime.Now.AddDays(-8),
                },
                new Bookmark()
                {
                    Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3),
                    Rating = new Rating(){ Score = 5},
                    ResourceType = ResourceType.Video,
                },
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now},
                new Bookmark() {Id = 39, Title = "All abot Mike", CreatedDate = DateTime.Now, ResourceType = ResourceType.Reference},
                new Bookmark() {Id = 101, Title = "All abot Mike", CreatedDate = DateTime.Now, ResourceType = ResourceType.Reference, Rating = new Rating(){Score = 5}},

            };

            var searchCriteria = new BookmarkSearchCriteria
            {
                MinimumRating = 4,
                ResourceTypeIds = new List<int> { (int)ResourceType.Reference },
                AndOrFieldLogic = LogicType.And
            };

            var result = applyPredicateToList(bookmarkList, searchCriteria);

            Assert.AreEqual(2,result.Count);
            var ids = result.Select(r => r.Id);
            Assert.That(!ids.Except(new List<int>{1,101}).Any());
        }

        [Test]
        public void MultipleCriteria_NotFound()
        {
            var bookmarkList = new List<Bookmark>
            {
                new Bookmark()
                {
                    Id = 1,
                    Title = "Mike's awesome blog",
                    Rating = new Rating(){ Score = 4},
                    ResourceType = ResourceType.Reference,
                    CreatedDate = DateTime.Now.AddDays(-8),
                },
                new Bookmark()
                {
                    Id = 2, Title = "All about Javascript", CreatedDate = DateTime.Now.AddDays(-3),
                    Rating = new Rating(){ Score = 5},
                    ResourceType = ResourceType.Video,
                },
                new Bookmark() {Id = 34, Title = "All about the like of Mike", CreatedDate = DateTime.Now},
                new Bookmark() {Id = 39, Title = "All abot Mike", CreatedDate = DateTime.Now, ResourceType = ResourceType.Reference},
                new Bookmark() {Id = 101, Title = "All abot Mike", CreatedDate = DateTime.Now, ResourceType = ResourceType.Reference, Rating = new Rating(){Score = 5}},

            };

            var searchCriteria = new BookmarkSearchCriteria
            {
                MinimumRating = 5,
                ResourceTypeIds = new List<int> { (int)ResourceType.Tutorial },
                AndOrFieldLogic = LogicType.And
            };

            var result = applyPredicateToList(bookmarkList, searchCriteria);

            Assert.That(!result.Any());
         
        }
    }
}
