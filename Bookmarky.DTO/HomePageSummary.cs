using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DTO
{
    public class HomePageSummary
    {
        public IEnumerable<Bookmark> RecentBookmarks { get; set; }
        public IEnumerable<Bookmark> StickiedBookmarks { get; set; }
    }
}
