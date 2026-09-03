// <copyright file="TransactionDetailViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.EntityFrameworkCore;
    using Purse.Resources.Strings;
    using Purse.Shared.Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    /// <summary>
    /// Defines the <see cref="TransactionDetailViewModel" />.
    /// </summary>
    [QueryProperty(nameof(TransactionId), "TransactionId")]
    public partial class TransactionDetailViewModelVersion01
        : ObservableDetailViewModel<Transaction>
    {
        private readonly LocalDbContext dbContext;
        private string socialSecurityNumber = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDetailViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The dbContext<see cref="LocalDbContext"/>.</param>
        public TransactionDetailViewModelVersion01(LocalDbContext dbContext)
        {
            this.Title = "Transaction Detail";
            this.dbContext = dbContext;

            this.Splits = new ObservableCollection<TransactionLineItem>();
            this.Splits.CollectionChanged += this.Splits_CollectionChanged;

            this.Vendors = new() { "Aldi", "Lidl", "Edeka", "REWE", "Kaufland", "Penny", "Netto", "dm", "Rossmann" };
            this.Categories = new() { "Lebensmittel", "Freizeit", "Miete", "Versicherung", "Mobilität", "Drogerie", "Tabak", "Alkohol" };

            this.ErrorsChanged += this.OnErrorsChanged;

            this.ResetEditorForNewTransaction();
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        [ObservableProperty]
        public partial string? Tags
        {
            get;
            set;
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
            get;
            set;
        }

        partial void OnTotalAmountChanged(double value)
        {
            this.ValidateProperty(value, nameof(this.TotalAmount));
            this.ValidateProperty(this.PlannedAmount, nameof(this.PlannedAmount));
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
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        [ObservableProperty]
        public partial List<string> Categories
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(MVVMBaseTen.Resources.StringResources))]
        [MinLength(2, ErrorMessage = "Name should be longer than one character")]
        public partial string Name
        {
            get;
            set;
        } = "N1";

        /// <summary>
        /// Gets or sets the name2.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(2, ErrorMessage = "Name2 should be longer than one character")]
        public partial string Name2
        {
            get;
            set;
        } = "N2";

        /// <summary>
        /// Gets or sets the selected vendor.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(2)]
        [MaxLength(100)]
        public partial string? SelectedVendor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the social security number.
        /// </summary>
        [RegularExpression(
            @"^(?!000)(?!666)(?!9)\d{3}([- ]?)(?!00)\d{2}\1(?!0000)\d{4}$",
            ErrorMessage = "Invalid Social Security Number.")]
        public string SocialSecurityNumber
        {
            get => this.socialSecurityNumber;
            set => this.SetProperty(ref this.socialSecurityNumber, value, true);
        }

        /// <summary>
        /// Gets or sets the splits.
        /// </summary>
        [ObservableProperty]
        public partial ObservableCollection<TransactionLineItem> Splits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        [ObservableProperty]
        public partial DateTime TransactionDate
        {
            get;
            set;
        } = DateTime.Today;

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        [ObservableProperty]
        public partial string? TransactionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        [ObservableProperty]
        public partial ObservableCollection<ValidationResult> ValidationResults
        {
            get;
            set;
        } = new();

        /// <summary>
        /// Gets or sets the vendors.
        /// </summary>
        [ObservableProperty]
        public partial List<string> Vendors
        {
            get;
            set;
        }

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

            bool isNew = string.IsNullOrWhiteSpace(this.TransactionId);

            if (isNew)
            {
                this.Item = new Transaction();
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
                Transaction transaction = this.Item
                    ?? await this.dbContext.Transactions.FindAsync(this.TransactionId)
                    ?? throw new InvalidOperationException($"Transaction '{this.TransactionId}' was not found.");

                if (this.Item is null || !ReferenceEquals(this.Item, transaction))
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

            await this.dbContext.SaveChangesAsync();

            this.ResetEditorForNewTransaction();

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Adds a new split.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        private Task AddSplitAsync()
        {
            TransactionLineItem split = new()
            {
                Amount = default,
                Category = this.Categories.First(),
            };

            this.Splits.Add(split);
            this.UpdateSplitProperties();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Cancels the current edit operation.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        private async Task CancelAsync()
        {
            this.ResetEditorForNewTransaction();
            await Shell.Current.GoToAsync("..");
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
        /// Loads an existing transaction and its splits.
        /// </summary>
        /// <param name="id">The transaction id.</param>
        /// <returns>A task.</returns>
        private async Task LoadTransactionAsync(string id)
        {
            Transaction? transaction = await this.dbContext.Transactions.FindAsync(id);

            if (transaction is null)
            {
                throw new InvalidOperationException($"Transaction '{id}' was not found.");
            }

            this.SetItem(transaction);

            this.ClearSplits();

            List<TransactionLineItem> splits = await this.dbContext.TransactionLineItems
                .Where(lineItem => lineItem.TransactionId == id)
                .ToListAsync();

            foreach (TransactionLineItem split in splits)
            {
                this.Splits.Add(split);
            }

            this.ValidateAllProperties();
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
            this.Tags = string.Join(", ",
                this.Splits
                    .Select(split => split.Category)
                    .Where(category => !string.IsNullOrWhiteSpace(category))
                    .Distinct());
            this.RefreshCanGoBack();
            // this.OnPropertyChanged(nameof(this.CanGoBack));
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
        /// Resets the editor to a clean state for creating a new transaction.
        /// </summary>
        private void ResetEditorForNewTransaction()
        {
            this.Item = null;
            this.TransactionId = null;
            this.SelectedVendor = this.Vendors.FirstOrDefault();
            this.TotalAmount = 0;
            this.PlannedAmount = 0;
            this.TransactionDate = DateTime.Today;
            this.Tags = null;

            this.ClearSplits();
            this.ValidationResults.Clear();

            TransactionLineItem split = new()
            {
                Amount = default,
                Category = this.Categories.First(),
            };

            this.Splits.Add(split);

            this.ValidateAllProperties();
        }

        partial void OnSelectedVendorChanged(string? value)
        {
            Console.WriteLine($"Name has changed to {value}");
        }

        partial void OnSelectedVendorChanging(string? value)
        {
            Console.WriteLine($"Name is about to change to {value}");
        }

        partial void OnTransactionIdChanged(string? value)
        {
            Debug.WriteLine($"TransactionId has changed to {value}");

            if (!string.IsNullOrWhiteSpace(value))
            {
                _ = this.LoadTransactionAsync(value);
            }
        }

        partial void OnTransactionIdChanging(string? value)
        {
            Debug.WriteLine($"TransactionId is about to change to {value}");
        }
    }
}