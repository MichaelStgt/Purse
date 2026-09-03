// <copyright file="Category.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Shared.Model
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using MVVMBaseTen.Model;
    using Purse.Shared.Resources.Strings;
    using System.ComponentModel.DataAnnotations;
    using System.Resources;

    /// <summary>
    /// Defines a transaction category.
    /// </summary>
    [INotifyPropertyChanged]
    public partial class Category : OfflineClientEntity
    {
        private static readonly ResourceManager IncomeManager = new(
            "Purse.Shared.Resources.Strings.IncomeCategories",
            typeof(Category).Assembly);

        private static readonly ResourceManager ExpenseManager = new(
            "Purse.Shared.Resources.Strings.ExpenseCategories",
            typeof(Category).Assembly);

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this is an income category.
        /// </summary>
        [ObservableProperty]
        public partial bool IsIncome { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a pre-defined system category.
        /// </summary>
        [ObservableProperty]
        public partial bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this category is a default for new entries.
        /// </summary>
        [ObservableProperty]
        [Display(Name = "IsDefault", ResourceType = typeof(AppResources))]
        public partial bool IsDefault { get; set; }

        /// <summary>
        /// Gets the localized display name of the category.
        /// </summary>
        [Display(Name = "DisplayName", ResourceType = typeof(AppResources))]
        public string DisplayName
        {
            get
            {
                if (this.IsSystem)
                {
                    try
                    {
                        string? localized = this.IsIncome
                            ? IncomeManager.GetString(this.Id)
                            : ExpenseManager.GetString(this.Id);

                        if (!string.IsNullOrEmpty(localized))
                        {
                            return localized;
                        }
                    }
                    catch
                    {
                        // Fallback in case of resource loading exceptions
                    }
                }

                return this.Name;
            }
        }
    }
}
