using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Purse.Data;

namespace Purse.Data.Migrator
{
    public class LocalDbContextFactory : IDesignTimeDbContextFactory<LocalDbContext>
    {
        public LocalDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LocalDbContext>();

            // The connection string doesn't matter here; it just tells EF Core 
            // to generate SQLite-compatible SQL.
            builder.UseSqlite("Data Source=migration_dummy.db");

            return new LocalDbContext(builder.Options);
        }
    }
}