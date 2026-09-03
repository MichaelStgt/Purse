// <copyright file="SQLiteAsyncService.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace HourTracker.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    // using System.Threading.Tasks;
    using MVVMBase.Abstractions;
    using SQLite;

    /// <summary>
    /// Defines the <see cref="SQLiteAsyncService" />.
    /// </summary>
    public class SQLiteAsyncService : ICloudService
    {
        /// <summary>
        /// Defines the current.
        /// </summary>
        private static ICloudService? current;

        #region Constants

        /// <summary>
        /// Defines the Flags.
        /// </summary>
        private const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;

        #endregion Constants

        #region Fields

        /// <summary>
        /// Defines the LazyDefaultPreferencesInitializer.
        /// </summary>
        private static readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(()
                                                         => new SQLiteAsyncConnection(SQLiteDBPath, Flags));

        /// <summary>
        /// Gets the connection.
        /// </summary>
        private SQLiteAsyncConnection Connection => LazyInitializer.Value;

        /// <summary>
        /// Defines the tables.
        /// This is just to make the MockCloudService and the AzureCloudservice working the same.......
        /// </summary>
        private readonly Dictionary<string, object> tables = new Dictionary<string, object>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteAsyncService"/> class.
        /// </summary>
        public SQLiteAsyncService()
        {
            this.InitializeAsync();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Defines the DataChanged.
        /// </summary>
        public event DataChangingEventHandler? DataChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>An ICloudService?</value>
        public static ICloudService? Current
        {
            get
            {
                if (current == null)
                {
                    current = new SQLiteAsyncService();
                }

                return current;
            }
        }

        /// <summary>
        /// Gets the SQLiteDBPath
        /// Gets a value containing the path and filename to the local SQLite database.
        /// </summary>
        public static string SQLiteDBPath
        {
            get
            {
                try
                {
                    //string s = string.Empty;

                    //if (FileSystem.Current?.AppDataDirectory != null)
                    //{
                    //    /*string*/
                    //    s = Path.Combine(FileSystem.Current?.AppDataDirectory, "SQLiteDB.db3");
                    //}

                    string s = Path.Combine(FileSystem.AppDataDirectory, "SQLiteDB.db3");

                    return s;
                }
                catch (Exception ex)
                {
                    if (Debugger.IsAttached)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                return string.Empty;
            }
        }
        private async Task<string> GetDBPath()
        {
            try
            {
                string s = Path.Combine(FileSystem.AppDataDirectory, "SQLiteDB.db3");

                return s;
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the Identifier.
        /// </summary>
        public Guid Identifier { get; } = Guid.NewGuid();

        #endregion Properties

        #region Methods

        /// <summary>
        /// The InitializeAsync.
        /// </summary>
        public void InitializeAsync()
        {
            try
            {
                IEnumerable<Type> models = this.GetTypes<TableData>();

                foreach (Type t in models)
                {
                    this.DefineTable(t).ConfigureAwait(false);
                }

                // this.DefineTable<ItemModel>().ConfigureAwait(false);
                this.InitializeBaseDataAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = $"Error in Class {this.GetType().Name}, Method {nameof(this.InitializeAsync)}" + Environment.NewLine;
                error += $"Stacktrace: {ex.StackTrace}" + Environment.NewLine;
                error += $"Original Message: {ex.Message}" + Environment.NewLine;
                error += $"Inner Exception: {ex.InnerException}" + Environment.NewLine;

                if (Debugger.IsAttached)
                {
                    Debug.WriteLine(error);
                }

                throw;
            }
        }

        /// <summary>
        /// The CreateTable.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        public void CreateTable<T>()
            where T : TableData, new()
        {
            var tableName = typeof(T).Name;

            if (!this.tables.ContainsKey(tableName))
            {
                ICloudTable<T> table = new SQLiteAsyncTable<T>(this.Connection);

                this.tables[tableName] = table;

                table.DataChanged += this.Table_DataChanged;
            }
        }

        /// <summary>
        /// The GetTable.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="ICloudTable{T}"/>.</returns>
        public ICloudTable<T> GetTable<T>()
           where T : TableData, new()
        {
            var tableName = typeof(T).Name;

            if (!this.tables.ContainsKey(tableName))
            {
                ICloudTable<T> sqLiteTable = new SQLiteAsyncTable<T>(this.Connection);
                sqLiteTable.DataChanged += this.Table_DataChanged;
                this.tables[tableName] = sqLiteTable;
            }
            return (ICloudTable<T>)this.tables[tableName];
        }

        /// <summary>
        /// Initializes the base data async.
        /// </summary>
        /// <returns>A Task.</returns>
        public Task InitializeBaseDataAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The SeedTable.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<T> SeedTable<T>(T item)
            where T : TableData, new()
        {
            var tableName = typeof(T).Name;

            if (this.tables.ContainsKey(tableName))
            {
                ICloudTable<T> table = this.GetTable<T>();

                return await table.CreateItemAsync(item).ConfigureAwait(false);
            }

            return item;
        }

        ///// <summary>
        ///// The DefineTable.
        ///// </summary>
        ///// <typeparam name="T">.</typeparam>
        ///// <returns>The <see cref="Task"/>.</returns>
        //private async Task DefineTable<T>()
        //{
        //    string tableName = typeof(T).SelectedColor;

        //    try
        //    {
        //        if (!this.Connection.TableMappings.Any(m => m.MappedType.SelectedColor == tableName))
        //        {
        //            await this.Connection.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// The DefineTable.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task DefineTable(Type type)
        {
            string tableName = type.Name;

            try
            {
                if (!this.Connection.TableMappings.Any(m => m.MappedType.Name == tableName))
                {
                    try
                    {
                        await this.Connection.CreateTablesAsync(CreateFlags.None, type).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>An object</returns>
        private object GetTable(string tableName)

        // where T : TableData, new()
        {
            Type d1 = typeof(Dictionary<,>);
            Type x = typeof(SQLiteAsyncTable<>);

            // if (!this.tables.ContainsKey(tableName))
            // {
            //    object sqLiteTable = new SQLiteAsyncTable<T>(this.Connection);
            //    // sqLiteTable.DataChanged += this.Table_DataChanged;
            //    this.tables[tableName] = sqLiteTable;
            // }
            return /*(ICloudTable<T>)*/this.tables[tableName];
        }

        /// <summary>
        /// Get the tables.
        /// </summary>
        /// <param name="newType">The new type.</param>
        /// <returns><![CDATA[IEnumerable<object?>]]></returns>
        [Obsolete("This just doesn't work as expected")]
        private IEnumerable<object?> GetTables(Type[] newType)

        // where T : TableData, new()
        {
            // List<object> list = new List<object>();

            // foreach (var x in this.tables)
            // {
            //    list.Add(this.GetTable(x.GroupName));
            //    // yield return this.GetTable(x.GroupName);
            // }
            for (int i = 0; i < newType.Length; i++)
            {
                string tableName = newType[i].Name;

                Type d1 = typeof(SQLiteAsyncTable<>);

                Type[] typeArgs = { newType[i], };

                Type constructed = d1.MakeGenericType(typeArgs);

                // d1.MakeGenericType(typeArgs);
                object? o = Activator.CreateInstance(constructed, this.Connection);

                yield return o;

                // System.Reflection.MethodInfo getTable = this.CloudService.GetType().GetMethod("GetTable");

                // // Step 3: Construct the method generic with desired type of arguments
                // System.Reflection.MethodInfo getTableTyped = getTable.MakeGenericMethod(newType[i]);
                //// /*ICloudTable<TableData>*/
                //// object result; //  = new object();
                //// object result = getTableTyped.Invoke(null, []);
            }

            // return true;
        }

        /// <summary>
        /// The Table_DataChanged.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e<see cref="DataObjectEventArgs"/>.</param>
        private void Table_DataChanged<T>(T sender, DataObjectEventArgs e)
            where T : TableData
        {
            this.DataChanged?.Invoke(sender, e);
        }

        #endregion Methods
    }
}