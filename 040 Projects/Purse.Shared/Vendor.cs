// <copyright file="Vendor.cs" company="Behr, Michael">
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
    /// Defines a Vendor.
    /// </summary>
    [INotifyPropertyChanged]
    public partial class Vendor : OfflineClientEntity
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

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        [ObservableProperty]
        public partial string? Street { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        [ObservableProperty]
        public partial string? City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        [ObservableProperty]
        public partial string? State { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        [ObservableProperty]
        public partial string? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country identifier.
        /// </summary>
        [ObservableProperty]
        public partial string? CountryId { get; set; }
    }
}
