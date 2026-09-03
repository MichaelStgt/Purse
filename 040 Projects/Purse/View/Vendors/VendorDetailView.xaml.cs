// <copyright file="VendorDetailView.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    using Purse.ViewModel;

    /// <summary>
    /// The vendor detail view.
    /// </summary>
    public partial class VendorDetailView : ContentPage, IShellNavigationTarget
    {
        /// <summary>
        /// Gets or sets the ViewModel.
        /// </summary>
        public ObservableViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorDetailView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public VendorDetailView(VendorDetailViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
            this.BindingContext = this.ViewModel;
        }
    }
}
