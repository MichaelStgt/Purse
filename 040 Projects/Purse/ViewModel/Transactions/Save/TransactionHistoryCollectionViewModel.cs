// <copyright file="TransactionHistoryCollectionViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using Purse.Shared.Model;

    /// <summary>
    /// The transaction history collection view model.
    /// </summary>
    public partial class TransactionHistoryCollectionViewModel
        : ObservableCollectionViewModel<Transaction>
    {
        #region Fields

        /// <summary>
        /// The db context.
        /// </summary>
        private readonly LocalDbContext dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHistoryCollectionViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        public TransactionHistoryCollectionViewModel(LocalDbContext dbContext)
        {
            this.Title = AppResources.TransactionHistory;
            this.dbContext = dbContext;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether refreshing.
        /// </summary>
        /// <value>A bool</value>
        [ObservableProperty]
        public partial bool IsRefreshing
        {
            get; set;
        }

        ///// <summary>
        ///// Gets or sets the selected transaction.
        ///// </summary>
        ///// <value>A Transaction?</value>
        //[ObservableProperty]
        //public partial Transaction? SelectedTransaction
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// On selected transaction changed. This is automatically called when the SelectedTransaction property changes,
        ///// due to the [ObservableProperty] attribute.
        ///// </summary>
        ///// <param name="value">The value.</param>
        //partial void OnSelectedTransactionChanged(Transaction? value)
        //{
        //    if (value != null)
        //    {
        //        Shell.Current.GoToAsync($"{nameof(TransactionDetailView)}?TransactionId={value.Id}");

        //        // Clear the selection immediately so the user can tap it again later
        //        SelectedTransaction = null;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the transactions.
        ///// </summary>
        ///// <value>A observablecollection of transactions.</value>
        //[ObservableProperty]
        //public partial ObservableCollection<Transaction> Transactions { get; set; } = new();

        /// <summary>
        /// Load item collection command implementation.
        /// </summary>
        /// <returns>A Task</returns>
        public override async Task LoadItemCollectionCommandImplementation()
        {
            if (this.IsRefreshing)
            {
                return;
            }

            try
            {
                this.IsRefreshing = true;

                // Fetch all transactions from the local offline database, ordered by newest first
                List<Transaction> dbTransactions = await this.dbContext.Transactions
                    .OrderByDescending(t => t.Date)
                    .ToListAsync();

                this.ItemCollection.Clear();

                foreach (var transaction in dbTransactions)
                {
                    this.ItemCollection.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading transactions: {ex.Message}");
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// Load the transactions asynchronously.
        ///// </summary>
        ///// <returns>A Task</returns>
        //[RelayCommand]
        //public async Task LoadTransactionsAsync()
        //{
        //    if (this.IsRefreshing)
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        this.IsRefreshing = true;

        //        // Fetch all transactions from the local offline database, ordered by newest first
        //        var dbTransactions = await this.dbContext.Transactions
        //            .OrderByDescending(t => t.Date)
        //            .ToListAsync();

        //        this.Transactions.Clear();
        //        foreach (var transaction in dbTransactions)
        //        {
        //            this.Transactions.Add(transaction);
        //        }
        //        // this.OnPropertyChanged(nameof(this.Transactions));
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Error loading transactions: {ex.Message}");
        //    }
        //    finally
        //    {
        //        this.IsRefreshing = false;
        //    }
        //}

        /// <summary>
        /// Go converts to add transaction asynchronously.
        /// </summary>
        /// <returns>A Task</returns>
        [RelayCommand]
        private async Task AddTransactionAsync()
        {
            // Navigate to the Add page
            await Shell.Current.GoToAsync(nameof(TransactionDetailView));
        }

        public override void OnSelectedItemChangedImplementation(Transaction? value)
        {
            if (value != null)
            {
                Shell.Current.GoToAsync($"{nameof(TransactionDetailView)}?TransactionId={value.Id}");

                // Clear the selection immediately so the user can tap it again later
                this.SelectedItem = null;
            }
        }

        #endregion
    }
}
