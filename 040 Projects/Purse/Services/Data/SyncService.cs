// <copyright file="SyncService.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Services.Data
{
    using CommunityToolkit.Datasync.Client.Offline;

    /// <summary>
    /// Defines the <see cref="SyncService" />.
    /// </summary>
    public class SyncService
    {
        #region Fields

        /// <summary>
        /// Defines the appDBContext.
        /// </summary>
        private readonly LocalDbContext appDBContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncService"/> class.
        /// </summary>
        /// <param name="dbContext">The dbContext<see cref="LocalDbContext"/>.</param>
        public SyncService(LocalDbContext dbContext)
        {
            this.appDBContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The SynchronizeAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SynchronizeAsync()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }

            try
            {
                // Todo: In the modern toolkit, we can push and pull directly from the DbContext, which will handle the Operations Queue for us.
                // clear, if this will work whenever we have multiple tables with relationships, or if we need to push/pull each table separately. In the legacy toolkit, we had to push/pull each table separately, but in the modern toolkit, the DbContext handles this for us.
                // also, if there is any build in functionality to handle the User currently Logged into the Application, to avoid having 
                // other users to update data that don't belong to them, or if we need to handle this manually by adding a UserId field to each table and filtering the data based on the logged in user. In the legacy toolkit, we had to handle this manually, but in the modern toolkit, there might be some built in functionality to handle this for us.
                // In the modern toolkit, the DbContext handles the Operations Queue directly.
                // Push local offline changes up to the server
                await this.appDBContext.PushAsync();
                // await this.appDBContext.TransactionLineItems.PushAsync();

                // Pull remote changes down to the local offline database
                await this.appDBContext.PullAsync();
            }
            catch (Exception ex)
            {
                // Handle Sync Conflicts here (e.g., if a record was changed on the server 
                // and locally at the same time)
                System.Diagnostics.Debug.WriteLine($"Sync Failed: {ex.Message}");
            }
        }

        #endregion
    }
}
