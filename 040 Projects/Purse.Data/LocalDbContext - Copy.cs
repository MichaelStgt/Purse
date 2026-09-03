using CommunityToolkit.Datasync.Client.Http;
using CommunityToolkit.Datasync.Client.Offline;
using Microsoft.EntityFrameworkCore;
using MVVMBaseTen.Model;
using Purse.Shared.Model;
using System.Linq.Expressions;
using static SQLite.SQLite3;

namespace Purse.Data
{
    public class LocalDbContext : OfflineDbContext
    {
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionLineItem> TransactionLineItems => Set<TransactionLineItem>();

        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
        }

        public async Task<List<T>> ReadItemsAsync<T>(Expression<Func<T, bool>> expression)
            where T : class
        {
            return await this.Set<T>()
                .Where(expression)
                .ToListAsync();
        }

        //public Task<T?> ReadItemAsync<T>(Expression<Func<T, bool>> predicate)
        //    where T : class
        //{

        //}

        //public Task<List<T>> ReadItemsAsync<T>()
        //    where T : class
        //{

        //}

        //public Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate)
        //    where T : class
        //{

        //}

        //public Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate)
        //    where T : class
        //{

        //}

        //public async Task<IQueryable<TResult>> ReadItemsAsync<TResult>(Expression<Func<IQueryable<TResult>>> expression/*Expression<Func<T, bool>> expression*/)
        //    where TResult : OfflineClientEntity, new()
        //{
        //    try
        //    {
        //        // IEnumerable<T> items;

        //        // var items = await this.FromExpression<TResult>(expression).ToListAsync().ConfigureAwait(false);
        //        // var x = this.co
        //            // items = await this.Database./*Table<T>().*/Where(expression).ToListAsync().ConfigureAwait(false);

        //        return items;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

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