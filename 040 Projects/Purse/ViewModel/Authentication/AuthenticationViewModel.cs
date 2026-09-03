// <copyright file="AuthenticationViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using System.Windows.Input;
    using Purse.Services;

    /// <summary>
    /// Defines the <see cref="AuthenticationViewModel" />.
    /// </summary>
    public class AuthenticationViewModel
        : ObservableViewModel
    {
        #region Fields

        /// <summary>
        /// Defines the windowCreator.
        /// </summary>
        private readonly WindowCreator windowCreator;

        /// <summary>
        /// Defines the message.
        /// </summary>
        private string? message = "Logging you in ...";

        /// <summary>
        /// Defines the showRetry.
        /// </summary>
        private bool showRetry;

        private bool isAuthenticating;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationViewModel"/> class.
        /// </summary>
        /// <param name="windowCreator">The window creator used for creating application windows.</param>
        public AuthenticationViewModel(WindowCreator? windowCreator = null)
        {
            this.Title = string.Empty;
            this.windowCreator = windowCreator ?? throw new ArgumentNullException(nameof(windowCreator));
            this.AuthenticateCommand = new AsyncRelayCommand(async () =>
            {
                Debug.WriteLine("AuthenticationViewModel.AuthenticateCommand");
                this.ShowRetry = false;
                this.Message = "Retrying authentication...";
                await windowCreator.RetryAuthenticationAsync();
            });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether authenticating.
        /// </summary>
        /// <value>A bool</value>
        public bool IsAuthenticating
        {
            get => this.isAuthenticating;
            set => this.SetProperty(ref this.isAuthenticating, value);
        }

        /// <summary>
        /// Gets the AuthenticateCommand.
        /// </summary>
        public ICommand AuthenticateCommand
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public string? Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.SetProperty(ref this.message, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ShowRetry.
        /// </summary>
        public bool ShowRetry
        {
            get => this.showRetry;
            set
            {
                this.SetProperty(ref this.showRetry, value);
            }
        }

        #endregion
    }
}
