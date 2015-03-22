using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarky.DAL.EntityModels
{
    public interface IBookmarkyContext : IDisposable
    {
        //DbSet<Bookmark> Bookmarks { get; set; }
        //DbSet<Source> Sources { get; set; }
        //DbSet<Rating> Ratings { get; set; }

        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet Set(Type entityType);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);

        int SaveChanges();
    }
}
