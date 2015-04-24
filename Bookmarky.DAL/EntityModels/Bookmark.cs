using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookmarky.DAL.EntityModels
{
    public class Bookmark
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public ResourceType ResourceType { get; set; }
		public string Gist { get; set; }

		public bool IsRead { get; set; }

		public int? SourceId { get; set; }

		public DateTime? CreatedDate { get; set; }

		public virtual Source Source { get; set; }
		public virtual Rating Rating { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
	}
}
