// <copyright file="IncomeSourcesCollectionView.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    using Purse.ViewModel;

    /// <summary>
    /// The income sources collection view.
    /// </summary>
    public partial class IncomeSourcesCollectionView : ContentPage
    {
        private bool isFirstAppearing = true;

        /// <summary>
        /// Gets or sets the ViewModel.
        /// </summary>
        public IncomeSourcesCollectionViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeSourcesCollectionView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public IncomeSourcesCollectionView(IncomeSourcesCollectionViewModel viewModel)
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
