using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DAL.EntityModels;
using Bookmarky.DAL.Mapping;
using Bookmarky.Utility.Extensions;
using NUnit.Framework;
using Bookmark_Dto = Bookmarky.DTO.Bookmark;
using Bookmark_Db = Bookmarky.DAL.EntityModels.Bookmark;

namespace Bookmarky.Test.UnitTests.Mapping
{
    [TestFixture]
    public class BookmarkMapping_UnitTest
    {
        private IBookmarkyMapper _mapper = new BookmarkyMapper(new DefaultValueInjection());


        [Test]
        public void Bookmark_MapRegularBookmark_To_Db()
        {
            var bookmarkDto = new Bookmark_Dto
            {
                Title = "Just a title",
                Gist = "Gist Gist Gist",
                ResourceType = (int)ResourceType.Video,
                Url = "www.website.com"
            };

            var bookmarkDb = _mapper.MapToBookmarkDb(bookmarkDto);

            Assert.That(bookmarkDto.Title == bookmarkDb.Title);
            Assert.That(bookmarkDto.Gist == bookmarkDb.Gist);
            Assert.That(bookmarkDto.ResourceType == (int)bookmarkDb.ResourceType);
            Assert.That(bookmarkDto.Url == bookmarkDb.Url);
            Assert.IsInstanceOf<Bookmark_Db>(bookmarkDb);
        }

        [Test]
        public void Bookmark_Ensure_IsStickyGetter()
        {
            var stickyDto = new Bookmark_Dto
            {
                Title = "Just a title",
                Gist = "Gist Gist Gist",
                ResourceType = (int)ResourceType.Video,
                Url = "www.website.com",
                LinkIconUrl = "www.image.com",
            };

            Assert.That(stickyDto.IsStickied);
        }

        [Test]
        public void Bookmark_MapRegularBookmark_To_DTO()
        {
            var bookmarkDb = new Bookmark_Db
            {
                Title = "Just a title",
                Gist = "Gist Gist Gist",
                ResourceType = ResourceType.Video,
                Url = "www.website.com"
            };

            var dtoBm = _mapper.MapToBookmarkDto(bookmarkDb);

            Assert.IsFalse(dtoBm.IsStickied);
        }


    }
}
