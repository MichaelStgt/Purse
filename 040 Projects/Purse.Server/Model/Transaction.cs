// <copyright file="Transaction.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Server.Model
{
    using CommunityToolkit.Datasync.Server.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="Transaction" />.
    /// </summary>
    public partial class Transaction : EntityTableData
    {
        /// <summary>
        /// Gets or sets the vendor name.
        /// </summary>
        /// <value>A string?</value>
        public string? VendorName { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>A DateTime</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>A double</value>
        public double TotalAmount { get; set; }

        public double PlannedAmount { get; set; }

        #region Properties

        //private DateTime date = DateTime.Now;

        ///// <summary>
        ///// Gets or sets the Date.
        ///// </summary>
        //public DateTime Date
        //{
        //    get
        //    {
        //        return this.date;
        //    }
        //    set
        //    {
        //        if (this.date != value)
        //        {
        //            this.date = value;
        //            this.OnPropertyChanged();
        //        }
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the TotalAmount.
        ///// </summary>
        //public double TotalAmount
        //{
        //    get
        //    {
        //        return this.totalAmount;
        //    }

        //    set
        //    {
        //        if (this.totalAmount != value)
        //        {
        //            this.totalAmount = value;
        //            this.OnPropertyChanged();
        //        }
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the VendorName.
        ///// </summary>
        //public string VendorName
        //{
        //    get
        //    {
        //        return this.vendorName ?? string.Empty;
        //    }
        //    set
        //    {
        //        if (this.vendorName != value)
        //        {
        //            this.vendorName = value;
        //            this.OnPropertyChanged();
        //        }
        //    }
        //}

        #endregion
    }
}