using System.Collections.Generic;

namespace Bookmarky.DTO
{
    public class BookmarkSearchFieldInitialization
    {
        public IEnumerable<Tag> AvailableTags { get; set; }
        public IEnumerable<object> Resources { get; set; }
        public IEnumerable<object> LogicTypes { get; set; }
        public IEnumerable<object> InclusionTypes { get; set; }
    }
}