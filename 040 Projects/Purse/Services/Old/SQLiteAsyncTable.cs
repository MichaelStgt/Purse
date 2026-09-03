// <copyright file="SQLiteAsyncTable.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

#nullable enable

namespace HourTracker.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    // using System.Threading.Tasks;
    using MVVMBase.Abstractions;

    /// <summary>
    /// Defines the <see cref="SQLiteAsyncTable{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class SQLiteAsyncTable<T> : ICloudTable<T>
        where T : TableData, new()
    {
        #region Fields

        /// <summary>
        /// Defines the database.
        /// </summary>
        private readonly SQLite.SQLiteAsyncConnection database;

        ///// <summary>
        ///// Defines the currentVersion.
        ///// </summary>
        //private int currentVersion = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteAsyncTable{T}"/> class.
        /// </summary>
        /// <param name="connection">The connection<see cref="SQLite.SQLiteAsyncConnection"/>.</param>
        public SQLiteAsyncTable(SQLite.SQLiteAsyncConnection connection)
        {
            this.database = connection;
        }

        #endregion

        #region Events

        /// <summary>
        /// Defines the DataChanged.
        /// </summary>
        public event DataChangingEventHandler<T>? DataChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Count the items asynchronously.
        /// </summary>
        /// <param name="syncItems">If true, sync items.</param>
        /// <returns><![CDATA[Task<int>]]>.</returns>
        public async Task<int> CountItemsAsync(bool syncItems = false)
        {
            try
            {
                int count = await this.database.Table<T>().CountAsync();
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Count the items asynchronously.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="syncItems">If true, sync items.</param>
        /// <returns><![CDATA[Task<int>]]>.</returns>
        public async Task<int> CountItemsAsync(Expression<Func<T, bool>> expression, bool syncItems = false)
        {
            try
            {
                int count = await this.database.Table<T>().CountAsync(expression);
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The CreateItemAsync.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> CreateItemAsync(T item)
        {
            if (!string.IsNullOrEmpty(item?.Id))
            {
                try
                {
                    item.CreatedAt = DateTimeOffset.Now;
                    // item.Version = SQLiteAsyncTable<T>.ToVersionString(this.currentVersion++);
                    item.IsDeleted = false;
                    await this.database.InsertAsync(item);
                    await this.DataChangedInvoke(item, DataOperation.CreateItem).ConfigureAwait(false);
                    return item;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in {nameof(this.CreateItemAsync)}: {ex.Message}", ex.InnerException);
                }
            }
            else
            {
                throw new ArgumentException($"Item.Id of item {item?.ToString()} can not be empty!");
            }
        }

        /// <summary>
        /// The DataChangedInvoke.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="dataOperation">The dataOperation<see cref="DataOperation"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task DataChangedInvoke(T obj, DataOperation dataOperation)
        {
            this.DataChanged?.Invoke(obj, new DataObjectEventArgs { DataOperation = dataOperation });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes all items asynchronously.
        /// </summary>
        /// <returns><![CDATA[Task<bool>]]>.</returns>
        public async Task<bool> DeleteAllItemsAsync()
        {
            try
            {
                IEnumerable<T> items = await this.database.Table<T>().ToListAsync().ConfigureAwait(false);

                // T currentItem = null;
                foreach (T item in items)
                {
                    await this.database.DeleteAsync(item);
                }

                // await this.DataChangedInvoke(currentItem, DataOperation.DeleteItem).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(this.DeleteAllItemsAsync)}: {ex.Message}", ex.InnerException);
            }
        }

        /// <summary>
        /// The DeleteItemAsync.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="softDelete">The softDelete<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> DeleteItemAsync(T item, bool softDelete)
        {
            if (!string.IsNullOrEmpty(item.Id))
            {
                try
                {
                    if (softDelete)
                    {
                        item.IsDeleted = true;
                        item.UpdatedAt = DateTimeOffset.Now;
                        // item.Version = SQLiteAsyncTable<T>.ToVersionString(this.currentVersion++);
                        await this.database.UpdateAsync(item);
                    }
                    else
                    {
                        await this.database.DeleteAsync(item);
                    }

                    await this.DataChangedInvoke(item, DataOperation.DeleteItem).ConfigureAwait(false);

                    return item;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in {nameof(this.DeleteItemAsync)}: {ex.Message}", ex.InnerException);
                }
            }
            else
            {
                throw new ArgumentException($"Item.Id of item {item?.ToString()} can not be empty!");
            }
        }

        /// <summary>
        /// The DeleteItemsAsync.
        /// </summary>
        /// <param name="expression">The expression<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="softDelete">The softDelete<see cref="bool"/>.</param>
        /// <param name="syncItems">The syncItems<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> DeleteItemsAsync(Expression<Func<T, bool>> expression, bool softDelete = false, bool syncItems = false)
        {
            try
            {
                IEnumerable<T> items = await this.database.Table<T>().Where(expression).ToListAsync().ConfigureAwait(false);
                T? currentItem = null;

                foreach (T item in items)
                {
                    if (softDelete)
                    {
                        item.IsDeleted = true;
                        item.UpdatedAt = DateTimeOffset.Now;
                        // item.Version = SQLiteAsyncTable<T>.ToVersionString(this.currentVersion++);
                        await this.database.UpdateAsync(item);
                    }
                    else
                    {
                        await this.database.DeleteAsync(item);
                    }

                    currentItem = item;
                }

#pragma warning disable CS8604 // Possible null reference argument.
                await this.DataChangedInvoke(currentItem, DataOperation.DeleteItem).ConfigureAwait(false);
#pragma warning restore CS8604 // Possible null reference argument.

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(this.DeleteItemsAsync)}: {ex.Message}", ex.InnerException);
            }
        }

        /// <summary>
        /// The Exists.
        /// </summary>
        /// <param name="expression">The expression<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
            try
            {
                T item = await this.database.Table<T>()
                   .FirstOrDefaultAsync(expression).ConfigureAwait(false);

                return item != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(this.Exists)}: {ex.Message}", ex.InnerException);
            }
        }

        /// <summary>
        /// The Exists.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> Exists(string id)
        {
            try
            {
                T item = await this.database.Table<T>()
                   .FirstAsync(w => w.Id == id).ConfigureAwait(false);

                return item != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(this.Exists)}: {ex.Message}", ex.InnerException);
            }
        }

        /// <summary>
        /// The InsertAllAsync.
        /// </summary>
        /// <param name="objects">The objects<see cref="IEnumerable"/>.</param>
        /// <param name="runInTransaction">The runInTransaction<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> InsertAllAsync(IEnumerable objects, bool runInTransaction = true)
        {
            try
            {
                await this.database.InsertAllAsync(objects, true);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The ReadItemAsync.
        /// </summary>
        /// <param name="expression">The predicate<see cref="Func{T, bool}"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> ReadItemAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                T item = await this.database.Table<T>()
                   .FirstOrDefaultAsync(expression);

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(this.ReadItemAsync)}: {ex.Message}", ex.InnerException);
            }
        }

        /// <summary>
        /// The ReadItemAsync.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> ReadItemAsync(string id)
        {
            try
            {
                T item = await this.database.Table<T>()
                   .FirstOrDefaultAsync(w => w.Id == id);

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(this.ReadItemAsync)}: {ex.Message}", ex.InnerException);
            }
        }

        /// <summary>
        /// The ReadItemsAsync.
        /// </summary>
        /// <param name="syncItems">The syncItems<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        public async Task<IEnumerable<T>> ReadItemsAsync(bool syncItems = false)
        {
            try
            {
                // SQL queries are also possible
                // IEnumerable<T> items2 = await this.database.QueryAsync<T>($"SELECT * FROM [{typeof(T).Text}]"); // WHERE [Done] = 0");
                IEnumerable<T> items = await this.database.Table<T>().ToListAsync().ConfigureAwait(false);
                return items;
            }
            catch (SQLite.SQLiteException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The ReadItemsAsync.
        /// </summary>
        /// <param name="expression">The expression<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="skip">The skip<see cref="int"/>.</param>
        /// <param name="take">The take<see cref="int"/>.</param>
        /// <param name="syncItems">The syncItems<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        public async Task<IEnumerable<T>> ReadItemsAsync(Expression<Func<T, bool>> expression, int skip = 0, int take = 0, bool syncItems = false)
        {
            try
            {
                IEnumerable<T> items;

                if (skip == 0 && take == 0)
                {
                    items = await this.database.Table<T>().Where(expression).ToListAsync().ConfigureAwait(false);
                }
                else
                {
                    items = await this.database.Table<T>().Where(expression).Skip(skip).Take(take).ToListAsync().ConfigureAwait(false);
                }

                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The ReadItemsDefferedAsync.
        /// </summary>
        /// <param name="syncItems">The syncItems<see cref="bool"/>.</param>
        /// <returns>The <see cref="IAsyncEnumerable{T}"/>.</returns>
        public async IAsyncEnumerable<T> ReadItemsDeferredAsync(bool syncItems = false)
        {
            IEnumerable<T> items = await this.database.Table<T>().ToListAsync().ConfigureAwait(false);

            foreach (T item in items)
            {
                yield return item;
            }
        }

        /// <summary>
        /// The ReadItemsDefferedAsync.
        /// </summary>
        /// <param name="expression">The expression<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="syncItems">The syncItems<see cref="bool"/>.</param>
        /// <returns>The <see cref="IAsyncEnumerable{T}"/>.</returns>
        public async IAsyncEnumerable<T> ReadItemsDeferredAsync(Expression<Func<T, bool>> expression, bool syncItems = false)
        {
            IEnumerable<T> items = await this.database.Table<T>().Where(expression).ToListAsync().ConfigureAwait(false);

            foreach (T item in items)
            {
                yield return item;
            }
        }

        /// <summary>
        /// The UpdateItemAsync.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> UpdateItemAsync(T item)
        {
            if (!string.IsNullOrEmpty(item?.Id))
            {
                try
                {
                    item.UpdatedAt = DateTimeOffset.Now;
                    // item.Version = SQLiteAsyncTable<T>.ToVersionString(this.currentVersion++);
                    await this.database.UpdateAsync(item);
                    await this.DataChangedInvoke(item, DataOperation.UpdateItem).ConfigureAwait(false);
                    return item;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in {nameof(this.UpdateItemAsync)}: {ex.Message}", ex.InnerException);
                }
            }
            else
            {
                throw new ArgumentException($"Item.Id of item {item?.ToString()} can not be empty!");
            }
        }

        /// <summary>
        /// The ToVersionString.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private static byte[] ToVersionString(int i)
        {
            byte[] b = BitConverter.GetBytes(i);
            string str = Convert.ToBase64String(b);
            return Encoding.ASCII.GetBytes(str);
        }

        #endregion
    }
}
