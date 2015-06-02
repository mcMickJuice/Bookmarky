using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmarky.DAL.EntityModels;

namespace Bookmarky.DAL.Schema
{
	public class ContextInit : System.Data.Entity.DropCreateDatabaseIfModelChanges<Bookmarky.DAL.EntityModels.BookmarkyContext>
	{
		protected override void Seed(BookmarkyContext context)
		{
            var tag1 = new Tag() { Id = 1, TagName = "Javascript" };
            var tag2 = new Tag() { Id = 2, TagName = "C#" };
            var tag3 = new Tag() { Id = 3, TagName = "Design Patterns" };
            var tag4 = new Tag() { Id = 4, TagName = "Database" };

		    context.Tags.AddRange(new List<Tag> {tag1, tag2, tag3, tag4});
		    context.SaveChanges();

			var b1 = new Bookmark { Title = "Bookmark 1", Gist = "The maiden voyage", Url = "www.buttlash.com", ResourceType = ResourceType.Article };
			var b2 = new Bookmark { Title = "Bookmark 2", Gist = "Something Else", Url = "www.youtube.com", ResourceType = ResourceType.Video };
		    var b3 = new StickiedBookmark()
		    {
		        Title = "MDN",
		        Gist = "Mozilla Reference of EVERYTHING",
		        Url = "https://developer.mozilla.org/en-US/",
		        ResourceType = ResourceType.Reference,
		        LinkIconUrl = "https://developer.cdn.mozilla.net/media/img/favicon144.png"
		    };
            var bookmarks = new List<Bookmark>
			{
				b1,b2,b3
				
			};

			context.Bookmarks.AddRange(bookmarks);
			context.SaveChanges();

			var ratings = new List<Rating>
			{
				new Rating { Overview="It was basass", Score = 5, Bookmark = b1},
				new Rating { Overview="It was ok", Score = 3, Bookmark = b2},
			};

			context.Ratings.AddRange(ratings);
		    context.SaveChanges();

			var sources = new List<Source>
			{
				new Source { SourceName = "Reddit", Url = "www.reddit.com", Bookmarks = bookmarks }
			};

			context.Sources.AddRange(sources);

		    context.SaveChanges();
		}
	}
}
