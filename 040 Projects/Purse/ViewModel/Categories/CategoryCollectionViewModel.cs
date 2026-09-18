// <copyright file="CategoryCollectionViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using MVVMBaseTen.Messaging;
    using Purse.Data;
    using Purse.Shared.Model;
    using Purse.View;
    using System.Collections.ObjectModel;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The category history/management collection view model.
    /// </summary>
    public partial class CategoryCollectionViewModel : ObservableCollectionViewModel<Category>
    {
        private readonly LocalDbContext dbContext;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowExpenses))]
        public partial bool ShowIncome
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether to show expense categories.
        /// </summary>
        public bool ShowExpenses => !this.ShowIncome;

        [ObservableProperty]
        public partial ObservableCollection<Category> FilteredCategories { get; private set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCollectionViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CategoryCollectionViewModel(LocalDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.Title = Purse.Resources.Strings.AppResources.Categories;

            // Reactively rebuild separate lists when the main collection changes
            this.ItemCollection.CollectionChanged += (s, e) => this.RebuildFilteredCollection();

            // Register messenger to dynamically update collections on changes (Created, Updated, Deleted)
            WeakReferenceMessenger.Default.Register<EntityChangedMessage<Category>>(this, (r, m) =>
            {
                this.HandleCategoryChanged(m.Value, m.Action);
            });
        }

        /// <summary>
        /// Loads the items asynchronously from the SQLite database.
        /// </summary>
        protected override async Task<IReadOnlyList<Category>> LoadItemsAsync(CancellationToken cancellationToken)
        {
            var categories = await this.dbContext.ReadItemsAsync<Category>(
                query => query.OrderBy(c => c.Name),
                cancellationToken);

            var categoryUsage = await this.dbContext.Set<TransactionLineItem>()
                .Where(t => t.Category != null)
                .GroupBy(t => t.Category)
                .Select(g => new { CategoryName = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.CategoryName!, v => v.Count, cancellationToken);

            foreach (var category in categories)
            {
                int count = 0;
                if (category.Name != null && categoryUsage.TryGetValue(category.Name, out int cName))
                    count += cName;
                if (category.DisplayName != null && category.DisplayName != category.Name && categoryUsage.TryGetValue(category.DisplayName, out int cDisplay))
                    count += cDisplay;
                if (category.Id != null && categoryUsage.TryGetValue(category.Id, out int cId))
                    count += cId;
                    
                category.TransactionCount = count;
            }

            return categories;
        }

        /// <summary>
        /// Navigation when a category is selected.
        /// </summary>
        protected override async Task OnSelectedItemChangedImplementationAsync(Category? value)
        {
            if (value is null || Shell.Current is null)
            {
                return;
            }

            try
            {
                await Shell.Current.GoToAsync(
                    nameof(CategoryDetailView),
                    new ShellNavigationQueryParameters
                    {
                        { this.QueryIdParameterName, value.Id },
                    });
            }
            finally
            {
                this.SelectedItem = null;
            }
        }

        /// <summary>
        /// Navigate to the Category detail screen to add a new category.
        /// </summary>
        [RelayCommand]
        private async Task AddItemAsync()
        {
            await Shell.Current.GoToAsync(nameof(CategoryDetailView));
        }

        [RelayCommand]
        private void SwitchTab()
        {
            this.ShowIncome = !this.ShowIncome;

            // this.ShowIncome = tabType == Purse.Resources.Strings.AppResources.Income;
        }

        /// <summary>
        /// Switch between Income and Expense tabs.
        /// </summary>
        [RelayCommand]
        private void SelectTab(string tabType)
        {
            this.ShowIncome = tabType == Purse.Resources.Strings.AppResources.Income;
        }

        /// <summary>
        /// Callback when the ShowIncome property changes to automatically trigger collection filter rebuild.
        /// </summary>
        /// <param name="value">The new value.</param>
        partial void OnShowIncomeChanged(bool value)
        {
            this.RebuildFilteredCollection();
        }

        private void RebuildFilteredCollection()
        {
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                this.FilteredCategories.Clear();

                foreach (var item in this.ItemCollection)
                {
                    if (item.IsIncome == this.ShowIncome)
                    {
                        this.FilteredCategories.Add(item);
                    }
                }
            });
        }

        private void HandleCategoryChanged(Category category, EntityChangeAction action)
        {
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                if (action == EntityChangeAction.Created)
                {
                    this.ItemCollection.Add(category);
                }
                else if (action == EntityChangeAction.Updated)
                {
                    var existing = this.ItemCollection.FirstOrDefault(c => c.Id == category.Id);
                    if (existing is not null)
                    {
                        int index = this.ItemCollection.IndexOf(existing);
                        this.ItemCollection[index] = category;
                    }
                }
                else if (action == EntityChangeAction.Deleted)
                {
                    var existing = this.ItemCollection.FirstOrDefault(c => c.Id == category.Id);
                    if (existing is not null)
                    {
                        this.ItemCollection.Remove(existing);
                    }
                }
            });
        }
    }
}
