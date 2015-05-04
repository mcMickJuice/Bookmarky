using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.Utility.Extensions;
using Omu.ValueInjecter;
using Bookmark_Dto = Bookmarky.DTO.Bookmark;
using Bookmark_Db = Bookmarky.DAL.EntityModels.Bookmark;
using Tag_Dto = Bookmarky.DTO.Tag;
using Tag_Db = Bookmarky.DAL.EntityModels.Tag;

namespace Bookmarky.DAL.Mapping
{
    public interface IBookmarkyMapper
    {
        Bookmark_Dto MapToBookmarkDto(Bookmark_Db bookmarkDb);
        Bookmark_Db MapToBookmarkDb(Bookmark_Dto bookmarkDto);
        Tag_Dto MapToTagDto(Tag_Db tagDb);
        Tag_Db MapToTagDb(Tag_Dto tagDto);
    }

    public class BookmarkyMapper : IBookmarkyMapper
    {
        private readonly ConventionInjection _convention;

        public BookmarkyMapper(ConventionInjection convention)
        {
            _convention = convention;
        }

        public Bookmark_Dto MapToBookmarkDto(Bookmark_Db bookmarkDb)
        {
            var bookmarkDto = bookmarkDb.MapTo<Bookmark_Dto>(_convention);

            if (bookmarkDb.Tags.Any())
                bookmarkDto.Tags = bookmarkDb.Tags.Select(MapToTagDto);

            return bookmarkDto;
        }

        public Bookmark_Db MapToBookmarkDb(Bookmark_Dto bookmarkDto)
        {
            var bookmarkDb = bookmarkDto.MapTo<Bookmark_Db>(_convention);

            if (bookmarkDto.Tags.Any())
            {
                bookmarkDb.Tags = bookmarkDto.Tags.Select(MapToTagDb).ToList();
                
            }
            else
            {
                bookmarkDb.Tags = new List<Tag_Db>();
            }

            return bookmarkDb;
        }

        public Tag_Dto MapToTagDto(Tag_Db tagDb)
        {
            var tagDto = tagDb.MapTo<Tag_Dto>(_convention);

            return tagDto;
        }

        public Tag_Db MapToTagDb(Tag_Dto tagDto)
        {
            var tagDb = tagDto.MapTo<Tag_Db>(_convention);

            return tagDb;
        }
    }
}
