// <copyright file="TransactionDetailViewModel.Selection.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Maui.Controls;

    public partial class TransactionDetailViewModel
    {
        private TransactionLineItem? currentEditingSplit;

        // Tracks which split triggered the category overlay
        private TransactionLineItem _activeSplitForCategory;

        [RelayCommand]
        private void OpenCategoryOverlay(TransactionLineItem split)
        {
            if (split == null)
                return;

            this._activeSplitForCategory = split;
            this.IsCategoryOverlayVisible = true;
        }

        [RelayCommand]
        private void CategorySelected(Category selectedCategory)
        {
            if (this._activeSplitForCategory != null && selectedCategory != null)
            {
                // Assign the string name to the split
                this._activeSplitForCategory.Category = selectedCategory.Name;
            }

            this.IsCategoryOverlayVisible = false;
        }


        [ObservableProperty]
        public partial bool IsCategoryOverlayVisible
        {
            get; set;
        }
        [ObservableProperty]
        public partial bool IsVendorOverlayVisible
        {
            get; set;
        }

        [ObservableProperty]
        public partial System.Collections.ObjectModel.ObservableCollection<Category> AvailableCategories { get; set; } = new();

        [ObservableProperty]
        public partial Category? OverlaySelectedCategory
        {
            get; set;
        }
        [ObservableProperty]
        public partial string? OverlaySearchText
        {
            get; set;
        }
        [ObservableProperty]
        public partial Vendor? OverlaySelectedVendor
        {
            get; set;
        }
        [ObservableProperty]
        public partial string? OverlayVendorSearchText
        {
            get; set;
        }

        [RelayCommand]
        private void OpenOverlay(TransactionLineItem? split)
        {
            this.currentEditingSplit = split;
            if (split != null)
            {
                this.OverlaySearchText = split.Category;
                this.OverlaySelectedCategory = this.AvailableCategories.FirstOrDefault(c => string.Equals(c.DisplayName, split.Category, StringComparison.OrdinalIgnoreCase) || string.Equals(c.Name, split.Category, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                this.OverlaySearchText = string.Empty;
                this.OverlaySelectedCategory = null;
            }
            this.IsCategoryOverlayVisible = true;
        }
        [RelayCommand]
        private void CancelOverlay()
        {
            this.IsCategoryOverlayVisible = false;
            this.currentEditingSplit = null;
        }
        [RelayCommand]
        private void SaveOverlay()
        {
            string categoryName = this.OverlaySelectedCategory?.Name ?? this.OverlaySearchText ?? string.Empty;
            if (this.currentEditingSplit == null)
            {
                var newLineItem = new TransactionLineItem { Amount = 0, Category = categoryName };
                this.Splits.Add(newLineItem);
            }
            else
            {
                this.currentEditingSplit.Category = categoryName;
            }
            this.UpdateSplitProperties();
            this.IsCategoryOverlayVisible = false;
            this.currentEditingSplit = null;
        }
        [RelayCommand]
        private void CancelVendorOverlay()
        {
            this.IsVendorOverlayVisible = false;
        }
        [RelayCommand]
        private void SaveVendorOverlay()
        {
            string vendorName = this.OverlaySelectedVendor?.Name ?? this.OverlayVendorSearchText ?? string.Empty;
            this.SelectedVendor = this.Vendors.FirstOrDefault(v => string.Equals(v.Name, vendorName, StringComparison.OrdinalIgnoreCase));
            this.IsVendorOverlayVisible = false;
        }

        [RelayCommand]
        private void OpenVendorOverlay()
        {
            this.OverlayVendorSearchText = this.SelectedVendor?.Name;
            this.OverlaySelectedVendor = this.Vendors.FirstOrDefault(v => v.Id == this.SelectedVendor?.Id);
            this.IsVendorOverlayVisible = true;
        }

        [RelayCommand]
        private async Task CreateExpenseCategory()
        {
            string name = this.OverlaySearchText?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name))
                return;
            var cat = new Category { Name = name, IsIncome = false };
            this.dbContext.Categories.Add(cat);
            await this.dbContext.SaveChangesAsync();
            this.AvailableCategories.Add(cat);
            this.OverlaySelectedCategory = cat;
        }

        [RelayCommand]
        private async Task CreateIncomeCategory()
        {
            string name = this.OverlaySearchText?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name))
                return;
            var cat = new Category { Name = name, IsIncome = true };
            this.dbContext.Categories.Add(cat);
            await this.dbContext.SaveChangesAsync();
            this.AvailableCategories.Add(cat);
            this.OverlaySelectedCategory = cat;
        }
        [RelayCommand]
        private async Task CreateVendor()
        {
            string name = this.OverlayVendorSearchText?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name))
                return;
            var v = new Vendor { Name = name };
            this.dbContext.Vendors.Add(v);
            await this.dbContext.SaveChangesAsync();
            this.Vendors.Add(v);
            this.OverlaySelectedVendor = v;
        }
        [RelayCommand]
        private async Task EditSelectedCategory()
        {
            if (this.OverlaySelectedCategory != null)
            {
                await Shell.Current.GoToAsync($"///CategoryDetail?CategoryId={this.OverlaySelectedCategory.Id}");
            }
        }
        [RelayCommand]
        private async Task EditSelectedVendor()
        {
            if (this.OverlaySelectedVendor != null)
            {
                await Shell.Current.GoToAsync($"///VendorDetail?VendorId={this.OverlaySelectedVendor.Id}");
            }
        }
    }
}