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

    /// <summary>
    /// Defines the <see cref="WindowCreator" />.
    /// </summary>
    public partial class WindowCreator : IWindowCreator
    {
        #region Fields

        /// <summary>
        /// Defines the biometric.
        /// </summary>
        private readonly IBiometric? biometric;

        /// <summary>
        /// Defines the current.
        /// </summary>
        private static WindowCreator? current;

        /// <summary>
        /// The biometric view.
        /// </summary>
        private BiometricAuthenticationView? biometricView;
        private AppShellViewModel? appShellViewModel;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowCreator"/> class.
        /// </summary>
        public WindowCreator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowCreator"/> class.
        /// </summary>
        /// <param name="biometric">The biometric<see cref="IBiometric"/>.</param>
        public WindowCreator(IBiometric? biometric = null, AppShellViewModel? viewModel = default)
        {
            this.appShellViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.biometric = biometric ?? throw new ArgumentNullException(nameof(biometric));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Current.
        /// </summary>
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
        }

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
            // Keep this for testing purposes
            if (Debugger.IsAttached)
            {
                return this.CreateAppShell();
            }

            // If there is no biometric implementation or the platform does not support it,
            // fall back to AppShell as well.
            if (this.biometric == null || !this.biometric.IsPlatformSupported)
            {
                return this.CreateAppShell();
            }

            // If the user disabled biometric authentication in preferences, go straight to AppShell.
            if (!AppPreferences.UseBiometrics)
            {
                return this.CreateAppShell();
            }

            // Biometric is enabled in preferences and supported by platform:
            // show the biometric view and let AuthenticateUserAsync decide what to do next.
            this.biometricView = new BiometricAuthenticationView(this);
            var biometricWindow = this.CreateWindow(this.biometricView, AppResources.ApplicationTitle);

            // Remember the biometric window as the current window.
            this.CurrentWindow = biometricWindow;

            _ = this.AuthenticateUserAsync(); // fire-and-forget, fully async inside
            return biometricWindow;
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
        /// The CreateAppShell.
        /// </summary>
        /// <returns>The <see cref="Window"/>.</returns>
        internal Window CreateAppShell()
        {
            Window mainWindow = this.CreateWindow(new AppShell(this.appShellViewModel), AppResources.ApplicationTitle);

            // Remember the main window instance so we can navigate later.
            this.CurrentWindow = mainWindow;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
#if WINDOWS
                if (mainWindow != null)
                {
                    await this.ActivateWindowAsync(mainWindow);
                }
#endif
            });

            return mainWindow;
        }

        /// <summary>
        /// The SetAppShell.
        /// </summary>
        internal void SetAppShell()
        {
            AppShellViewModel? viewModel = ServiceHelper.GetService<AppShellViewModel>();
            var appShell = new AppShell(viewModel);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var currentWindow = this.CurrentWindow;
                if (currentWindow == null)
                {
                    return;
                }

                currentWindow.Page = appShell;
#if WINDOWS
                await this.ActivateWindowAsync(currentWindow);
#endif
            });
        }

        /// <summary>
        /// The AuthenticateUserAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task AuthenticateUserAsync()
        {
            try
            {
                // Check the current biometric hardware/enrollment status before attempting authentication.
                BiometricHwStatus? result = await this.biometric!.GetAuthenticationStatusAsync();
                if (result == BiometricHwStatus.Success)
                {
                    var authenticationRequest = new AuthenticationRequest()
                    {
                        Title = AppResources.AuthenticationRequestMessage,
                        NegativeText = AppResources.Cancel,
                        AllowPasswordAuth = false,
                    };

                    // Show the "authenticating" state.
                    this.UpdateBiometricView(AppResources.AuthenticationRequestMessage, false, true);

                    var authenticationResult = await this.biometric.AuthenticateAsync(authenticationRequest, CancellationToken.None);

                    if (authenticationResult.Status == BiometricResponseStatus.Success)
                    {
                        this.SetAppShell();
                    }
                    else
                    {
                        // Show the platform error / user cancel message and allow retry.
                        this.UpdateBiometricView(authenticationResult.ErrorMsg, true);
                    }
                }
                // There are several reasons why the hardware state is not available. For all Except Success, we
                // just redirect to the AppShell
                else
                {
                    this.SetAppShell();
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
        /// <param name="title">The title<see cref="string?"/>.</param>
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
        /// <param name="isAuthenticating">The isAuthenticating<see cref="bool"/>.</param>
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
