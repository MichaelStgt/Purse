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
    /// Defines the <see cref="TransactionDetailViewModel" />. Todo: Move all the stuff needed for validation to the
    /// base class, so it can be reused in other ViewModels as well.
    /// </summary>
    public partial class TransactionDetailViewModelVersion01
    {
        #region Fields

        /// <summary>
        /// Tracks which validation results belong to which property.
        /// </summary>
        private readonly Dictionary<string, List<ValidationResult>> validationResultsByProperty =
            new(StringComparer.Ordinal);

        /// <summary>
        /// Defines the rawAmount.
        /// </summary>
        private string? rawAmount;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the MyValueInput. Gets or sets a sample string input value.
        /// </summary>
        [ObservableProperty]
        public partial string? MyValueInput
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the raw amount input string.
        /// </summary>
        public string? RawAmount
        {
            get => this.rawAmount; set => this.SetProperty(ref this.rawAmount, value, true);
        }

        /// <summary>
        /// Gets or sets the validated numeric amount.
        /// </summary>
        [ObservableProperty]
        public partial double ValidatedAmount
        {
            get; set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates whether a string can be parsed as a double.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public static ValidationResult ValidateDouble(string value, ValidationContext context)
        {
            if (double.TryParse(value, out _))
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult("Please enter a valid number.");
        }

        /// <summary>
        /// Validates a numeric string input and stores the parsed value.
        /// </summary>
        /// <param name="value">The raw string value.</param>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public static ValidationResult ValidateNumeric(string value, ValidationContext context)
        {
            TransactionDetailViewModel vm = (TransactionDetailViewModel)context.ObjectInstance;

            if (string.IsNullOrWhiteSpace(value))
            {
                return new ValidationResult("Number required.");
            }

            if (double.TryParse(value, out double result))
            {
                vm.ValidatedAmount = result;
                return ValidationResult.Success!;
            }

            return new ValidationResult("Not a valid number.");
        }

        /// <summary>
        /// Handles validation error changes and keeps the aggregated validation list in sync.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.PropertyName))
            {
                this.ValidationResults.Clear();

                foreach (var prop in this.GetType().GetProperties(
                             System.Reflection.BindingFlags.Instance |
                             System.Reflection.BindingFlags.Public))
                {
                    string name = prop.Name;

                    foreach (ValidationResult msg in this.GetErrors(name).OfType<ValidationResult>())
                    {
                        this.ValidationResults.Add(new ValidationResult(msg.ErrorMessage, new[] { name }));
                    }
                }

                this.RefreshCanGoBack();
                return;
            }

            string propertyName = e.PropertyName;

            List<ValidationResult> toRemove = this.ValidationResults
                .Where(r => r.MemberNames?.Contains(propertyName) == true)
                .ToList();

            foreach (ValidationResult result in toRemove)
            {
                this.ValidationResults.Remove(result);
            }

            IEnumerable<ValidationResult> messages = this.GetErrors(propertyName).OfType<ValidationResult>();
            foreach (ValidationResult msg in messages)
            {
                this.ValidationResults.Add(new ValidationResult(msg.ErrorMessage, new[] { propertyName }));
            }

            this.RefreshCanGoBack();
        }

        /// <summary>
        /// Alternative validation aggregation implementation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        [Obsolete]
        private void OnErrorsChanged2(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            string? propertyName = e.PropertyName;

            bool found = this.validationResultsByProperty.TryGetValue(e.PropertyName!, out List<ValidationResult>? oldResults);
            if (found && oldResults != null)
            {
                foreach (ValidationResult vr in oldResults)
                {
                    this.ValidationResults?.Remove(vr);
                }
            }

            List<ValidationResult> newResults = this.GetErrors(propertyName).ToList();
            if (string.IsNullOrEmpty(propertyName) == false)
            {
                if (newResults.Count == 0)
                {
                    this.validationResultsByProperty.Remove(propertyName);
                    this.RefreshCanGoBack();
                    return;
                }

                this.validationResultsByProperty[propertyName] = newResults;
                foreach (ValidationResult vr in newResults)
                {
                    this.ValidationResults?.Add(vr);
                }
            }

            this.RefreshCanGoBack();
        }

        /// <summary>
        /// Older validation aggregation implementation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        [Obsolete]
        private void OnErrorsChangedOwn(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            List<ValidationResult>? oldResult = this.ValidationResults?
                .Where(w => w.MemberNames.Contains(e.PropertyName))
                .ToList();

            if (oldResult?.Any() == true)
            {
                oldResult.ForEach(vr => this.ValidationResults?.Remove(vr));
            }

            List<ValidationResult> newResults = this.GetErrors(e.PropertyName).ToList();

            foreach (ValidationResult vr in newResults)
            {
                this.ValidationResults?.Add(vr);
            }

            this.RefreshCanGoBack();
        }

        /// <summary>
        /// Recalculates whether save/navigation is currently allowed.
        /// </summary>
        private void RefreshCanGoBack()
        {
            this.CanGoBack = this.ValidationResults.Count == 0;
        }

        #endregion
    }
}
