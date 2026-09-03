// <copyright file="Country.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Shared.Model
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using MVVMBaseTen.Model;

    /// <summary>
    /// Defines a Country.
    /// </summary>
    [INotifyPropertyChanged]
    public partial class Country : OfflineClientEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [ObservableProperty]
        public partial string? Description { get; set; }
    }
}
