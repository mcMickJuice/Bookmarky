using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DTO
{
    public class BookmarkSearchCriteria
    {
        public bool IsAnd { get; set; }
        public int? MinimumRating { get; set; }
        public string TitleText { get; set; }

        public List<int> TagIds { get; set; }
        public bool IsAllTags { get; set; }

        public List<int> ResourceTypeIds { get; set; }

        public BookmarkSearchCriteria()
        {
            TagIds = new List<int>();
            ResourceTypeIds = new List<int>();
            IsAnd = true;
        }

    }

    public class BookmarkSearchFieldInitialization
    {
        public List<Tag> AvailableTags { get; set; }
    }

    public class BookmarkSearchInitialization
    {
        public BookmarkSearchCriteria SearchCriteria { get; set; }
        public BookmarkSearchFieldInitialization FieldInitialization { get; set; }
    }
}
