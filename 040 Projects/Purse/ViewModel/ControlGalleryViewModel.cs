// <copyright file="ControlGalleryViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    /// <summary>
    /// Defines the <see cref="ControlGalleryViewModel" />.
    /// </summary>
    public partial class ControlGalleryViewModel : ObservableViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlGalleryViewModel"/> class.
        /// </summary>
        public ControlGalleryViewModel()
        {

            this.Title = AppResources.ControlGallery;
            this.Description = "This ViewModel and the corresponding View is for Testing Purposes and Color Management";
        }

        /// <summary>
        /// The ToolbarItem1 command.
        /// </summary>
        [RelayCommand]
        public void ToolbarItem1()
        {
            Debug.WriteLine("CountUp");
        }
        /// <summary>
        /// The NavigateToAboutPage.
        /// </summary>
        [RelayCommand]
        public void NavigateToAboutPage()
        {
            Shell.Current.GoToAsync(nameof(AboutView));
        }
        #endregion
    }
}