namespace Purse.Data
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;

    public partial class LocalDbContext
    {
        public IQueryable<T> Query<T>()
            where T : class
        {
            return this.Set<T>();
        }

        public IQueryable<T> QueryNoTracking<T>()
            where T : class
        {
            return this.Set<T>().AsNoTracking();
        }

        public Task<List<T>> ReadItemsAsync<T>(CancellationToken cancellationToken = default)
            where T : class
        {
            return this.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public Task<List<T>> ReadItemsAsync<T>(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class
        {
            return this.Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public Task<List<T>> ReadItemsAsync<T>(
            Func<IQueryable<T>, IQueryable<T>> queryBuilder,
            CancellationToken cancellationToken = default)
            where T : class
        {
            IQueryable<T> query = queryBuilder(this.Set<T>().AsNoTracking());
            return query.ToListAsync(cancellationToken);
        }

        public Task<T?> ReadItemAsync<T>(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class
        {
            return this.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public ValueTask<T?> FindItemAsync<T>(params object[] keyValues)
            where T : class
        {
            return this.Set<T>().FindAsync(keyValues);
        }

        public Task<bool> AnyAsync<T>(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class
        {
            return this.Set<T>().AnyAsync(predicate, cancellationToken);
        }

        public Task<int> CountAsync<T>(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where T : class
        {
            IQueryable<T> query = this.Set<T>().AsNoTracking();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return query.CountAsync(cancellationToken);
        }

        public async Task<int> InsertAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            await this.Set<T>().AddAsync(entity, cancellationToken);
            return await this.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> InsertRangeAsync<T>(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
            where T : class
        {
            await this.Set<T>().AddRangeAsync(entities, cancellationToken);
            return await this.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            this.Set<T>().Update(entity);
            return await this.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> UpdateRangeAsync<T>(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
            where T : class
        {
            this.Set<T>().UpdateRange(entities);
            return await this.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> DeleteAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            this.Set<T>().Remove(entity);
            return await this.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> DeleteRangeAsync<T>(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
            where T : class
        {
            this.Set<T>().RemoveRange(entities);
            return await this.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> DeleteAsync<T>(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class
        {
            List<T> entities = await this.Set<T>()
                .Where(predicate)
                .ToListAsync(cancellationToken);

            this.Set<T>().RemoveRange(entities);
            return await this.SaveChangesAsync(cancellationToken);
        }
    }
}
