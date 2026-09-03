// <copyright file="TransactionDetailViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using Microsoft.EntityFrameworkCore;
    using Purse.Services.Data;
    using Purse.Shared.Models;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="TransactionDetailViewModel" />.
    /// </summary>
    [QueryProperty(nameof(TransactionId), "TransactionId")]
    public partial class TransactionDetailViewModel : ObservableDetailViewModel<Transaction>
    {
        // Tracks which ValidationResults belong to which property
        private readonly Dictionary<string, List<ValidationResult>> validationResultsByProperty =
            new(StringComparer.Ordinal);

        private void RebuildAllValidationResults(object? sender)
        {
            this.ValidationResults.Clear();
            this.validationResultsByProperty.Clear();

            // Re-query errors for all public instance properties
            foreach (var prop in this.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                var propertyName = prop.Name;

                var results = this.GetErrors(propertyName)
                    .OfType<string>()
                    .Select(msg => new ValidationResult(msg, new[] { propertyName }))
                    .ToList();

                if (results.Count == 0)
                {
                    continue;
                }

                this.validationResultsByProperty[propertyName] = results;
                foreach (var vr in results)
                {
                    this.ValidationResults.Add(vr);
                }
            }
        }

        // Important: Move all the Stuff needed for Validation to the Base Class, so we can reuse it in other ViewModels as well. We could also consider creating a separate ValidationService that handles all validation logic and can be injected into the ViewModels. This would further separate concerns and make the code more modular and testable.
        // Todo: Move all the Stuff needed for Validation to the Base Class, so we can reuse it in other ViewModels as well. We could also consider creating a separate ValidationService that handles all validation logic and can be injected into the ViewModels. This would further separate concerns and make the code more modular and testable.
        private void OnErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.PropertyName))
            {
                // Object-level change: easiest is rebuild everything (or clear + add all)
                this.ValidationResults.Clear();

                foreach (var prop in this.GetType().GetProperties(
                             System.Reflection.BindingFlags.Instance |
                             System.Reflection.BindingFlags.Public
                             | System.Reflection.BindingFlags.NonPublic))
                {
                    var name = prop.Name;

                    //foreach (var msg in this.GetErrors(name).OfType<string>())
                    //    this.ValidationResults.Add(new ValidationResult(msg, new[] { name }));
                    foreach (var msg in this.GetErrors(name).OfType<ValidationResult>())
                    {
                        this.ValidationResults.Add(msg);
                    }
                }

                return;
            }

            var propertyName = e.PropertyName;

            // Remove everything currently shown for this property
            var toRemove = this.ValidationResults
                .Where(r => r.MemberNames?.Contains(propertyName) == true)
                .ToList();

            foreach (var r in toRemove)
            {
                this.ValidationResults.Remove(r);
            }

            // Add current errors for this property
            var messages = this.GetErrors(propertyName).OfType<ValidationResult>();
            foreach (var msg in messages)
            {
                // this.ValidationResults.Add(new ValidationResult(msg, new[] { propertyName }));
                this.ValidationResults.Add(msg);
            }
        }


        private void OnErrorsChangedOwn(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            // Usually e.PropertyName is set; handle null/empty defensively.
            //if (string.IsNullOrWhiteSpace(e.PropertyName))
            //{
            //    this.RebuildAllValidationResults(this);
            //    return;
            //}

            // var propertyName = e.PropertyName;

            //// 1) Remove old results for this property
            //if (this.validationResultsByProperty.TryGetValue(e.PropertyName!, out var oldResults))
            //{
            //    foreach (var vr in oldResults)
            //    {
            //        this.ValidationResults?.Remove(vr);
            //    }
            //}

            // this.ValidationResults?.Where(w => w.MemberNames.Contains(e.PropertyName)).ToList().ForEach(vr => this.ValidationResults.Remove(vr));

            var oldResult = this.ValidationResults?.Where(w => w.MemberNames.Contains(e.PropertyName)).ToList();
            if (/*oldResult != null &&*/ oldResult?.Any() == true)
            {
                oldResult?.ForEach(vr => this.ValidationResults?.Remove(vr));
            }
            // foreach (var vr in oldResult)
            // {
            //    this.ValidationResults?.Remove(vr);
            // }

            var newResults = this.GetErrors(e.PropertyName).ToList();

            //if (newResults.Count == 0)
            //{
            //    this.ValidationResults?.Where(w => w.MemberNames.Contains(e.PropertyName)).ToList().ForEach(vr => this.ValidationResults.Remove(vr));
            //    // this.validationResultsByProperty.Remove(propertyName);
            //    return;
            //}

            // this.validationResultsByProperty[propertyName] = newResults;
            foreach (var vr in newResults)
            {
                this.ValidationResults?.Add(vr);
            }
        }

        private void OnErrorsChanged2(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            // Usually e.PropertyName is set; handle null/empty defensively.
            //if (string.IsNullOrWhiteSpace(e.PropertyName))
            //{
            //    this.RebuildAllValidationResults(this);
            //    return;
            //}

            var propertyName = e.PropertyName;

            // 1) Remove old results for this property
            if (this.validationResultsByProperty.TryGetValue(e.PropertyName!, out var oldResults))
            {
                foreach (var vr in oldResults)
                {
                    this.ValidationResults?.Remove(vr);
                }
            }



            foreach (var vr in oldResults)
            {
                this.ValidationResults?.Remove(vr);
            }

            // 2) Add current results for this property
            //var newResults = GetErrors(propertyName)
            //    .OfType<string>() // ObservableValidator returns strings
            //    .Select(msg => new ValidationResult(msg, new[] { propertyName }))
            //    .ToList();
            var newResults = this.GetErrors(propertyName).ToList();

            if (newResults.Count == 0)
            {
                this.validationResultsByProperty.Remove(propertyName);
                return;
            }

            this.validationResultsByProperty[propertyName] = newResults;
            foreach (var vr in newResults)
            {
                this.ValidationResults?.Add(vr);
            }
        }

        /// <summary>
        /// The TransactionDetailViewModel_ErrorsChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.DataErrorsChangedEventArgs"/>.</param>
        private void TransactionDetailViewModel_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            this.ValidationResults.Where(w => w.MemberNames.Contains(e.PropertyName)).ToList().ForEach(vr => this.ValidationResults.Remove(vr));

            // this.Exceptions.Clear();
            // this.ValidationResults.Clear();
            // Debug.WriteLine($"Errors changed for property: {e.PropertyName}");
            if (this.HasErrors)
            {
                //List<System.ComponentModel.DataAnnotations.ValidationResult> errors = (this.GetErrors(e.PropertyName) as IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult>)?.ToList();
                IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> errors = this.GetErrors(e.PropertyName);
                Debug.WriteLine($"Errors for property: {e.PropertyName}, count: {errors?.Count() ?? 0}");
                foreach (var y in errors)
                {
                    // var x = this.ValidationResults.Any(a => a.ErrorMessage.Equals(y.ErrorMessage));
                    var x = this.ValidationResults.Any(a => a.MemberNames.Contains(e.PropertyName));
                    if (!x)
                    {
                        this.ValidationResults.Add(y);
                    }
                    Debug.WriteLine($"Error for {e.PropertyName}: {y.ErrorMessage}");
                }

                // var x = this.ValidationResults.Any(a => a.MemberNames.Contains(e.PropertyName));


                //if (!x)
                //{
                //    this.ValidationResults.AddRange(errors);
                //    this.OnPropertyChanged(nameof(this.ValidationResults));
                //}
                // this.ValidationResults = errors?.ToList() /*?? new List<System.ComponentModel.DataAnnotations.ValidationResult>()*/;
                // this.ValidationResults = (this.GetErrors(e.PropertyName) as IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult>)?.ToList() ?? new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                // this.ValidationResults = (ObservableCollection<ValidationResult>)(this.GetErrors(e.PropertyName) ?? new ObservableCollection<ValidationResult>());
                // this.ValidationResults = new ObservableCollection<ValidationResult>((this.GetErrors(e.PropertyName) ?? Enumerable.Empty<System.ComponentModel.DataAnnotations.ValidationResult>()).Select(vr => new ValidationResult(vr)));
                //foreach (var error in this.GetErrors(e.PropertyName) ?? Enumerable.Empty<System.ComponentModel.DataAnnotations.ValidationResult>())
                //{
                //    Debug.WriteLine($"Error for {e.PropertyName}: {error.ErrorMessage}");
                //    this.Exceptions.Add(error.ErrorMessage ?? "Unknown error");
                //    var x = new ValidationResult(error);
                //    //if (!this.ValidationResults.Contains(x))
                //    //{
                //    //    this.ValidationResults.Add(new ValidationResult(error));
                //    //}
                //}
            }
            else
            {
                this.ValidationResults?.Clear();
                //Debug.WriteLine($"No errors for property: {e.PropertyName}");
                //this.Exceptions.Clear();
                //this.ValidationResults.Clear();
            }
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

            this.Exceptions = new ObservableCollection<string>();
            this.Exceptions.CollectionChanged += this.Exceptions_CollectionChanged;

            this.Vendors = new() { "Aldi", "Lidl", "Edeka", "REWE", "Kaufland", "Penny", "Netto", "dm", "Rossmann" };
            this.SelectedVendor = this.Vendors.FirstOrDefault();
            this.Categories = new() { "Lebensmittel", "Freizeit", "Miete", "Versicherung", "Mobilität", "Drogerie", "Tabak", "Alkohol" };

            // Only add a default transactionLineItem if we are NOT editing an existing one
            if (string.IsNullOrEmpty(this.TransactionId))
            {
                this.AddSplitCommand.Execute(null);
            }
            // this.ErrorsChanged += this.TransactionDetailViewModel_ErrorsChanged;
            this.ErrorsChanged += this.OnErrorsChanged;

            // ErrorsChanged += (_, __) => RebuildValidationResults2();

        }

        //private void RebuildValidationResults2()
        //{
        //    ValidationResults.Clear();

        //    foreach (var prop in this.GetType().GetProperties(
        //                 //System.Reflection.BindingFlags.Instance |
        //                 //System.Reflection.BindingFlags.Public)
        //        )
        //    {
        //        var name = prop.Name;

        //        foreach (var msg in GetErrors(name).OfType<string>())
        //            ValidationResults.Add(new ValidationResult(msg, new[] { name }));
        //    }
        //}



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether CanGoBack.
        /// </summary>
        [NotifyCanExecuteChangedFor(nameof(SaveTransactionCommand))]
        [ObservableProperty]
        public partial bool CanGoBack { get; set; } = true;

        /// <summary>
        /// Gets or sets the Categories.
        /// </summary>
        [ObservableProperty]
        public partial List<string> Categories
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Exceptions.
        /// </summary>
        [ObservableProperty]
        public partial ObservableCollection<string> Exceptions
        {
            get; set;
        }

        // public string? TransactionId
        // {
        //    get => this.transactionId;
        //    set
        //    {
        //        this.transactionId = value;
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            // this.PageTitle = "Ausgabe bearbeiten"; // Edit Title
        //            _ = this.LoadTransactionAsync(value);
        //        }
        //    }
        // }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        //[Required(ErrorMessage = "Name is Required")]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(MVVMBaseTen.Resources.StringResources))]
        [MinLength(2, ErrorMessage = "Name should be longer than one character")]
        public partial string Name { get; set; } = "N1";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        //[Required(ErrorMessage = "Name is Required")]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(MVVMBaseTen.Resources.StringResources))]
        [MinLength(2, ErrorMessage = "Name2 should be longer than one character")]
        public partial string Name2 { get; set; } = "N2";

        /// <summary>
        /// Gets or sets the SelectedVendor.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        // [Required]
        // [Required(ErrorMessage = "SelectedVendor is Required")]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(MVVMBaseTen.Resources.StringResources))]
        [MinLength(2)]
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
        /// Gets or sets the TotalAmount.
        /// </summary>
        [ObservableProperty]
        public partial double TotalAmount
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the TransactionDate.
        /// </summary>
        [ObservableProperty]
        public partial DateTime TransactionDate { get; set; } = DateTime.Today;

        //[ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(CanGoBack))]

        //public partial bool IsValid
        //{
        //    get; set;
        //}

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
        public partial ObservableCollection<System.ComponentModel.DataAnnotations.ValidationResult> ValidationResults
        {
            get; set;
        } = new();

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
        /// The AddSplit.
        /// </summary>
        [RelayCommand]
        private void AddSplit()
        {
            // Splits.Add(new TransactionLineItem { Amount = 0, Category = Categories.First() });
            TransactionLineItem transactionLineItem = new TransactionLineItem
            {
                Amount = 0,
                Category = this.Categories.First(), //  Purse.Shared.AppConstants.Categories.First()
            };

            // transactionLineItem.PropertyChanged += this.TransactionLineItem_PropertyChanged;

            this.Splits.Add(transactionLineItem);
        }

        /// <summary>
        /// The CalculateSummary.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task CalculateSummary()
        {
            double splitSum = this.Splits.Sum(s => s.Amount);
            this.TotalAmount = splitSum;
        }

        /// <summary>
        /// The DoValidations.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task DoValidations()
        {
            this.Exceptions.Clear();

            if (string.IsNullOrWhiteSpace(this.SelectedVendor))
            {
                this.Exceptions.Add("Selected Vendor can not be Empty");
            }

            if (this.TotalAmount <= 0)
            {
                this.Exceptions.Add("Amount can not be null");
            }

            double splitSum = this.Splits.Sum(s => s.Amount);

            if (Math.Abs(splitSum - this.TotalAmount) > 0.01)
            {
                this.Exceptions.Add("Splits do not equal total amount!");
            }

            this.CanGoBack = this.Exceptions.Count == 0;
        }

        /// <summary>
        /// The Exceptions_CollectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/>.</param>
        private void Exceptions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add ||
               e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                // this.CanGoBack = this.Exceptions.Count == 0;
                // this.OnPropertyChanged(nameof(this.CanGoBack));
                // this.OnPropertyChanged(nameof(this.CanGoBack));
            }
        }

        /// <summary>
        /// The GoBackAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [RelayCommand]
        private async Task GoBackAsync()
        {
            // Navigate to the Add page
            // await Shell.Current.GoToAsync(nameof(TransactionDetailView));
            // await Shell.Current.GoToAsync(nameof(TransactionDetailView));
            this.SaveTransactionCommand.Execute(null);
        }

        /// <summary>
        /// The LoadTransactionAsync.
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
                this.TransactionDate = this.transaction.Date;

                this.Splits.Clear();
                // Load the related splits and add them to the UI
                var splits = await this.dbContext.TransactionLineItems.Where(l => l.TransactionId == id).ToListAsync();
                // this.Splits = new ObservableCollection<TransactionLineItem>(splits);

                foreach (TransactionLineItem transactionLineItem in splits)
                {
                    this.Splits.Add(transactionLineItem);
                }

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

        /// <summary>
        /// The SaveTransactionAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private async Task SaveTransactionAsync()
        {
            await this.DoValidations();

            if (string.IsNullOrEmpty(this.TransactionId))
            {
                // INSERT NEW
                // var transaction = new Transaction
                var transaction = new Transaction
                {
                    VendorName = this.SelectedVendor,
                    TotalAmount = this.TotalAmount,
                    Date = this.TransactionDate
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
                transaction?.Date = this.TransactionDate;
                this.dbContext.Transactions.Update(transaction!);

                foreach (var split in this.Splits)
                {
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
            this.Splits.Clear();

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// The Splits_CollectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/>.</param>
        private async void Splits_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TransactionLineItem newItem in e.NewItems)
                {
                    newItem.PropertyChanged += this.TransactionLineItem_PropertyChanged;
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TransactionLineItem oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= this.TransactionLineItem_PropertyChanged;
                    await this.CalculateSummary();
                    await this.DoValidations();
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
            if (e.PropertyName == nameof(TransactionLineItem.Amount))
            {
                await this.CalculateSummary();
                await this.DoValidations();
            }
        }

        /// <summary>
        /// The OnCanGoBackChanged.
        /// </summary>
        /// <param name="value">The value<see cref="bool"/>.</param>
        partial void OnCanGoBackChanged(bool value)
        {
            Console.WriteLine($"CanGoBack has changed to {value}");
        }

        /// <summary>
        /// The OnCanGoBackChanging.
        /// </summary>
        /// <param name="value">The value<see cref="bool"/>.</param>
        partial void OnCanGoBackChanging(bool value)
        {
            Console.WriteLine($"CanGoBack is about to change to {value}");
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
        /// The OnTransactionIdChanged.
        /// </summary>
        /// <param name="value">The value<see cref="string?"/>.</param>
        partial void OnTransactionIdChanged(string? value)
        {
            Debug.WriteLine($"TransactionId has changed to {value}");

            if (!string.IsNullOrEmpty(value))
            {
                // this.PageTitle = "Ausgabe bearbeiten"; // Edit Title
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
