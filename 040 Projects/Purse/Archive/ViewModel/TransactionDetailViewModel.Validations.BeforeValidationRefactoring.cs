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
    public partial class TransactionDetailViewModel
    {

        #region Properties


        /// <summary>
        /// Gets or sets the raw amount input string.
        /// </summary>

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
        /// Recalculates whether save/navigation is currently allowed.
        /// </summary>
        private void RefreshCanGoBack()
        {
            this.CanGoBack = this.ValidationResults.Count == 0;
        }

        #endregion
    }
}
