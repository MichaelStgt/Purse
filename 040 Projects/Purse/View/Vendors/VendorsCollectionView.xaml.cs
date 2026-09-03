// <copyright file="VendorsCollectionView.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    using Purse.ViewModel;

    /// <summary>
    /// The vendors collection view.
    /// </summary>
    public partial class VendorsCollectionView : ContentPage
    {
        private bool isFirstAppearing = true;

        /// <summary>
        /// Gets or sets the ViewModel.
        /// </summary>
        public VendorsCollectionViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorsCollectionView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public VendorsCollectionView(VendorsCollectionViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
            this.BindingContext = this.ViewModel;
        }

        /// <summary>
        /// On appearing.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.isFirstAppearing)
            {
                this.isFirstAppearing = false;
                this.ViewModel.LoadItemCollectionCommand.Execute(null);
            }
        }
    }
}
