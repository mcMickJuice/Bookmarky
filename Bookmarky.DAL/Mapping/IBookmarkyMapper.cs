using System;
using System.Text;
using System.Threading.Tasks;
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
}
