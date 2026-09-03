// <copyright file="TransactionLineItem.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Server.Model
{
    using CommunityToolkit.Datasync.Server.EntityFrameworkCore;

    public class TransactionLineItem : EntityTableData
    {
        /// <summary>
        /// Gets or sets the TransactionId. This is the Foreign Key linking back to the Transaction.
        /// </summary>
        public string? TransactionId
        {
            get; set;
        }

        public double Amount
        {
            get; set;
        }

        private string? category = string.Empty;
        // [ObservableProperty]
        public string? Category
        {
            get { return this.category; }
            set
            {
                if (value == null)
                {
                    return;
                }

                this.category = value;
            }
        }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        public string? Tags
        {
            get; set;

        } = string.Empty;




    }
}