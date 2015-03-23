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
				new Rating { Overview="It was basass", Score = 5, BookmarkId = b1.Id},
				new Rating { Overview="It was ok", Score = 3, BookmarkId = b2.Id},
			};

			context.Ratings.AddRange(ratings);

			var sources = new List<Source>
			{
				new Source { SourceName = "Reddit", Url = "www.reddit.com", Bookmarks = bookmarks }
			};

			context.Sources.AddRange(sources);
		}
	}
}
