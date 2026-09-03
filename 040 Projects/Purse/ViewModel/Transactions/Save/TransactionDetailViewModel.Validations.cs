// <copyright file="TransactionDetailViewModel.Validations.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="TransactionDetailViewModel" />. Todo: Move all the Stuff needed for Validation to the
    /// Base Class, so we can reuse it in other ViewModels as well. We could also consider creating a separate
    /// ValidationService that handles all validation logic and can be injected into the ViewModels. This would further
    /// separate concerns and make the code more modular and testable.
    /// </summary>
    public partial class TransactionDetailViewModel
    {
        private string? rawAmount;

        // [Required(ErrorMessage = "Value cannot be empty")]
        // [CustomValidation(typeof(TransactionDetailViewModel), nameof(ValidateNumeric))]
        public string? RawAmount
        {
            get => this.rawAmount;
            set => this.SetProperty(ref this.rawAmount, value, true); // The 'true' is critical
        }

        // This is where your actual double lives after validation
        [ObservableProperty]
        public partial double ValidatedAmount
        {
            get; set;
        }

        // private double _validatedAmount;

        public static ValidationResult ValidateNumeric(string value, ValidationContext context)
        {
            var vm = (TransactionDetailViewModel)context.ObjectInstance;

            if (string.IsNullOrWhiteSpace(value))
            {
                return new ValidationResult("Number required.");
            }

            if (double.TryParse(value, out double result))
            {
                vm.ValidatedAmount = result; // Update the actual double property
                return ValidationResult.Success;
            }

            return new ValidationResult("Not a valid number.");
        }

        [ObservableProperty]
        // [NotifyDataErrorInfo]
        // [CustomValidation(typeof(TransactionDetailViewModel), nameof(ValidateDouble))]
        // private string _myValueInput;
        public partial string? MyValueInput
        {
            get; set;
        }

        public static ValidationResult ValidateDouble(string value, ValidationContext context)
        {
            if (double.TryParse(value, out _))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Please enter a valid number.");
        }

        #region Fields

        /// <summary>
        /// Defines the validationResultsByProperty. Tracks which ValidationResults belong to which property
        /// </summary>
        private readonly Dictionary<string, List<ValidationResult>> validationResultsByProperty =
            new(StringComparer.Ordinal);

        #endregion

        #region Methods

        /// <summary>
        /// The OnErrorsChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.DataErrorsChangedEventArgs"/>.</param>
        private void OnErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.PropertyName))
            {
                // Object-level change: easiest is rebuild everything (or clear + add all)
                this.ValidationResults.Clear();

                foreach (var prop in this.GetType().GetProperties(
                             System.Reflection.BindingFlags.Instance |
                             System.Reflection.BindingFlags.Public))
                {
                    var name = prop.Name;

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
                this.ValidationResults.Add(new ValidationResult(msg.ErrorMessage, new[] { propertyName }));
                // this.ValidationResults.Add(msg);
            }

            if (this.CanGoBack != (this.ValidationResults.Count == 0))
            {
                this.CanGoBack = this.ValidationResults.Count == 0;
            }
        }

        /// <summary>
        /// The OnErrorsChanged2.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.DataErrorsChangedEventArgs"/>.</param>
        private void OnErrorsChanged2(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            // Usually e.PropertyName is set; handle null/empty defensively.
            //if (string.IsNullOrWhiteSpace(e.PropertyName))
            //{
            //    this.RebuildAllValidationResults(this);
            //    return;
            //}

            string? propertyName = e.PropertyName;

            bool found = this.validationResultsByProperty.TryGetValue(e.PropertyName!, out List<ValidationResult>? oldResults);
            // 1) Remove old results for this property
            // if (this.validationResultsByProperty.TryGetValue(e.PropertyName!, out List<ValidationResult>? oldResults))
            if (found && oldResults != null)
            {
                foreach (var vr in oldResults)
                {
                    this.ValidationResults?.Remove(vr);
                }
            }

            // 2) Add current results for this property
            //var newResults = GetErrors(propertyName)
            //    .OfType<string>() // ObservableValidator returns strings
            //    .Select(msg => new ValidationResult(msg, new[] { propertyName }))
            //    .ToList();
            var newResults = this.GetErrors(propertyName).ToList();
            if (newResults != null && string.IsNullOrEmpty(propertyName) == false)
            {

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
        }

        /// <summary>
        /// The OnErrorsChangedOwn.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.DataErrorsChangedEventArgs"/>.</param>
        private void OnErrorsChangedOwn(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            var oldResult = this.ValidationResults?.Where(w => w.MemberNames.Contains(e.PropertyName)).ToList();
            if (/*oldResult != null &&*/ oldResult?.Any() == true)
            {
                oldResult?.ForEach(vr => this.ValidationResults?.Remove(vr));
            }

            var newResults = this.GetErrors(e.PropertyName).ToList();

            foreach (var vr in newResults)
            {
                this.ValidationResults?.Add(vr);
            }
        }

        [Obsolete]
        private void RebuildValidationResults2()
        {
            this.ValidationResults.Clear();

            foreach (var prop in this.GetType().GetProperties(
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public)
                )
            {
                var name = prop.Name;

                foreach (var msg in this.GetErrors(name).OfType<ValidationResult>())
                {
                    this.ValidationResults?.Add(msg);
                }
            }
        }

        /// <summary>
        /// The DoValidations.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Obsolete]
        private async Task DoValidations()
        {
            // this.Exceptions.Clear();

            if (string.IsNullOrWhiteSpace(this.SelectedVendor))
            {
                // this.Exceptions.Add("Selected Vendor can not be Empty");
            }

            if (this.TotalAmount <= 0)
            {
                // this.Exceptions.Add("Amount can not be null");
            }

            double splitSum = this.Splits.Sum(s => s.Amount);

            if (Math.Abs(splitSum - this.TotalAmount) > 0.01)
            {
                // this.Exceptions.Add("Splits do not equal total amount!");
            }
        }

        /// <summary>
        /// The RebuildAllValidationResults.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        [Obsolete]
        private void RebuildAllValidationResults(object? sender)
        {
            this.ValidationResults.Clear();
            this.validationResultsByProperty.Clear();

            // Re-query errors for all public instance properties
            foreach (var prop in this.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                var propertyName = prop.Name;

                var results = this.GetErrors(propertyName).ToList();
                //.OfType<ValidationResult>()
                //// .Select(msg => new ValidationResult(msg, new[] { propertyName }))
                //.Select(msg => msg)
                //.ToList();

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

        /// <summary>
        /// The TransactionDetailViewModel_ErrorsChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/>.</param>
        /// <param name="e">The e<see cref="System.ComponentModel.DataErrorsChangedEventArgs"/>.</param>
        [Obsolete("This was the first try to implement this. Refer to 'OnErrorsChanged' for the final version")]
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

        #endregion
    }
}
