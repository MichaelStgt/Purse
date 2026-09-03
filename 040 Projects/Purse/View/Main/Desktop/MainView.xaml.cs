// <copyright file="MainView.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View.Desktop
{
    // using CommunityToolkit.Mvvm.Messaging;
    using LibraryTen.Messaging;
    using Purse.Messaging;

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

        #region Methods

        /// <summary>
        /// The OnAppearing.
        /// </summary>
        protected async override void OnAppearing()
        {
            var x = WeakReferenceMessenger.Default.IsRegistered<CountPropertyChangedMessage>(this);
            if (!x)
            {
                //WeakReferenceMessenger.Default.Register<ConversionCompletedMessage>(this, (recipient, message) =>
                //{
                //    _ = this.DisplayAlertAsync("Conversion Completed", $"The result of ViewModel.Count is: {message.Value:N2}", "OK");
                //});

                WeakReferenceMessenger.Default.Register<CountPropertyChangedMessage>(this, (recipient, message) =>
                {
                    this.AdditionalFeaturesSpan.Text = $"The value of ViewModel.{message.PropertyName} changed from {message.OldValue:N0} to {message.NewValue:N0}";
                    // _ = this.DisplayAlertAsync("Conversion Completed", $"The result of ViewModel.Count changed from: {message.OldValue:N2} to: {message.NewValue:N2}", "OK");

                });
            }
        }

        #endregion
    }
}
