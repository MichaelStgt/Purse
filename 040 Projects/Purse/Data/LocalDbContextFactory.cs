using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Purse.Data
{
    // This class is ONLY used by Visual Studio when you run Add-Migration.
    // It is completely ignored when the app actually runs on a device.
    public class LocalDbContextFactory : IDesignTimeDbContextFactory<LocalDbContext>
    {
        public LocalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocalDbContext>();

            // We provide a dummy connection string just so the tooling knows 
            // we are generating SQL syntax for SQLite.
            optionsBuilder.UseSqlite("Data Source=design_time_migration.db");

            return new LocalDbContext(optionsBuilder.Options);
        }
    }
}
