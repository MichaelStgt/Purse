// <copyright file="AboutView.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    /// <summary>
    /// Defines the <see cref="AboutView" />.
    /// </summary>
    public partial class AboutView : ContentPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutView"/> class.
        /// </summary>
        public AboutView()
        {
            this.Content =
                new Label
                {
                    Text = "Register the AboutViewModel as Singleton in MauiProgram.cs",
                    TextColor = Microsoft.Maui.Graphics.Colors.Red,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutView"/> class.
        /// </summary>
        /// <param name="viewModel">The viewModel<see cref="AboutViewModel"/>.</param>
        public AboutView(AboutViewModel viewModel)
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