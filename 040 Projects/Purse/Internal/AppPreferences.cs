// <copyright file="StaticPreferences.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Internal
{
    /// <summary>
    /// Defines the <see cref="AppPreferences" />.
    /// </summary>
    internal static class AppPreferences
    {
        public static bool UseBiometrics
        {
            get
            {
                return Default.Get<bool>(nameof(UseBiometrics), true, string.Empty);
            }

            set
            {
                Default.Set(nameof(UseBiometrics), value);
            }
        }

        #region Fields

        /// <summary>
        /// Gets the Default.
        /// </summary>
        private static IPreferences Default
        {
            get
            {
                try
                {
                    return Microsoft.Maui.Storage.Preferences.Default;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Defines the defaultCurrency.
        /// </summary>
        private static string defaultCurrency = "€";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the DefaultCurrency.
        /// </summary>
        public static string DefaultCurrency
        {
            get
            {
                return Default.Get(nameof(DefaultCurrency), defaultCurrency);
            }
            set
            {
                Default.Set(nameof(DefaultCurrency), value);
            }
        }

        #endregion
    }
}
