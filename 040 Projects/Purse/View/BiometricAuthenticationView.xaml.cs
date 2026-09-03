// <copyright file="BiometricAuthenticationView.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    using Purse.Services;

    /// <summary>
    /// Defines the <see cref="BiometricAuthenticationView" />.
    /// </summary>
    public partial class BiometricAuthenticationView : ContentPage
    {
        #region Constructors

        //public BiometricAuthenticationView(AuthenticationViewModel? viewModel = null)
        //{
        //    this.InitializeComponent();
        //    this.ViewModel = viewModel ?? new AuthenticationViewModel();
        //    this.BindingContext = this.ViewModel;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="BiometricAuthenticationView"/> class.
        /// </summary>
        /// <param name="windowCreator">The windowCreator<see cref="WindowCreator"/>.</param>
        public BiometricAuthenticationView(WindowCreator windowCreator)
        {
            this.InitializeComponent();
            this.ViewModel = new AuthenticationViewModel(windowCreator);
            this.BindingContext = this.ViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ViewModel.
        /// </summary>
        public AuthenticationViewModel ViewModel
        {
            get; set;
        }

        #endregion
    }
}
