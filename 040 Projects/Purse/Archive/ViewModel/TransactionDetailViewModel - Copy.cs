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
    using Purse.Shared.Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    public partial class ValidationResult : ObservableObject // : System.ComponentModel.DataAnnotations.ValidationResult
    {
        public ValidationResult(System.ComponentModel.DataAnnotations.ValidationResult validationResult)
        {
            ArgumentNullException.ThrowIfNull(validationResult);

            this.ErrorMessage = validationResult.ErrorMessage;
            this.MemberNames = string.Join(", ", validationResult.MemberNames);
        }

        // public IEnumerable<string> MemberNames
        [ObservableProperty]
        public partial string MemberNames
        {
            get; set;
        }

        [ObservableProperty]
        public partial string ErrorMessage { get; set; } = string.Empty;
    }

    [QueryProperty(nameof(TransactionId), "TransactionId")]
    public partial class TransactionDetailViewModel : ObservableDetailViewModel<Transaction>
    {
        //[ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(CanGoBack))]

        //public partial bool IsValid
        //{
        //    get; set;
        //}

        [ObservableProperty]
        public partial string? TransactionId
        {
            get; set;
        }

        partial void OnTransactionIdChanging(string? value)
        {
            Debug.WriteLine($"TransactionId is about to change to {value}");
        }

        partial void OnTransactionIdChanged(string? value)
        {
            Debug.WriteLine($"TransactionId has changed to {value}");

            if (!string.IsNullOrEmpty(value))
            {
                // this.PageTitle = "Ausgabe bearbeiten"; // Edit Title
                _ = this.LoadTransactionAsync(value);
            }

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



        [ObservableProperty]
        [NotifyDataErrorInfo]
        //[Required(ErrorMessage = "Name is Required")]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(MVVMBaseTen.Resources.StringResources))]
        [MinLength(2, ErrorMessage = "Name should be longer than one character")]
        public partial string Name
        {
            get;
            set;
            // get => _name;
            // set => SetProperty(ref _name, value, true);
        } = "D";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        // [Required]
        // [Required(ErrorMessage = "SelectedVendor is Required")]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(MVVMBaseTen.Resources.StringResources))]
        [MinLength(2)]
        [MaxLength(100)]
        public partial string? SelectedVendor
        {
            get;
            set;
        }

        [ObservableProperty]
        public partial List<System.ComponentModel.DataAnnotations.ValidationResult>? ValidationResults
        {
            get;
            set;
        }


        private void TransactionDetailViewModel_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            // this.Exceptions.Clear();
            // this.ValidationResults.Clear();
            // Debug.WriteLine($"Errors changed for property: {e.PropertyName}");
            if (this.HasErrors)
            {
                //List<System.ComponentModel.DataAnnotations.ValidationResult> errors = (this.GetErrors(e.PropertyName) as IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult>)?.ToList();
                IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> errors = this.GetErrors(e.PropertyName);
                this.ValidationResults = errors?.ToList() /*?? new List<System.ComponentModel.DataAnnotations.ValidationResult>()*/;
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

        partial void OnSelectedVendorChanging(string? value)
        {
            Console.WriteLine($"Name is about to change to {value}");

        }

        partial void OnSelectedVendorChanged(string? value)
        {
            Console.WriteLine($"Name has changed to {value}");
        }

        #region Fields

        private readonly LocalDbContext dbContext;

        private string? transactionId;

        #endregion

        #region Constructors

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
            this.ErrorsChanged += this.TransactionDetailViewModel_ErrorsChanged;
        }



        #endregion

        #region Properties

        [NotifyCanExecuteChangedFor(nameof(SaveTransactionCommand))]
        [ObservableProperty]
        public partial bool CanGoBack
        {
            get;
            set;
            //get
            //{
            //    return this.Exceptions.Count == 0;
            //}
        } = true;

        partial void OnCanGoBackChanging(bool value)
        {
            Console.WriteLine($"CanGoBack is about to change to {value}");
        }

        partial void OnCanGoBackChanged(bool value)
        {
            Console.WriteLine($"CanGoBack has changed to {value}");
        }

        [ObservableProperty]
        public partial List<string> Categories
        {
            get; set;
        }

        [ObservableProperty]
        public partial ObservableCollection<string> Exceptions
        {
            get; set;
        }

        [ObservableProperty]
        public partial ObservableCollection<TransactionLineItem> Splits
        {
            get; set;
        }

        [ObservableProperty]
        public partial double TotalAmount
        {
            get; set;
        }

        [ObservableProperty]
        public partial DateTime TransactionDate { get; set; } = DateTime.Today;



        [ObservableProperty]
        public partial List<string> Vendors
        {
            get; set;
        }

        #endregion

        #region Methods

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

        private async Task CalculateSummary()
        {
            double splitSum = this.Splits.Sum(s => s.Amount);
            this.TotalAmount = splitSum;
        }

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

        [RelayCommand]
        private async Task GoBackAsync()
        {
            // Navigate to the Add page
            // await Shell.Current.GoToAsync(nameof(TransactionDetailView));
            // await Shell.Current.GoToAsync(nameof(TransactionDetailView));
            this.SaveTransactionCommand.Execute(null);
        }

        // 3. Method to load existing data

        internal Transaction? transaction = null;

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

        //[RelayCommand]
        //private async Task SaveTransactionAsync2()
        //{
        //    if (string.IsNullOrWhiteSpace(this.SelectedVendor) || this.TotalAmount <= 0)
        //    {
        //        return;
        //    }

        //    double splitSum = this.Splits.Sum(s => s.Amount);
        //    if (Math.Abs(splitSum - this.TotalAmount) > 0.01)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Splits do not equal total amount!");
        //        return; // Add an alert here later
        //    }

        //    // 4. Handle Insert vs Update
        //    if (string.IsNullOrEmpty(this.TransactionId))
        //    {
        //        // INSERT NEW
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
        //        var transaction = await this.dbContext.Transactions.FindAsync(this.TransactionId);
        //        transaction.VendorName = this.SelectedVendor;
        //        transaction.TotalAmount = this.TotalAmount;
        //        transaction.Date = this.TransactionDate;

        //        // For splits, EF Core is already tracking the loaded ones. 
        //        // We just need to explicitly add any brand new rows the user created during the edit.
        //        foreach (var split in this.Splits)
        //        {
        //            if (string.IsNullOrEmpty(split.TransactionId))
        //            {
        //                split.TransactionId = transaction.Id;
        //                this.dbContext.TransactionLineItems.Add(split);
        //            }
        //        }
        //    }

        //    await this.dbContext.SaveChangesAsync();

        //    // 4. Navigate back to the History Page
        //    await Shell.Current.GoToAsync("..");

        //    // Reset Form & Navigate Back
        //    this.TransactionId = null;
        //    this.SelectedVendor = null;
        //    this.TotalAmount = 0;
        //    this.Splits.Clear();

        //    await Shell.Current.GoToAsync("..");
        //}

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

        private async void TransactionLineItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TransactionLineItem.Amount))
            {
                await this.CalculateSummary();
                await this.DoValidations();
            }

            // this.OnPropertyChanged(nameof(this.CanGoBack));
        }

        #endregion
    }
}
