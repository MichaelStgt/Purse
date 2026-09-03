// <copyright file="TransactionLineItem.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Shared.Model
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using MVVMBaseTen.Model;

    /// <summary>
    /// Defines the <see cref="TransactionLineItem" />.
    /// </summary>
    [INotifyPropertyChanged]
    public partial class TransactionLineItem : OfflineClientEntity
    {
        /// <summary>
        /// Gets or sets the TransactionId. This is the Foreign Key linking back to the Transaction.
        /// </summary>
        public string? TransactionId
        {
            get; set;
        }

        [ObservableProperty]
        public partial double Amount
        {
            get; set;
        }

        private string? category = string.Empty;
        // [ObservableProperty]
        public string? Category
        {
            get
            {
                return this.category;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                // this.category = value;
                this.SetProperty(ref this.category, value);
            }
        }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        [ObservableProperty]
        public partial string? Tags
        {
            get; set;

            //get
            //{
            //    return this.tags;
            //}

            //set
            //{
            //    if (value == null)
            //    {
            //        return;
            //    }

            //    if (this.tags != value)
            //    {
            //        this.tags = value;
            //        this.OnPropertyChanged();
            //    }
            //}
        } = string.Empty;

        #region Fields

        ///// <summary>
        ///// Defines the amount.
        ///// </summary>
        //private double amount;

        ///// <summary>
        ///// Defines the category.
        ///// </summary>
        //private string category = string.Empty;

        ///// <summary>
        ///// Defines the tags.
        ///// </summary>
        //private string tags = string.Empty;

        /// <summary>
        /// Defines the transactionId.
        /// </summary>
        // private string transactionId = string.Empty;

        #endregion

        #region Properties

        ///// <summary>
        ///// Gets or sets the Amount.
        ///// </summary>
        //public double Amount
        //{
        //    get
        //    {
        //        return this.amount;
        //    }

        //    set
        //    {
        //        //if (this.amount != value)
        //        //{
        //        this.SetProperty(ref this.amount, value);
        //        //this.amount = value;
        //        //this.OnPropertyChanged();
        //        //}
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the Category.
        ///// </summary>
        //public string Category
        //{
        //    get
        //    {
        //        return this.category;
        //    }

        //    set
        //    {
        //        if (value == null)
        //        {
        //            return;
        //        }

        //        if (this.category != value)
        //        {
        //            this.category = value;
        //            this.OnPropertyChanged();
        //        }
        //    }
        //}





        #endregion
    }
}