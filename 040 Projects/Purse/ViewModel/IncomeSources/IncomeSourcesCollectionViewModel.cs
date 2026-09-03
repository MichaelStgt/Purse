// <copyright file="IncomeSourcesCollectionViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.ViewModel
{
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.EntityFrameworkCore;
    using MVVMBaseTen.Messaging;
    using Purse.Data;
    using Purse.Shared.Model;
    using Purse.View;
    using Purse.Resources.Strings;

    /// <summary>
    /// The income sources collection view model.
    /// </summary>
    public partial class IncomeSourcesCollectionViewModel : ObservableCollectionViewModel<IncomeSource>
    {
        private readonly LocalDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeSourcesCollectionViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public IncomeSourcesCollectionViewModel(LocalDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.Title = AppResources.IncomeSources;

            // Register messenger to dynamically update collections on changes
            WeakReferenceMessenger.Default.Register<EntityChangedMessage<IncomeSource>>(this, (r, m) =>
            {
                this.HandleIncomeSourceChanged(m.Value, m.Action);
            });
        }

        /// <summary>
        /// Loads the items asynchronously from the SQLite database.
        /// </summary>
        protected override async Task<IReadOnlyList<IncomeSource>> LoadItemsAsync(CancellationToken cancellationToken)
        {
            return await this.dbContext.ReadItemsAsync<IncomeSource>(
                query => query.OrderBy(isrc => isrc.Name),
                cancellationToken);
        }

        /// <summary>
        /// Navigation when an income source is selected.
        /// </summary>
        protected override async Task OnSelectedItemChangedImplementationAsync(IncomeSource? value)
        {
            if (value is null || Shell.Current is null)
            {
                return;
            }

            try
            {
                await Shell.Current.GoToAsync(
                    nameof(IncomeSourceDetailView),
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
        /// Navigate to the Income Source detail screen to add a new income source.
        /// </summary>
        [RelayCommand]
        private async Task AddIncomeSourceAsync()
        {
            if (Shell.Current is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(IncomeSourceDetailView));
        }

        private void HandleIncomeSourceChanged(IncomeSource incomeSource, EntityChangeAction action)
        {
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                if (action == EntityChangeAction.Created)
                {
                    this.ItemCollection.Add(incomeSource);
                }
                else if (action == EntityChangeAction.Updated)
                {
                    var existing = this.ItemCollection.FirstOrDefault(isrc => isrc.Id == incomeSource.Id);
                    if (existing is not null)
                    {
                        int index = this.ItemCollection.IndexOf(existing);
                        this.ItemCollection[index] = incomeSource;
                    }
                }
                else if (action == EntityChangeAction.Deleted)
                {
                    var existing = this.ItemCollection.FirstOrDefault(isrc => isrc.Id == incomeSource.Id);
                    if (existing is not null)
                    {
                        this.ItemCollection.Remove(existing);
                    }
                }
            });
        }
    }
}
