// <copyright file="WindowCreator.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Services
{
    using LibraryTen.Authentication;
    using Purse.Internal;
    using Purse.Resources.Strings;

    /// <summary>
    /// Defines the <see cref="WindowCreator" />.
    /// </summary>
    public partial class WindowCreator : IWindowCreator
    {
        private static WindowCreator? current;
        public static WindowCreator? Current
        {
            get
            {
                if (current == null)
                {
                    current = new WindowCreator();
                }
                return current;
            }
            // private set;
        }
        #region Fields

        /// <summary>
        /// Defines the biometric.
        /// </summary>
        private readonly IBiometric? biometric;

        /// <summary>
        /// The biometric view.
        /// </summary>
        private BiometricAuthenticationView? biometricView;

        #endregion

        #region Constructors
        public WindowCreator()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowCreator"/> class.
        /// </summary>
        /// <param name="biometric">The biometric<see cref="IBiometric"/>.</param>
        public WindowCreator(IBiometric biometric)
        {
            this.biometric = biometric ?? throw new ArgumentNullException(nameof(biometric));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current window.
        /// </summary>
        public Window? CurrentWindow
        {
            get; private set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The CreateWindow.
        /// </summary>
        /// <param name="app">The app<see cref="Application"/>.</param>
        /// <param name="activationState">The activationState<see cref="IActivationState"/>.</param>
        /// <returns>The <see cref="Window"/>.</returns>
        public Window CreateWindow(Application app, IActivationState? activationState)
        {
            if (false && Debugger.IsAttached)
            {
                Window mainWindow = this.CreateWindow(new AppShell(), AppResources.ApplicationTitle);
                return mainWindow;
            }

            if (AppPreferences.UseBiometrics == true)
            {
                this.biometricView = new BiometricAuthenticationView(this);
                var window = this.CreateWindow(this.biometricView);
                _ = this.AuthenticateUserAsync();
                return window;
            }

            // Move to the AppShell if no biometric or pin is set or, if authentication is not required/available.
            return this.CreateWindow(new AppShell());
        }

        /// <summary>
        /// Retries the authentication asynchronously.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task RetryAuthenticationAsync()
        {
            await this.AuthenticateUserAsync();
        }

        /// <summary>
        /// The AuthenticateUserAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task AuthenticateUserAsync()
        {
            try
            {
                if (this.biometric == null)
                {
                    this.UpdateBiometricView("Biometric hardware not available or not enrolled.", true);
                    return;
                }

                BiometricHwStatus result = await this.biometric.GetAuthenticationStatusAsync();
                if (result == BiometricHwStatus.Success)
                {
                    var authenticationRequest = new AuthenticationRequest()
                    {
                        Title = AppResources.AuthenticationRequestMessage,
                        NegativeText = AppResources.Cancel,
                        AllowPasswordAuth = false,
                    };

                    this.UpdateBiometricView(AppResources.AuthenticationRequestMessage, false, true);

                    var authenticationResult = await this.biometric.AuthenticateAsync(authenticationRequest, CancellationToken.None);

                    if (authenticationResult.Status == BiometricResponseStatus.Success)
                    {
                        AppShellViewModel? viewModel = ServiceHelper.GetService<AppShellViewModel>();
                        var appShell = new AppShell(viewModel);
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            Window? x = App.Current?.Windows.FirstOrDefault();
                            if (x != null)
                            {
                                x.Page = appShell;
#if WINDOWS

                                await this.ActivateWindowAsync(x);
#endif
                            }
                            // Application.Current?.MainView = appShell;
                        });
                    }
                    else
                    {
                        this.UpdateBiometricView(authenticationResult.ErrorMsg, true);
                    }
                }
                else
                {
                    this.UpdateBiometricView("Biometric hardware not available or not enrolled.", true);
                }
            }
            catch (Exception ex)
            {
                this.UpdateBiometricView($"Authentication error: {ex.Message}", true);
            }
        }

#if WINDOWS
        private async Task ActivateWindowAsync(Window window)
        {
            // Ensure handler wiring completes after swapping window.Page.
            await Task.Yield();
            //Application.Current?.ActivateWindow(window);
            //return;
            var mauiWinUIWindow = window.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
            mauiWinUIWindow?.Activate();

            IntPtr hwnd = this.TryGetHwnd(mauiWinUIWindow);
            if (hwnd != IntPtr.Zero)
            {
                _ = SetForegroundWindow(hwnd);
            }
        }

        private IntPtr TryGetHwnd(Microsoft.UI.Xaml.Window? winUIWindow)
        {
            if (winUIWindow == null)
            {
                return IntPtr.Zero;
            }

            return WinRT.Interop.WindowNative.GetWindowHandle(winUIWindow);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
#endif

        /// <summary>
        /// The CreateWindow.
        /// </summary>
        /// <param name="page">The page<see cref="Page"/>.</param>
        /// <returns>The <see cref="Window"/>.</returns>
        private Window CreateWindow(Page page, string? title = null)
        {
#if WINDOWS
            var window = new MainWindow(page, new MainWindowViewModel(title, default, true));
#else
            var window = new Window(page);
#endif

            return window;
        }

        /// <summary>
        /// Update biometric view.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="showRetry">If true, show retry.</param>
        private void UpdateBiometricView(string? message, bool showRetry, bool isAuthenticating = false)
        {
            if (this.biometricView != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (this.biometricView.BindingContext is AuthenticationViewModel viewModel)
                    {
                        viewModel.Message = message;
                        viewModel.IsAuthenticating = isAuthenticating;
                        viewModel.ShowRetry = showRetry;
                    }
                });
            }
        }

        #endregion
    }
}
