using Fleama.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Fleama.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<UserFavoriteProduct> UserFavoriteProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically apply all entity configurations defined in the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }

    }
}