using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Purse.Data.Services
{
    /// <summary>
    /// Design-time DbContext factory for Entity Framework Core migrations.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LocalDbContext>
    {
        /// <summary>
        /// Creates a new instance of the database context at design-time.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>A new <see cref="LocalDbContext"/> instance.</returns>
        public LocalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocalDbContext>();
            optionsBuilder.UseSqlite("Data Source=designTime.db");

            return new LocalDbContext(optionsBuilder.Options);
        }
    }
}
