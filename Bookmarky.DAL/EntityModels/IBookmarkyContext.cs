using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DAL.EntityModels
{
	public interface IBookmarkyContext : IDisposable
	{
		DbSet<Bookmark> Bookmarks { get; set; }
		DbSet<Source> Sources { get; set; }
		DbSet<Rating> Ratings { get; set; }

		int SaveChanges();
	}
}
