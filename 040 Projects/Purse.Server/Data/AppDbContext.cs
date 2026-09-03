using Microsoft.EntityFrameworkCore;
using Purse.Server.Model;
// using Purse.Shared.Model;

namespace Purse.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionLineItem> LineItems => Set<TransactionLineItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SQL Server requires the Version property to be mapped as a concurrency token
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Version)
                .IsRowVersion();

            modelBuilder.Entity<TransactionLineItem>()
                .Property(t => t.Version)
                .IsRowVersion();
        }
    }
}