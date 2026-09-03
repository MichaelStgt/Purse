// <copyright file="AppShellViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael 2018-2026.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    public partial class AppShellViewModel : ObservableViewModel
    {
        #region Constructors

        public AppShellViewModel()
        {
        }

        #endregion

        #region Methods

        [RelayCommand]
        private void Close()
        {
            App.Current?.Quit();
        }

        [RelayCommand]
        private void SwitchTheme()
        {
            if (App.Current?.UserAppTheme == AppTheme.Dark)
            {
                Debug.WriteLine("Current theme is Dark, setting to Light");
                App.Current.UserAppTheme = AppTheme.Light;
            }
            else if (App.Current?.UserAppTheme == AppTheme.Light)
            {
                Debug.WriteLine("Current theme is Light, setting to Dark");
                App.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                Debug.WriteLine("Current theme is Unspecified, setting to Dark as default");
                App.Current?.UserAppTheme = AppTheme.Dark;
            }
        }

        #endregion
    }
}
