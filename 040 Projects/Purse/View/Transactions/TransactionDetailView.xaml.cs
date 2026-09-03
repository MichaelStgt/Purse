// <copyright file="AddTransactionPage.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    using Purse.ViewModel;

    /// <summary>
    /// Defines the <see cref="TransactionDetailView" />.
    /// </summary>
    public partial class TransactionDetailView : ContentPage, IShellNavigationTarget
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDetailView"/> class.
        /// </summary>
        public TransactionDetailView()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDetailView"/> class.
        /// </summary>
        /// <param name="viewModel">The viewModel<see cref="TransactionDetailViewModel"/>.</param>
        public TransactionDetailView(TransactionDetailViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
            this.BindingContext = this.ViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ViewModel.
        /// </summary>
        public ObservableViewModel ViewModel
        {
            get; set;
        }

        #endregion
    }
}
