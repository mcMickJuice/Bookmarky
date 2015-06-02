using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bookmarky.DAL.EntityModels
{

	public class Rating
	{
		public int Id { get; set; }
		public string Overview { get; set; }
		[Range(1,5)]
		public int Score { get; set; }
        //public int BookmarkId { get; set; }
		public DateTime? CreatedDate { get; set; }

		public Bookmark Bookmark { get; set; }
	}
}
