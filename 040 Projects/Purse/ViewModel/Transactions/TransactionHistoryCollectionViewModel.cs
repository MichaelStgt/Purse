// <copyright file="TransactionHistoryCollectionViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using CommunityToolkit.Mvvm.Messaging;
    using MVVMBaseTen.Messaging;

    /// <summary>
    /// The transaction history collection view model.
    /// </summary>
    public partial class TransactionHistoryCollectionViewModel : ObservableCollectionViewModel<Transaction>
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
            this.dbContext = dbContext;

            // Register messenger to dynamically update collections on changes (Created, Updated, Deleted)
            WeakReferenceMessenger.Default.Register<EntityChangedMessage<Transaction>>(this, (r, m) =>
            {
                this.HandleTransactionChanged(m.Value, m.Action);
            });

            this.Title = AppResources.TransactionHistory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load the items asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><![CDATA[Task<IReadOnlyList<Transaction>>]]>.</returns>
        protected override async Task<IReadOnlyList<Transaction>> LoadItemsAsync(CancellationToken cancellationToken)
        {
            var items = await this.dbContext.ReadItemsAsync<Transaction>(
                query => query.OrderByDescending(t => t.Date),
                cancellationToken);

            var categories = await this.dbContext.Categories.ToListAsync(cancellationToken);
            var lineItems = await this.dbContext.TransactionLineItems.ToListAsync(cancellationToken);

            foreach (var t in items)
            {
                var tSplits = lineItems.Where(l => l.TransactionId == t.Id).ToList();
                if (tSplits.Count > 0)
                {
                    decimal net = 0m;
                    foreach (var s in tSplits)
                    {
                        var cat = categories.FirstOrDefault(c =>
                            string.Equals(c.DisplayName, s.Category, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(c.Name, s.Category, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(c.Id, s.Category, StringComparison.OrdinalIgnoreCase));

                        bool isIncome = cat?.IsIncome ?? false;
                        decimal amt = (decimal)s.Amount;
                        if (isIncome)
                        {
                            net += Math.Abs(amt);
                        }
                        else
                        {
                            net -= Math.Abs(amt);
                        }
                    }
                    t.TotalAmount = (double)net;
                }
                else
                {
                    var cat = categories.FirstOrDefault(c =>
                        string.Equals(c.DisplayName, t.Tags, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(c.Name, t.Tags, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(c.Id, t.Tags, StringComparison.OrdinalIgnoreCase));

                    if (cat != null && !cat.IsIncome)
                    {
                        t.TotalAmount = -Math.Abs(t.TotalAmount);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// On selected item changed implementation asynchronously.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A Task.</returns>
        protected override async Task OnSelectedItemChangedImplementationAsync(Transaction? value)
        {
            if (value is null || Shell.Current is null)
            {
                return;
            }

            try
            {
                await Shell.Current.GoToAsync(
                    nameof(TransactionDetailView),
                    new ShellNavigationQueryParameters
                    {
                        { this.QueryIdParameterName, value.Id },
                    });
            }
            finally
            {
                this.SelectedItem = null;
            }
        }

        /// <summary>
        /// Add the transaction asynchronously.
        /// </summary>
        /// <returns>A Task.</returns>
        [RelayCommand]
        private async Task AddItemAsync()
        {
            await Shell.Current.GoToAsync(nameof(TransactionDetailView));
        }

        /// <summary>
        /// Handles transaction save/delete messages dynamically to update the collection.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="action">The action.</param>
        private void HandleTransactionChanged(Transaction transaction, EntityChangeAction action)
        {
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                if (action == EntityChangeAction.Created)
                {
                    // Find correct descending index by Date to keep list sorted
                    int insertIndex = 0;
                    while (insertIndex < this.ItemCollection.Count && this.ItemCollection[insertIndex].Date > transaction.Date)
                    {
                        insertIndex++;
                    }
                    this.ItemCollection.Insert(insertIndex, transaction);
                }
                else if (action == EntityChangeAction.Updated)
                {
                    var existing = this.ItemCollection.FirstOrDefault(t => t.Id == transaction.Id);
                    if (existing is not null)
                    {
                        int index = this.ItemCollection.IndexOf(existing);

                        // Replace reference to trigger property notifications and redraw item
                        this.ItemCollection[index] = transaction;

                        // If the date changed, move to the correct sorted position
                        if (existing.Date != transaction.Date)
                        {
                            this.ItemCollection.RemoveAt(index);
                            int newIndex = 0;
                            while (newIndex < this.ItemCollection.Count && this.ItemCollection[newIndex].Date > transaction.Date)
                            {
                                newIndex++;
                            }
                            this.ItemCollection.Insert(newIndex, transaction);
                        }
                    }
                }
                else if (action == EntityChangeAction.Deleted)
                {
                    var existing = this.ItemCollection.FirstOrDefault(t => t.Id == transaction.Id);
                    if (existing is not null)
                    {
                        this.ItemCollection.Remove(existing);
                    }
                }
            });
        }

        #endregion
    }
}