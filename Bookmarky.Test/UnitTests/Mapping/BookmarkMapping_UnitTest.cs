using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DAL.EntityModels;
using Bookmarky.Utility.Extensions;
using NUnit.Framework;
using Bookmark_Dto = Bookmarky.DTO.Bookmark;
using Bookmark_Db = Bookmarky.DAL.EntityModels.Bookmark;

namespace Bookmarky.Test.UnitTests.Mapping
{
    [TestFixture]
    public class BookmarkMapping_UnitTest
    {
        [Test]
        public void Bookmark_MapDtoToEntity()
        {
            var bookmarkDto = new Bookmark_Dto
            {
                Title = "Just a title",
                Gist = "Gist Gist Gist",
                ResourceType = (int) ResourceType.Video,
                Url = "www.butt.com"
            };

            var bookmarkDb = bookmarkDto.MapTo<Bookmark_Db>(new DefaultValueInjection());

            Assert.That(bookmarkDto.Title == bookmarkDb.Title);
            Assert.That(bookmarkDto.Gist == bookmarkDb.Gist);
            Assert.That(bookmarkDto.ResourceType == (int)bookmarkDb.ResourceType);
            Assert.That(bookmarkDto.Url == bookmarkDb.Url);
        }
    }
}
