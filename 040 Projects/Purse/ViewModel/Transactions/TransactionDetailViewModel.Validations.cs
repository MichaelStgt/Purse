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

        #endregion
    }
}
