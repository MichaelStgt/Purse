// <copyright file="VendorsCollectionViewModel.cs" company="Behr, Michael">
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
    /// The vendors collection view model.
    /// </summary>
    public partial class VendorsCollectionViewModel : ObservableCollectionViewModel<Vendor>
    {
        private readonly LocalDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorsCollectionViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public VendorsCollectionViewModel(LocalDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.Title = AppResources.Vendors;

            // Register messenger to dynamically update collections on changes
            WeakReferenceMessenger.Default.Register<EntityChangedMessage<Vendor>>(this, (r, m) =>
            {
                this.HandleVendorChanged(m.Value, m.Action);
            });
        }

        /// <summary>
        /// Loads the items asynchronously from the SQLite database.
        /// </summary>
        protected override async Task<IReadOnlyList<Vendor>> LoadItemsAsync(CancellationToken cancellationToken)
        {
            return await this.dbContext.ReadItemsAsync<Vendor>(
                query => query.OrderBy(v => v.Name),
                cancellationToken);
        }

        /// <summary>
        /// Navigation when a vendor is selected.
        /// </summary>
        protected override async Task OnSelectedItemChangedImplementationAsync(Vendor? value)
        {
            if (value is null || Shell.Current is null)
            {
                return;
            }

            try
            {
                await Shell.Current.GoToAsync(
                    nameof(VendorDetailView),
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
        /// Navigate to the Vendor detail screen to add a new vendor.
        /// </summary>
        [RelayCommand]
        private async Task AddVendorAsync()
        {
            if (Shell.Current is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(VendorDetailView));
        }

        private void HandleVendorChanged(Vendor vendor, EntityChangeAction action)
        {
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                if (action == EntityChangeAction.Created)
                {
                    this.ItemCollection.Add(vendor);
                }
                else if (action == EntityChangeAction.Updated)
                {
                    var existing = this.ItemCollection.FirstOrDefault(v => v.Id == vendor.Id);
                    if (existing is not null)
                    {
                        int index = this.ItemCollection.IndexOf(existing);
                        this.ItemCollection[index] = vendor;
                    }
                }
                else if (action == EntityChangeAction.Deleted)
                {
                    var existing = this.ItemCollection.FirstOrDefault(v => v.Id == vendor.Id);
                    if (existing is not null)
                    {
                        this.ItemCollection.Remove(existing);
                    }
                }
            });
        }
    }
}
