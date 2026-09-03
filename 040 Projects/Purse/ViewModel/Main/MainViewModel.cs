// <copyright file="MainViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    // using CommunityToolkit.Mvvm.Messaging;
    using LibraryTen.Messaging;
    using Purse.Messaging;

    /// <summary>
    /// Defines the <see cref="MainViewModel" />.
    /// </summary>
    public partial class MainViewModel : ObservableViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.Title = AppResources.Home;
            this.Text = $"Clicked {this.count} times";
        }

        #endregion

        #region Properties

        private int count = 0;

        /// <summary>
        /// Gets or sets the Count.
        /// Either set this to nullable and initialize it in the constructor, or set it to a default value here.
        /// </summary>
        // [ObservableProperty]
        public /*partial*/ int Count
        {
            // get;
            // set;
            get
            {
                return this.count;
            }

            set
            {
                if (!EqualityComparer<int>.Default.Equals(this.count, value))
                {
                    // this.OnPropertyChanging(nameof(this.Count));
                    this.count = value;
                    this.OnPropertyChanged(nameof(this.Text));
                    WeakReferenceMessenger.Default.Send(new CountPropertyChangedMessage(this, nameof(this.Count), this.count - 1, this.count));
                }
            }

            //get
            //{
            //    return this.count;
            //}
            //set
            //{

            //    if (this.count != value)
            //    {
            //        this.count = value;
            //        this.OnPropertyChanged();
            //    }
            //}
        }

        /// <summary>
        /// Gets or sets the Text.
        /// </summary>
        [ObservableProperty]
        public partial string? Text
        {
            get; set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The CountUp.
        /// </summary>
        [RelayCommand]
        public void CountUp()
        {
            this.Count++;

            if (this.count == 1)
            {
                this.Text = $"Clicked {this.count} time";
            }
            else
            {
                this.Text = $"Clicked {this.count} times";
            }
            // WeakReferenceMessenger.Default.Send(new ConversionCompletedMessage(this.Count));
        }
        #endregion
    }
}
