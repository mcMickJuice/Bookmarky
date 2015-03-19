using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DAL.EntityModels
{
	public class Source
	{
		public int Id { get; set; }
		public string SourceName { get; set; }
		public string Url { get; set; }
		public virtual ICollection<Bookmark> Bookmarks { get; set; }
		public DateTime? CreatedDate { get; set; }

	}
}
