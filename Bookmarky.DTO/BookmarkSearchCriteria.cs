using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DTO
{
    public class BookmarkSearchCriteria
    {
        public LogicType AndOrFieldLogic { get; set; }
        public int? MinimumRating { get; set; }
        public string TitleText { get; set; }

        public IEnumerable<int> TagIds { get; set; }
        public InclusionType AnyAllTagLogic { get; set; }

        public IEnumerable<int> ResourceTypeIds { get; set; }

        public BookmarkSearchCriteria()
        {
            TagIds = new List<int>();
            ResourceTypeIds = new List<int>();
            AndOrFieldLogic = LogicType.And;
            AnyAllTagLogic = InclusionType.Any;
        }

    }
}
