using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DTO
{
    public class Bookmark
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int ResourceType { get; set; }
        public string Gist { get; set; }
        public bool IsRead { get; set; }
        public int? SourceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public Review Review { get; set; }
    }
}
