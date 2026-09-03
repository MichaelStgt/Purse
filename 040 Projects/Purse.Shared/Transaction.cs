// <copyright file="Transaction.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Shared.Model
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using MVVMBaseTen.Model;

    // using MVVMBaseTen.Model;

    /// <summary>
    /// Defines the <see cref="Transaction" />.
    /// </summary>
    [INotifyPropertyChanged]
    public partial class Transaction : OfflineClientEntity
    {
        [ObservableProperty]
        public partial string? Temp
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the vendor name.
        /// </summary>
        /// <value>A string?</value>
        [ObservableProperty]
        public partial string? VendorName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>A DateTime</value>
        [ObservableProperty]
        public partial DateTime Date
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>A double</value>
        [ObservableProperty]
        public partial double TotalAmount
        {
            get; set;
        }


        [ObservableProperty]
        public partial double PlannedAmount
        {
            get; set;
        }

        [ObservableProperty]
        public partial string? Tags
        {
            get; set;

        } = string.Empty;
    }
}