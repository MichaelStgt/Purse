// <copyright file="ControlGalleryView.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    public partial class ControlGalleryView : ContentPage
    {
        public ControlGalleryView(ControlGalleryViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
            this.BindingContext = this.ViewModel;
        }

        public ObservableViewModel? ViewModel
        {
            get;
            set;
        }

    }
}