// <copyright file="MainView.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    /// <summary>
    /// Defines the <see cref="MainView" />.
    /// </summary>
    public partial class MainView : ContentPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// </summary>
        /// <param name="viewModel">The viewModel<see cref="ViewModel.MainViewModel"/>.</param>
        public MainView(ViewModel.MainViewModel viewModel)
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
        public ObservableViewModel? ViewModel
        {
            get; set;
        }

        #endregion
    }
}