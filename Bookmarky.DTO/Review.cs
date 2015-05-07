using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DTO
{
    public class Review
    {
        public int Id { get; set; }
        public string Overview { get; set; }
        public int Score { get; set; }
        public int BookmarkId { get; set; }
    }
}
