using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fleama.Data
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(
                "Server=KADIR\\SQLEXPRESS;Database=FleamaDb;Trusted_Connection=True;TrustServerCertificate=True"
            );

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}