using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DTO;

namespace Bookmarky.DAL.Service
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAllTags();
        int CreateTag(Tag tag);
    }
}
