// <copyright file="MainWindowViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    /// <summary>
    /// The main window view model.
    /// </summary>
    public partial class MainWindowViewModel : ObservableViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.Title = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="title">The title<see cref="string"/>.</param>
        /// <param name="subTitle">The subTitle<see cref="string"/>.</param>
        /// <param name="showTitleBar">The showTitleBar<see cref="bool"/>.</param>
        public MainWindowViewModel(string? title = default, string? subTitle = default, bool showTitleBar = false)
        {
            this.Title = title;
            this.SubTitle = subTitle;
            this.ShowTitleBar = showTitleBar;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether ShowTitleBar.
        /// </summary>
        [ObservableProperty]
        public partial bool ShowTitleBar
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the SubTitle.
        /// </summary>
        [ObservableProperty]
        public partial string? SubTitle
        {
            get; set;
        }

        #endregion
    }
}
