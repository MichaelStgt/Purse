using CommunityToolkit.Datasync.Client.Http;
using CommunityToolkit.Datasync.Client.Offline;
using Microsoft.EntityFrameworkCore;
using MVVMBaseTen.Model;
using Purse.Shared.Model;
using System.Linq.Expressions;
using static SQLite.SQLite3;

namespace Purse.Data
{
    public partial class LocalDbContext : OfflineDbContext
    {
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionLineItem> TransactionLineItems => Set<TransactionLineItem>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Vendor> Vendors => Set<Vendor>();
        public DbSet<IncomeSource> IncomeSources => Set<IncomeSource>();
        public DbSet<Country> Countries => Set<Country>();

        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
        }

        protected override void OnDatasyncInitialization(DatasyncOfflineOptionsBuilder optionsBuilder)
        {
            var syncOptions = new HttpClientOptions
            {
                Endpoint = new Uri("https://YOUR-SERVER-URL.com")
            };
            optionsBuilder.UseHttpClientOptions(syncOptions);
        }
    }
}