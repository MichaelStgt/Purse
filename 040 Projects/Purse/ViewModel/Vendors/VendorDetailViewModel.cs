// <copyright file="VendorDetailViewModel.cs" company="Behr, Michael">
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
    using Microsoft.EntityFrameworkCore;
    using MVVMBaseTen.Messaging;
    using Purse.Data;
    using Purse.Resources.Strings;
    using Purse.Shared.Model;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The vendor detail view model.
    /// </summary>
    public partial class VendorDetailViewModel : ObservableDetailViewModel<Vendor>
    {
        #region Fields

        /// <summary>
        /// Defines the dbContext.
        /// </summary>
        private readonly LocalDbContext dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorDetailViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public VendorDetailViewModel(LocalDbContext dbContext)
        {
            this.Title = AppResources.VendorDetail;
            this.dbContext = dbContext;
            _ = this.LoadCountriesAsync();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Vendor description.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MaxLength(250, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.Description), ResourceType = typeof(AppResources))]
        public partial string? VendorDescription
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the VendorId.
        /// </summary>
        [ObservableProperty]
        public partial string? VendorId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Vendor name.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = nameof(ValidationResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(1, ErrorMessageResourceName = nameof(ValidationResources.MinLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.Name), ResourceType = typeof(AppResources))]
        public partial string VendorName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Street.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        [MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.Street), ResourceType = typeof(AppResources))]
        public partial string? Street { get; set; }

        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        [MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.City), ResourceType = typeof(AppResources))]
        public partial string? City { get; set; }

        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        [MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.State), ResourceType = typeof(AppResources))]
        public partial string? State { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        [MaxLength(15, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.PostalCode), ResourceType = typeof(AppResources))]
        public partial string? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the SelectedCountry.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [Display(Name = nameof(AppResources.Country), ResourceType = typeof(AppResources))]
        public partial Country? SelectedCountry { get; set; }

        /// <summary>
        /// Gets the Countries.
        /// </summary>
        public ObservableCollection<Country> Countries { get; } = new();

        /// <summary>
        /// Gets the expected query parameter name for loading an existing vendor.
        /// </summary>
        protected override string QueryIdParameterName => nameof(this.VendorId);

        #endregion

        #region Methods

        /// <summary>
        /// Copies the model values into the VM properties.
        /// </summary>
        /// <param name="item">The item<see cref="Vendor"/>.</param>
        public override void LoadFromItem(Vendor item)
        {
            ArgumentNullException.ThrowIfNull(item);

            this.VendorId = item.Id;
            this.VendorName = item.Name;
            this.VendorDescription = item.Description;
            this.Street = item.Street;
            this.City = item.City;
            this.State = item.State;
            this.PostalCode = item.PostalCode;
            this.SelectedCountry = this.Countries.FirstOrDefault(c => c.Id == item.CountryId);
        }

        /// <summary>
        /// Saves the vendor and updates the database context.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public override async Task SaveCommandImplementation()
        {
            this.ValidateAllProperties();

            if (!this.CanGoBack)
            {
                return;
            }

            if (this.Item is null)
            {
                throw new InvalidOperationException("No vendor is loaded.");
            }

            bool exists = await this.dbContext.Vendors.AnyAsync(v => v.Id == this.Item.Id);

            if (!exists)
            {
                this.ApplyChangesToItem();
                this.dbContext.Vendors.Add(this.Item);
            }
            else
            {
                Vendor vendor = await this.dbContext.Vendors.FindAsync(this.Item.Id)
                    ?? throw new InvalidOperationException($"Vendor '{this.Item.Id}' was not found.");

                if (!ReferenceEquals(this.Item, vendor))
                {
                    this.Item = vendor;
                }

                this.ApplyChangesToItem();
            }

            bool isNew = !exists;
            await this.dbContext.SaveChangesAsync();

            // Broadcast the generic change message to update listing screens dynamically
            WeakReferenceMessenger.Default.Send(
                new EntityChangedMessage<Vendor>(
                    this.Item,
                    isNew ? EntityChangeAction.Created : EntityChangeAction.Updated));

            await Shell.Current.GoToAsync("..?cancel=true");
        }

        /// <summary>
        /// Copies the VM properties back into the model.
        /// </summary>
        /// <param name="item">The item<see cref="Vendor"/>.</param>
        public override void SaveToItem(Vendor item)
        {
            ArgumentNullException.ThrowIfNull(item);

            item.Name = this.VendorName;
            item.Description = this.VendorDescription;
            item.Street = this.Street;
            item.City = this.City;
            item.State = this.State;
            item.PostalCode = this.PostalCode;
            item.CountryId = this.SelectedCountry?.Id;
        }

        /// <summary>
        /// Creates a new vendor entity.
        /// </summary>
        /// <returns>The <see cref="Vendor"/>.</returns>
        protected override Vendor CreateNewItem()
        {
            return new Vendor();
        }

        /// <summary>
        /// Deletes the vendor.
        /// </summary>
        /// <param name="item">The item<see cref="Vendor"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task DeleteItemAsync(Vendor item)
        {
            if (this.dbContext.Entry(item).State == EntityState.Detached)
            {
                this.dbContext.Vendors.Attach(item);
            }

            this.dbContext.Vendors.Remove(item);
            await this.dbContext.SaveChangesAsync();

            // Broadcast delete event
            WeakReferenceMessenger.Default.Send(
                new EntityChangedMessage<Vendor>(item, EntityChangeAction.Deleted));
        }

        /// <summary>
        /// Loads the vendor by ID from EF Core.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Vendor?}"/>.</returns>
        protected override async Task<Vendor?> LoadItemByIdAsync(string id)
        {
            return await this.dbContext.Vendors.FindAsync(id);
        }

        /// <summary>
        /// Callback when the active vendor changes.
        /// </summary>
        /// <param name="item">The item<see cref="Vendor?"/>.</param>
        protected override void OnItemChanged(Vendor? item)
        {
            base.OnItemChanged(item);
            this.VendorId = item?.Id;

            if (item is null)
            {
                this.VendorId = null;
                this.VendorName = string.Empty;
                this.VendorDescription = string.Empty;
                this.Street = string.Empty;
                this.City = string.Empty;
                this.State = string.Empty;
                this.PostalCode = string.Empty;
                this.SelectedCountry = null;
                return;
            }

            this.Street = item.Street;
            this.City = item.City;
            this.State = item.State;
            this.PostalCode = item.PostalCode;
            this.SelectedCountry = this.Countries.FirstOrDefault(c => c.Id == item.CountryId);
        }

        private async Task LoadCountriesAsync()
        {
            try
            {
                var list = await this.dbContext.Countries.OrderBy(c => c.Name).ToListAsync();
                foreach (var country in list)
                {
                    this.Countries.Add(country);
                }

                if (this.Item is not null)
                {
                    this.SelectedCountry = this.Countries.FirstOrDefault(c => c.Id == this.Item.CountryId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load countries: {ex}");
            }
        }

        #endregion
    }
}
