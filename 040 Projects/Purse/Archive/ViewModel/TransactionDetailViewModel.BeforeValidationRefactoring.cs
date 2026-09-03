// <copyright file="TransactionDetailViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.EntityFrameworkCore;
    using MVVMBaseTen.Messaging;

    public partial class TransactionDetailViewModel : ObservableDetailViewModel<Transaction>
    {
        [Obsolete("This is an example of a property changed partial method. It can be removed if not needed.")]
        partial void OnTransactionIdChanged(string? value)
        {
            // Add this to the Class Header, if you want to retrieve Query Parameters based on the TransactionId property:
            // [QueryProperty(nameof(TransactionId), "TransactionId")]
            // Debug.WriteLine($"TransactionId has changed to {value}");

            // if (!string.IsNullOrWhiteSpace(value))
            // {
            //    _ = this.LoadTransactionAsync(value);
            // }
        }

        #region Fields

        /// <summary>
        /// Defines the dbContext.
        /// </summary>
        private readonly LocalDbContext dbContext;

        /// <summary>
        /// The task representing category initialization.
        /// </summary>
        private readonly Task initCategoriesTask;

        /// <summary>
        /// The default category name loaded from the database.
        /// </summary>
        private string? defaultCategoryName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDetailViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The dbContext<see cref="LocalDbContext"/>.</param>
        public TransactionDetailViewModel(LocalDbContext dbContext)
        {
            this.Title = "Transaction Detail";
            this.dbContext = dbContext;

            this.Splits = new ObservableCollection<TransactionLineItem>();
            this.Splits.CollectionChanged += this.Splits_CollectionChanged;

            this.Vendors = new() { "Aldi", "Lidl", "Edeka", "REWE", "Kaufland", "Penny", "Netto", "dm", "Rossmann" };
            this.Categories = new() { "General" };

            this.ErrorsChanged += this.OnErrorsChanged;

            // Load categories from database asynchronously
            this.initCategoriesTask = this.LoadCategoriesAsync();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Categories.
        /// </summary>
        [ObservableProperty]
        public partial List<string> Categories
        {
            get; set;
        }


        /// <summary>
        /// Gets or sets the planned amount.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Display(Name = "Planned Amount")]
        [GreaterThan(nameof(TotalAmount), "Planned amount should be greater than total amount.")]
        public partial double PlannedAmount
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the selected vendor.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(1)]
        [MaxLength(100)]
        public partial string? SelectedVendor
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Splits.
        /// </summary>
        [ObservableProperty]
        public partial ObservableCollection<TransactionLineItem> Splits
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        [ObservableProperty]
        public partial string? Tags
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyPropertyChangedFor(nameof(PlannedAmount))]
        [Display(Name = nameof(AppResources.TotalAmount), ResourceType = typeof(AppResources))]
        [Range(0.01, 10000000.0, ErrorMessageResourceName = nameof(ValidationResources.RangeErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        public partial double TotalAmount
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        [ObservableProperty]
        public partial DateTime TransactionDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        [ObservableProperty]
        public partial string? TransactionId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        [ObservableProperty]
        public partial ObservableCollection<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the Vendors.
        /// </summary>
        [ObservableProperty]
        public partial List<string> Vendors
        {
            get; set;
        }

        /// <summary>
        /// Gets the expected query parameter name for loading an existing transaction.
        /// </summary>
        protected override string QueryIdParameterName => nameof(this.TransactionId);

        #endregion

        #region Methods

        /// <summary>
        /// Copies the entity values into the ViewModel properties.
        /// </summary>
        /// <param name="item">The transaction item.</param>
        public override void LoadFromItem(Transaction item)
        {
            ArgumentNullException.ThrowIfNull(item);

            this.TransactionId = item.Id;
            this.SelectedVendor = item.VendorName;
            this.TotalAmount = item.TotalAmount;
            this.PlannedAmount = item.PlannedAmount;
            this.TransactionDate = item.Date;
            this.Tags = item.Tags;
        }

        /// <summary>
        /// Saves the current transaction and its splits.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SaveCommandImplementation()
        {
            this.ValidateAllProperties();

            if (!this.CanGoBack)
            {
                return;
            }

            if (this.Item is null)
            {
                throw new InvalidOperationException("No transaction is loaded.");
            }

            bool exists = await this.dbContext.Transactions.AnyAsync(t => t.Id == this.Item.Id);

            if (!exists)
            {
                this.ApplyChangesToItem();

                foreach (TransactionLineItem split in this.Splits)
                {
                    split.TransactionId = this.Item.Id;
                }

                this.dbContext.Transactions.Add(this.Item);
                this.dbContext.TransactionLineItems.AddRange(this.Splits);
            }
            else
            {
                Transaction transaction = await this.dbContext.Transactions.FindAsync(this.Item.Id)
                    ?? throw new InvalidOperationException($"Transaction '{this.Item.Id}' was not found.");

                if (!ReferenceEquals(this.Item, transaction))
                {
                    this.Item = transaction;
                }

                this.ApplyChangesToItem();

                List<TransactionLineItem> existingSplits = await this.dbContext.TransactionLineItems
                    .Where(lineItem => lineItem.TransactionId == transaction.Id)
                    .ToListAsync();

                List<TransactionLineItem> splitsToDelete = existingSplits
                    .Where(dbSplit => this.Splits.All(uiSplit => uiSplit.Id != dbSplit.Id))
                    .ToList();

                if (splitsToDelete.Count > 0)
                {
                    this.dbContext.TransactionLineItems.RemoveRange(splitsToDelete);
                }

                foreach (TransactionLineItem uiSplit in this.Splits)
                {
                    TransactionLineItem? existingSplit = existingSplits
                        .FirstOrDefault(dbSplit => dbSplit.Id == uiSplit.Id);

                    if (existingSplit is null)
                    {
                        uiSplit.TransactionId = transaction.Id;
                        this.dbContext.TransactionLineItems.Add(uiSplit);
                        continue;
                    }

                    existingSplit.TransactionId = transaction.Id;
                    existingSplit.Amount = uiSplit.Amount;
                    existingSplit.Category = uiSplit.Category;
                    existingSplit.Tags = uiSplit.Tags;
                }
            }

            bool isNew = !exists;
            await this.dbContext.SaveChangesAsync();

            // Send changed message to update the history collection dynamically
            WeakReferenceMessenger.Default.Send(
                new EntityChangedMessage<Transaction>(
                    this.Item, 
                    isNew ? EntityChangeAction.Created : EntityChangeAction.Updated));

            // await Shell.Current.GoToAsync("..");
            // Occasionally, a user may cancel the transaction
            await Shell.Current.GoToAsync("..?cancel=true");

        }

        /// <summary>
        /// Copies the ViewModel values back into the entity.
        /// </summary>
        /// <param name="item">The transaction item.</param>
        public override void SaveToItem(Transaction item)
        {
            ArgumentNullException.ThrowIfNull(item);

            item.VendorName = this.SelectedVendor;
            item.TotalAmount = this.TotalAmount;
            item.PlannedAmount = this.PlannedAmount;
            item.Date = this.TransactionDate;
            item.Tags = this.Tags;
        }

        /// <summary>
        /// Shows the delete confirmation prompt for a transaction.
        /// </summary>
        /// <returns>A task returning whether the delete was confirmed.</returns>
        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Shell.Current is null)
            {
                return false;
            }

            string itemName = string.IsNullOrWhiteSpace(this.SelectedVendor)
                ? "this transaction"
                : $"the transaction '{this.SelectedVendor}'";

            return await Shell.Current.DisplayAlertAsync(
                "Delete transaction",
                $"Do you want to delete {itemName} and all corresponding splits?",
                "Delete",
                "Cancel");
        }

        /// <summary>
        /// Creates a new transaction for add scenarios.
        /// </summary>
        /// <returns>A new transaction.</returns>
        protected override Transaction CreateNewItem()
        {
            return new Transaction
            {
                Date = DateTime.Today
            };
        }

        /// <summary>
        /// Deletes the specified transaction and all corresponding splits.
        /// </summary>
        /// <param name="item">The transaction to delete.</param>
        /// <returns>A task.</returns>
        protected override async Task DeleteItemAsync(Transaction item)
        {
            List<TransactionLineItem> splits = await this.dbContext.TransactionLineItems
                .Where(lineItem => lineItem.TransactionId == item.Id)
                .ToListAsync();

            if (splits.Count > 0)
            {
                this.dbContext.TransactionLineItems.RemoveRange(splits);
            }

            if (this.dbContext.Entry(item).State == EntityState.Detached)
            {
                this.dbContext.Transactions.Attach(item);
            }

            this.dbContext.Transactions.Remove(item);
            await this.dbContext.SaveChangesAsync();

            // Send changed message to remove the transaction from the history collection dynamically
            WeakReferenceMessenger.Default.Send(
                new EntityChangedMessage<Transaction>(item, EntityChangeAction.Deleted));
        }

        /// <summary>
        /// Loads an existing transaction by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The loaded transaction or null.</returns>
        protected override async Task<Transaction?> LoadItemByIdAsync(string id)
        {
            return await this.dbContext.Transactions.FindAsync(id);
        }

        /// <summary>
        /// Called whenever the current item changes.
        /// </summary>
        /// <param name="item">The new item.</param>
        protected override void OnItemChanged(Transaction? item)
        {
            base.OnItemChanged(item);

            this.TransactionId = item?.Id;

            if (item is null)
            {
                _ = this.ResetEditorForNewTransactionAsync();
                return;
            }

            _ = this.LoadSplitsAsync(item.Id);
        }

        /// <summary>
        /// Adds a new split.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        private async Task AddSplitAsync()
        {
            await this.initCategoriesTask;

            string defaultCat = !string.IsNullOrEmpty(this.defaultCategoryName) && this.Categories.Contains(this.defaultCategoryName)
                ? this.defaultCategoryName
                : (this.Categories.FirstOrDefault() ?? "General");

            TransactionLineItem split = new()
            {
                Amount = default,
                Category = defaultCat,
            };

            this.Splits.Add(split);
            this.UpdateSplitProperties();
        }

        /// <summary>
        /// Cancels the current edit operation.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Clears the current splits and detaches event handlers.
        /// </summary>
        private void ClearSplits()
        {
            foreach (TransactionLineItem split in this.Splits)
            {
                split.PropertyChanged -= this.TransactionLineItem_PropertyChanged;
            }

            this.Splits.Clear();
        }

        /// <summary>
        /// Executes save through the base save command.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        private Task GoBackAsync()
        {
            this.SaveCommand.Execute(null);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Loads the splits for the current transaction.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns>A task.</returns>
        private async Task LoadSplitsAsync(string transactionId)
        {
            await this.initCategoriesTask;

            this.ClearSplits();

            List<TransactionLineItem> splits = await this.dbContext.TransactionLineItems
                .Where(lineItem => lineItem.TransactionId == transactionId)
                .ToListAsync();

            foreach (TransactionLineItem split in splits)
            {
                this.Splits.Add(split);
            }

            if (this.Splits.Count == 0)
            {
                string defaultCat = !string.IsNullOrEmpty(this.defaultCategoryName) && this.Categories.Contains(this.defaultCategoryName)
                    ? this.defaultCategoryName
                    : (this.Categories.FirstOrDefault() ?? "General");

                this.Splits.Add(new TransactionLineItem
                {
                    Amount = default,
                    Category = defaultCat,
                });
            }

            this.ValidateAllProperties();
        }

        /// <summary>
        /// Removes a split from the UI collection. Database deletes are handled during save.
        /// </summary>
        /// <param name="item">The split item.</param>
        [RelayCommand]
        private void RemoveSplit(TransactionLineItem item)
        {
            if (item is null)
            {
                return;
            }

            if (this.Splits.Contains(item))
            {
                this.Splits.Remove(item);
            }
        }

        /// <summary>
        /// Resets the editor to a clean state for creating a new transaction.
        /// </summary>
        /// <returns>A task.</returns>
        private async Task ResetEditorForNewTransactionAsync()
        {
            await this.initCategoriesTask;

            this.TransactionId = this.Item?.Id;
            this.SelectedVendor = this.Vendors.FirstOrDefault();
            this.TotalAmount = 0;
            this.PlannedAmount = 0;
            this.TransactionDate = this.Item?.Date ?? DateTime.Today;
            this.Tags = null;

            this.ClearSplits();
            this.ValidationResults.Clear();

            string defaultCat = !string.IsNullOrEmpty(this.defaultCategoryName) && this.Categories.Contains(this.defaultCategoryName)
                ? this.defaultCategoryName
                : (this.Categories.FirstOrDefault() ?? "General");

            this.Splits.Add(new TransactionLineItem
            {
                Amount = default,
                Category = defaultCat,
            });

            this.ValidateAllProperties();
        }

        /// <summary>
        /// Loads categories from the SQLite database.
        /// </summary>
        /// <returns>A task.</returns>
        private async Task LoadCategoriesAsync()
        {
            try
            {
                var dbCategories = await this.dbContext.Categories.ToListAsync();
                if (dbCategories.Count > 0)
                {
                    var categoryNames = dbCategories
                        .Select(c => c.DisplayName)
                        .OrderBy(c => c)
                        .ToList();

                    this.Categories = categoryNames;

                    var defaultCat = dbCategories.FirstOrDefault(c => c.IsDefault && !c.IsIncome)
                                     ?? dbCategories.FirstOrDefault(c => c.IsDefault)
                                     ?? dbCategories.FirstOrDefault();

                    if (defaultCat is not null)
                    {
                        this.defaultCategoryName = defaultCat.DisplayName;
                    }
                }
                else
                {
                    this.Categories = new() { "Lebensmittel", "Freizeit", "Miete", "Versicherung", "Mobilität", "Drogerie", "Tabak", "Alkohol" };
                    this.defaultCategoryName = this.Categories.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                this.Categories = new() { "Lebensmittel", "Freizeit", "Miete", "Versicherung", "Mobilität", "Drogerie", "Tabak", "Alkohol" };
                this.defaultCategoryName = this.Categories.FirstOrDefault();
                System.Diagnostics.Debug.WriteLine($"Failed to load categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles collection changes for splits.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void Splits_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems is not null)
            {
                foreach (TransactionLineItem newItem in e.NewItems)
                {
                    newItem.PropertyChanged += this.TransactionLineItem_PropertyChanged;
                }

                this.UpdateSplitProperties();
                return;
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && e.OldItems is not null)
            {
                foreach (TransactionLineItem oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= this.TransactionLineItem_PropertyChanged;
                }

                this.UpdateSplitProperties();
            }
        }

        /// <summary>
        /// Handles split item property changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void TransactionLineItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TransactionLineItem.Amount)
                || e.PropertyName == nameof(TransactionLineItem.Category))
            {
                this.UpdateSplitProperties();
            }
        }

        /// <summary>
        /// Updates calculated properties based on the current splits.
        /// </summary>
        private void UpdateSplitProperties()
        {
            this.TotalAmount = this.Splits.Sum(split => split.Amount);

            if (this.PlannedAmount == 0)
            {
                this.PlannedAmount = this.TotalAmount;
            }

            this.Tags = string.Join(", ",
                this.Splits
                    .Select(split => split.Category)
                    .Where(category => !string.IsNullOrWhiteSpace(category))
                    .Distinct());

            this.RefreshCanGoBack();
        }

        /// <summary>
        /// The OnSelectedVendorChanged.
        /// </summary>
        /// <param name="value">The value<see cref="string?"/>.</param>
        partial void OnSelectedVendorChanged(string? value)
        {
            Console.WriteLine($"Name has changed to {value}");
        }

        /// <summary>
        /// The OnSelectedVendorChanging.
        /// </summary>
        /// <param name="value">The value<see cref="string?"/>.</param>
        partial void OnSelectedVendorChanging(string? value)
        {
            Console.WriteLine($"Name is about to change to {value}");
        }

        /// <summary>
        /// The OnTotalAmountChanged.
        /// </summary>
        /// <param name="value">The value<see cref="double"/>.</param>
        partial void OnTotalAmountChanged(double value)
        {
            this.ValidateProperty(value, nameof(this.TotalAmount));
            this.ValidateProperty(this.PlannedAmount, nameof(this.PlannedAmount));
        }

        #endregion
    }
}