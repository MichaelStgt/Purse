// <copyright file="LocalDbContext.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Services.Data
{
    using CommunityToolkit.Datasync.Client.Http;
    using CommunityToolkit.Datasync.Client.Offline;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using SQLite;

    // Todo: Rename this to AppDBContext to be more consistent with typical EF naming conventions.

    /// <summary>
    /// Defines the <see cref="LocalDbContext" />. Look at this to get a idea of how this should work with Migrations:
    /// https://github.com/taublast/MauiEF or https://github.com/MichaelStgt/MauiEF There is also a explaining blog post
    /// about this on https://medium.com/@taublast/entity-framework-with-code-first-migrations-in-net-maui-3efbdb765592
    /// </summary>
    public class LocalDbContext : OfflineDbContext // DbContext 
    {
        #region Stuff from the EFMaui Sample

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDbContext"/> class.
        /// </summary>
        //public LocalDbContext()
        //{
        //    // Todo: Replace ths File Property and use the same DataSource= Property as in the DBContextInitializer, to avoid having multiple places where the database file path is defined, which can lead to confusion and maintenance issues. We should have a single point of definition for the database file path, and that should be in the DBContextInitializer, where we can also handle any necessary setup for the database file path and other configurations. Once we have a working implementation of the database initialization, we can remove this property and any references to it from the app.
        //    File = Path.Combine("../", "UsedByMigratorOnly1.db3");
        //    this.Initialize();

        //}

        /// <summary>
        /// The Initialize.
        /// </summary>
        //void Initialize()
        //{
        //    if (!Initialized)
        //    {
        //        SQLitePCL.Batteries_V2.Init();

        //        this.Database.Migrate();

        //        Initialized = true;
        //    }
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseSqlite($"Filename={File}");
        //}

        public static string? File
        {
            get; protected set;
        }
        public static bool Initialized
        {
            get; protected set;
        }

        #endregion
        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDbContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{LocalDbContext}"/>.</param>
        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
            // Task.Run(async () => await this.Database.EnsureCreatedAsync());
            // Task.Run(async () => await this.Database.MigrateAsync());
            // this.Database.EnsureCreated();
            // this.Database.Migrate();
            // this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the TransactionLineItems.
        /// </summary>
        public DbSet<TransactionLineItem> TransactionLineItems => this.Set<TransactionLineItem>();

        /// <summary>
        /// Gets the Transactions.
        /// </summary>
        public DbSet<Transaction> Transactions => this.Set<Transaction>();

        #endregion

        #region Methods

        /// <summary>
        /// The AddItem.
        /// </summary>
        /// <typeparam name="TEntity">.</typeparam>
        /// <param name="o">The o<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> AddItem<TEntity>(TEntity o)
            where TEntity : class
        {
            // var x = this.Database.GetDbConnection();
            EntityEntry y = await this.AddAsync<TEntity>(o);
            return y != null;
        }

        // Todo: Check, if we need this methods as the parent class already has FindAsync<TEntity> method.

        /// <summary>
        /// The FindItem.
        /// </summary>
        /// <typeparam name="TEntity">.</typeparam>
        /// <param name="keyValues">The keyValues<see cref="object?[]?"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> FindItem<TEntity>(params object?[]? keyValues)
            where TEntity : class
        {
            // var x = this.Database.GetDbConnection();
            var y = await this.FindAsync(typeof(TEntity), keyValues);
            var x = await this.FindAsync<TEntity>(keyValues);
            return y != null;
        }

        /// <summary>
        /// The OnDatasyncInitialization.
        /// </summary>
        /// <param name="optionsBuilder">The optionsBuilder<see cref="DatasyncOfflineOptionsBuilder"/>.</param>
        protected override void OnDatasyncInitialization(DatasyncOfflineOptionsBuilder optionsBuilder)
        {
            // 1. Define your Datasync connection (This replaces the old DatasyncClient)
            var syncOptions = new HttpClientOptions
            {
                // Replace with your actual Server URL. 
                // Tip: Use https://10.0.2.2:PORT if testing on an Android Emulator against a local API
                // HttpEndpoint = new Uri("https://YOUR-SERVER-URL.com")
                Endpoint = new Uri("https://YOUR-SERVER-URL.com")
            };

            // 2. Pass the options to the builder
            // optionsBuilder.UseClientOptions(syncOptions);
            optionsBuilder.UseHttpClientOptions(syncOptions);
        }



        #endregion
    }
}
