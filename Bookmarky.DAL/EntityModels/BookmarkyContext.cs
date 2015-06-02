using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DAL.EntityModels
{
	public class BookmarkyContext : DbContext, IBookmarkyContext
	{
		public BookmarkyContext() :
			base("BookmarkyContext")
		{
			Configuration.LazyLoadingEnabled = false;
		}

	    public BookmarkyContext(string connectionString)
	        : base(connectionString)
	    {
	        Configuration.LazyLoadingEnabled = false;
	    }

		public DbSet<Bookmark> Bookmarks { get; set; }
		public DbSet<Source> Sources { get; set; }
		public DbSet<Rating> Ratings { get; set; }
        public DbSet<StickiedBookmark> StickedBookmarks { get; set; }
        public DbSet<Tag> Tags { get; set;}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<Source>()
				.HasKey(s => s.Id)
				.Property(s => s.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
				.HasColumnName("SourceId");

			modelBuilder.Entity<Rating>()
				.HasKey(r => r.Id)
				.Property(r => r.Id)
                //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
				.HasColumnName("RatingId");

			modelBuilder.Entity<Bookmark>()
				.HasKey(b => b.Id)
				.Property(b => b.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
				.HasColumnName("BookmarkId");

		    modelBuilder.Entity<Bookmark>()
		        .HasOptional(b => b.Rating)
		        .WithRequired(r => r.Bookmark)
		        .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Rating>()
            //    .HasRequired(b => b.Bookmark)
            //    .WithOptional(c => c.Rating);

            //modelBuilder.Entity<Rating>()
            //    .HasRequired(r => r.Bookmark)
            //    .WithRequiredPrincipal();

		    modelBuilder.Entity<Bookmark>()
		        .HasMany(b => b.Tags)
		        .WithMany(b => b.Bookmarks)
		        .Map(m =>
		        {
		            m.MapRightKey("TagId");
		            m.MapLeftKey("BookmarkId");
		            m.ToTable("TagBookmarkMapping");
		        });

		}
	}
}
