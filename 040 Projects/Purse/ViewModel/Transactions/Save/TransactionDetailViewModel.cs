// <copyright file="TransactionDetailViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using Microsoft.EntityFrameworkCore;
    using Purse.Shared.Model;
    // using Purse.Services.Data;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="TransactionDetailViewModel" />.
    /// </summary>
    [QueryProperty(nameof(TransactionId), "TransactionId")]
    public partial class TransactionDetailViewModel : ObservableDetailViewModel<Transaction>
    {
        [ObservableProperty]
        public partial string? Tags
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the totalAmount.
        /// </summary>
        private double totalAmount = 0;


        /// <summary>
        /// Gets or sets the TotalAmount.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyPropertyChangedFor(nameof(PlannedAmount))]
        [Display(Name = nameof(Purse.Resources.Strings.AppResources.TotalAmount), ResourceType = typeof(Purse.Resources.Strings.AppResources))]
        [Range(0.01, 10000000.0, ErrorMessageResourceName = nameof(Purse.Resources.Strings.ValidationResources.RangeErrorMessage), ErrorMessageResourceType = typeof(Purse.Resources.Strings.ValidationResources))]
        // [Required(ErrorMessageResourceName = nameof(Purse.Resources.Strings.ValidationResources.RangeErrorMessage), ErrorMessageResourceType = typeof(Purse.Resources.Strings.ValidationResources))]
        // public partial double TotalAmount
        public partial double TotalAmount
        {
            get; set;
            //get
            //{
            //    return this.totalAmount;
            //}
            //set
            //{
            //    if (this.totalAmount != value)
            //    {
            //        this.SetProperty(ref this.totalAmount, value, true);
            //    }
            //    // this.SetProperty(ref this.totalAmount, value, true);

            //    this.ValidateAllProperties();
            //    //this.ValidateProperty(value, nameof(this.TotalAmount));
            //    //this.ValidateProperty(value, nameof(this.PlannedAmount));
            //    // this.OnPropertyChanged(nameof(this.CanGoBack));

            //}
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

        /// <summary>
        /// Gets or sets the PlannedAmount. The Validation Properties here will stay hardcoded, as we do not want to
        /// keep this after we migrated this to the MVVMBaseTen.Samples Project
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Display(Name = "Planned Amount"/*, ResourceType = typeof(Purse.Resources.Strings.AppResources)*/)]
        [GreaterThan(nameof(TotalAmount), "Planned amount should be greater than total amount.")]
        public partial double PlannedAmount
        {
            get; set;
        }
        partial void OnPlannedAmountChanged(double value)
        {
            // this.ValidateAllProperties();
            // this.ValidateProperty(value, nameof(this.PlannedAmount));
        }

        #region Fields

        /// <summary>
        /// Defines the dbContext.
        /// </summary>
        private readonly LocalDbContext dbContext;

        // 3. Method to load existing data

        /// <summary>
        /// Defines the transaction.
        /// </summary>
        internal Transaction? transaction = null;

        /// <summary>
        /// The social security number.
        /// </summary>
        private string socialSecurityNumber = string.Empty;



        /// <summary>
        /// Defines the transactionId.
        /// </summary>
        private string? transactionId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDetailViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The dbContext<see cref="LocalDbContext"/>.</param>
        public TransactionDetailViewModel(LocalDbContext dbContext)
        {
            this.Title = AppResources.TransactionDetail;
            this.dbContext = dbContext;

            this.Splits = new ObservableCollection<TransactionLineItem>();
            this.Splits.CollectionChanged += this.Splits_CollectionChanged;

            this.Vendors = new() { "Aldi", "Lidl", "Edeka", "REWE", "Kaufland", "Penny", "Netto", "dm", "Rossmann" };
            this.SelectedVendor = this.Vendors.FirstOrDefault();
            this.Categories = new() { "Lebensmittel", "Freizeit", "Miete", "Versicherung", "Mobilität", "Drogerie", "Tabak", "Alkohol" };

            // Only add a default transactionLineItem if we are NOT editing an existing one
            if (string.IsNullOrEmpty(this.TransactionId))
            {
                this.AddSplitCommand.Execute(null);
            }
            // Add the ErrorsChanged event handler to trigger validation when properties change.
            // It is also responsible for updating the CanExecute state of the SaveCommand based on validation results.
            this.ErrorsChanged += this.OnErrorsChanged;
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
        /// Gets or sets the Name.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(MVVMBaseTen.Resources.StringResources))]
        [MinLength(2, ErrorMessage = "Name should be longer than one character")]
        public partial string Name { get; set; } = "N1";

        /// <summary>
        /// Gets or sets the Name2.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(2, ErrorMessage = "Name2 should be longer than one character")]
        public partial string Name2 { get; set; } = "N2";



        /// <summary>
        /// Gets or sets the SelectedVendor.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(2)]
        [MaxLength(100)]
        public partial string? SelectedVendor
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the social security number. This is just an example to show how to use the RegularExpression
        /// attribute for validation. It is not used anywhere else in the code and does not have to be implemented in
        /// the UI. The regex pattern checks for a valid US Social Security Number format (XXX-XX-XXXX) and also ensures
        /// that certain invalid numbers are not accepted (e.g., 000, 666, or numbers starting with 9).
        /// </summary>
        [RegularExpression(
            @"^(?!000)(?!666)(?!9)\d{3}([- ]?)(?!00)\d{2}\1(?!0000)\d{4}$",
            ErrorMessage = "Invalid Social Security Number.")]

        public string SocialSecurityNumber
        {
            get => this.socialSecurityNumber; set => this.SetProperty(ref this.socialSecurityNumber, value, true);
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
        /// Gets or sets the TransactionDate.
        /// </summary>
        [ObservableProperty]
        public partial DateTime TransactionDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or sets the TransactionId.
        /// </summary>
        [ObservableProperty]
        public partial string? TransactionId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the ValidationResults.
        /// </summary>
        [ObservableProperty]
        public partial ObservableCollection<System.ComponentModel.DataAnnotations.ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the Vendors.
        /// </summary>
        [ObservableProperty]
        public partial List<string> Vendors
        {
            get; set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The SaveCommandImplementation.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public override async Task SaveCommandImplementation()
        {
            this.ValidateAllProperties();
            if (!this.CanGoBack)
            {
                return;
            }
            if (string.IsNullOrEmpty(this.TransactionId))
            {
                // await this.RetrieveTags();

                // INSERT NEW
                var transaction = new Transaction
                {
                    //VendorName = this.SelectedVendor,
                    //TotalAmount = this.TotalAmount,
                    //PlannedAmount = this.PlannedAmount,
                    //Date = this.TransactionDate,
                    VendorName = this.SelectedVendor,
                    TotalAmount = this.TotalAmount,
                    PlannedAmount = this.PlannedAmount,
                    Date = this.TransactionDate,
                    Tags = this.Tags,
                };

                foreach (var split in this.Splits)
                {
                    split.TransactionId = transaction.Id;
                }

                this.dbContext.Transactions.Add(transaction);
                this.dbContext.TransactionLineItems.AddRange(this.Splits);
            }
            else
            {
                // UPDATE EXISTING
                // Todo: instead of loading the Current Transaction again, we could also just attach it to the context and mark it as modified. EF Core would then generate the correct UPDATE statement based on the primary key. However, since we also need to handle the related splits (add new ones, update existing ones, delete removed ones), it's easier to just load the existing transaction with its splits, apply the changes and let EF Core figure out the rest.
                Transaction? transaction = await this.dbContext.Transactions.FindAsync(this.TransactionId);

                transaction?.VendorName = this.SelectedVendor;
                transaction?.TotalAmount = this.TotalAmount;
                transaction?.PlannedAmount = this.PlannedAmount;
                transaction?.Date = this.TransactionDate;
                // await this.RetrieveTags();

                transaction?.Tags = this.Tags;
                this.dbContext.Transactions.Update(transaction!);

                foreach (var split in this.Splits)
                {
                    // if the Transaction has not yet an Id, assigne one first and then Add the Transaction to the Database
                    // It must be a new one.
                    if (string.IsNullOrEmpty(split.TransactionId))
                    {
                        split.TransactionId = transaction?.Id;
                        this.dbContext.TransactionLineItems.Add(split);
                    }
                    else
                    {
                        this.dbContext.TransactionLineItems.Update(split);
                    }
                }
            }

            await this.dbContext.SaveChangesAsync();

            // Reset Form & Navigate Back
            this.TransactionId = null;
            this.SelectedVendor = null;
            this.TotalAmount = 0;
            this.PlannedAmount = 0;
            this.Splits.Clear();

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// The AddSplit.
        /// </summary>
        [RelayCommand]
        private async Task AddSplitAsync()
        {
            TransactionLineItem transactionLineItem = new TransactionLineItem
            {
                Amount = default,
                Category = this.Categories.First(), //  Purse.Shared.AppConstants.Categories.First()
            };

            this.Splits.Add(transactionLineItem);
            await this.UpdateSplitProperties();
        }

        /// <summary>
        /// The CancelAsync. Todo: Move this inside the Base class
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// The GoBackAsync. Todo: Move this inside the Base class
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [RelayCommand]
        private async Task GoBackAsync()
        {
            // this.SaveTransactionCommand.Execute(null);
            this.SaveCommand.Execute(null);
        }

        /// <summary>
        /// The LoadTransactionAsync. Todo: think about if we could move this to the base class Todo: Also do think
        /// about replacing this with a Command instead of a simple Task, as this would allow us to better control when
        /// this is executed and also to handle any potential errors that might occur during the loading process.
        /// Additionally, we could also consider using a more generic method in the base class that can load any type of
        /// entity based on an id and then have the specific implementation in the derived class to handle the mapping
        /// of the loaded entity to the ViewModel properties.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task LoadTransactionAsync(string id)
        {
            // var transaction = await this.dbContext.Transactions.FindAsync(id);
            this.transaction = await this.dbContext.Transactions.FindAsync(id);
            if (this.transaction != null)
            {
                this.SelectedVendor = this.transaction.VendorName;
                this.TotalAmount = this.transaction.TotalAmount;
                this.PlannedAmount = this.transaction.PlannedAmount;
                this.TransactionDate = this.transaction.Date;
                this.Tags = this.transaction.Tags;

                this.Splits.Clear();
                // Load the related splits and add them to the UI
                var splits = await this.dbContext.TransactionLineItems.Where(l => l.TransactionId == id).ToListAsync();
                // this.Splits = new ObservableCollection<TransactionLineItem>(splits);

                foreach (TransactionLineItem transactionLineItem in splits)
                {
                    this.Splits.Add(transactionLineItem);
                }
                this.ValidateAllProperties();

            }
        }

        /// <summary>
        /// The RemoveSplit.
        /// </summary>
        /// <param name="item">The item<see cref="TransactionLineItem"/>.</param>
        [RelayCommand]
        private void RemoveSplit(TransactionLineItem item)
        {
            if (this.Splits.Contains(item))
            {
                this.Splits.Remove(item);

                // If this is an existing item from the DB, tell EF Core to delete it
                if (!string.IsNullOrEmpty(this.TransactionId))
                {
                    this.dbContext.TransactionLineItems.Remove(item);
                }
            }
        }

        ///// <summary>
        ///// The SaveTransactionAsync.
        ///// </summary>
        ///// <returns>The <see cref="Task"/>.</returns>
        //[RelayCommand(CanExecute = nameof(CanGoBack))]
        //private async Task SaveTransactionAsync()
        //{
        //    this.SaveCommandImplementation();
        //    return;
        //    if (string.IsNullOrEmpty(this.TransactionId))
        //    {
        //        // INSERT NEW
        //        // var transaction = new Transaction
        //        var transaction = new Transaction
        //        {
        //            VendorName = this.SelectedVendor,
        //            TotalAmount = this.TotalAmount,
        //            Date = this.TransactionDate
        //        };

        //        foreach (var split in this.Splits)
        //        {
        //            split.TransactionId = transaction.Id;
        //        }

        //        this.dbContext.Transactions.Add(transaction);
        //        this.dbContext.TransactionLineItems.AddRange(this.Splits);
        //    }
        //    else
        //    {
        //        // UPDATE EXISTING
        //        // Todo: instead of loading the Current Transaction again, we could also just attach it to the context and mark it as modified. EF Core would then generate the correct UPDATE statement based on the primary key. However, since we also need to handle the related splits (add new ones, update existing ones, delete removed ones), it's easier to just load the existing transaction with its splits, apply the changes and let EF Core figure out the rest.
        //        Transaction? transaction = await this.dbContext.Transactions.FindAsync(this.TransactionId);

        //        transaction?.VendorName = this.SelectedVendor;
        //        transaction?.TotalAmount = this.TotalAmount;
        //        transaction?.Date = this.TransactionDate;
        //        this.dbContext.Transactions.Update(transaction!);

        //        foreach (var split in this.Splits)
        //        {
        //            if (string.IsNullOrEmpty(split.TransactionId))
        //            {
        //                split.TransactionId = transaction?.Id;
        //                this.dbContext.TransactionLineItems.Add(split);

        //            }
        //            else
        //            {
        //                this.dbContext.TransactionLineItems.Update(split);
        //            }
        //        }
        //    }

        //    await this.dbContext.SaveChangesAsync();

        //    // Reset Form & Navigate Back
        //    this.TransactionId = null;
        //    this.SelectedVendor = null;
        //    this.TotalAmount = 0;
        //    this.Splits.Clear();

        //    await Shell.Current.GoToAsync("..");
        //}

        /// <summary>
        /// The Splits_CollectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/>.</param>
        private async void Splits_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (TransactionLineItem newItem in e.NewItems)
                    {
                        newItem.PropertyChanged += this.TransactionLineItem_PropertyChanged;
                        await this.UpdateSplitProperties();
                        // await this.RetrieveTags();
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (TransactionLineItem oldItem in e.OldItems)
                    {
                        oldItem.PropertyChanged -= this.TransactionLineItem_PropertyChanged;
                        await this.UpdateSplitProperties();
                        //await this.CalculateSummary();
                        //await this.RetrieveTags();
                        // this.OnPropertyChanged(nameof(this.CanGoBack));
                        // await this.DoValidations();
                    }
                }

            }
        }

        /// <summary>
        /// The TransactionLineItem_PropertyChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.PropertyChangedEventArgs"/>.</param>
        private async void TransactionLineItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TransactionLineItem.Amount)
                || e.PropertyName == nameof(TransactionLineItem.Category))
            {
                //await this.CalculateSummary();
                //await this.RetrieveTags();
                await this.UpdateSplitProperties();
                // this.ValidateAllProperties();
                // this.ValidateProperty(this.TotalAmount, nameof(this.TotalAmount));
            }
        }
        private async Task UpdateSplitProperties()
        {
            double splitSum = this.Splits.Sum(s => s.Amount);
            this.TotalAmount = splitSum;
            string s = string.Join(", ", this.Splits.Select(s => s.Category).Where(c => !string.IsNullOrEmpty(c)).Distinct());
            this.Tags = s;
            this.OnPropertyChanged(nameof(this.CanGoBack));
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

        /*partial*/



        /// <summary>
        /// The OnTransactionIdChanged.
        /// </summary>
        /// <param name="value">The value<see cref="string?"/>.</param>
        partial void OnTransactionIdChanged(string? value)
        {
            Debug.WriteLine($"TransactionId has changed to {value}");

            if (!string.IsNullOrEmpty(value))
            {
                _ = this.LoadTransactionAsync(value);
            }
        }

        /// <summary>
        /// The OnTransactionIdChanging.
        /// </summary>
        /// <param name="value">The value<see cref="string?"/>.</param>
        partial void OnTransactionIdChanging(string? value)
        {
            Debug.WriteLine($"TransactionId is about to change to {value}");
        }

        #endregion
    }

}
