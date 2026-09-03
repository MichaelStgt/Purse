// <copyright file="IncomeSourceDetailViewModel.cs" company="Behr, Michael">
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
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The income source detail view model.
    /// </summary>
    public partial class IncomeSourceDetailViewModel : ObservableDetailViewModel<IncomeSource>
    {
        private readonly LocalDbContext dbContext;

        #region Properties

        /// <summary>
        /// Gets or sets the IncomeSourceId.
        /// </summary>
        [ObservableProperty]
        public partial string? IncomeSourceId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the IncomeSource name.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessageResourceName = nameof(ValidationResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(1, ErrorMessageResourceName = nameof(ValidationResources.MinLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.Name), ResourceType = typeof(AppResources))]
        public partial string IncomeSourceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MaxLength(250, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.Description), ResourceType = typeof(AppResources))]
        public partial string? IncomeSourceDescription
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.Street), ResourceType = typeof(AppResources))]
        public partial string? Street
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.City), ResourceType = typeof(AppResources))]
        public partial string? City
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.State), ResourceType = typeof(AppResources))]
        public partial string? State
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MaxLength(20, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = nameof(AppResources.PostalCode), ResourceType = typeof(AppResources))]
        public partial string? PostalCode
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the selected country.
        /// </summary>
        [ObservableProperty]
        [Display(Name = nameof(AppResources.Country), ResourceType = typeof(AppResources))]
        public partial Country? SelectedCountry
        {
            get; set;
        }

        /// <summary>
        /// Gets the list of available countries.
        /// </summary>
        public ObservableCollection<Country> Countries { get; } = new();

        /// <summary>
        /// Gets the expected query parameter name for loading an existing income source.
        /// </summary>
        protected override string QueryIdParameterName => nameof(this.IncomeSourceId);

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeSourceDetailViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public IncomeSourceDetailViewModel(LocalDbContext dbContext)
        {
            this.Title = AppResources.IncomeSourceDetail;
            this.dbContext = dbContext;
            _ = this.LoadCountriesAsync();
        }

        #region Methods

        private async Task LoadCountriesAsync()
        {
            try
            {
                var list = await this.dbContext.Countries.OrderBy(c => c.Name).ToListAsync();
                foreach (var country in list)
                {
                    this.Countries.Add(country);
                }

                // Sync selection if the item was already loaded
                if (this.Item is not null && !string.IsNullOrEmpty(this.Item.CountryId))
                {
                    this.SelectedCountry = this.Countries.FirstOrDefault(c => c.Id == this.Item.CountryId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load countries: {ex}");
            }
        }

        /// <summary>
        /// Copies the model values into the VM properties.
        /// </summary>
        public override void LoadFromItem(IncomeSource item)
        {
            ArgumentNullException.ThrowIfNull(item);

            this.IncomeSourceId = item.Id;
            this.IncomeSourceName = item.Name;
            this.IncomeSourceDescription = item.Description;
            this.Street = item.Street;
            this.City = item.City;
            this.State = item.State;
            this.PostalCode = item.PostalCode;
            this.SelectedCountry = this.Countries.FirstOrDefault(c => c.Id == item.CountryId);
        }

        /// <summary>
        /// Copies the VM properties back into the model.
        /// </summary>
        public override void SaveToItem(IncomeSource item)
        {
            ArgumentNullException.ThrowIfNull(item);

            item.Name = this.IncomeSourceName;
            item.Description = this.IncomeSourceDescription;
            item.Street = this.Street;
            item.City = this.City;
            item.State = this.State;
            item.PostalCode = this.PostalCode;
            item.CountryId = this.SelectedCountry?.Id;
        }

        /// <summary>
        /// Saves the income source and updates the database context.
        /// </summary>
        public override async Task SaveCommandImplementation()
        {
            this.ValidateAllProperties();

            if (!this.CanGoBack)
            {
                return;
            }

            if (this.Item is null)
            {
                throw new InvalidOperationException("No income source is loaded.");
            }

            bool exists = await this.dbContext.IncomeSources.AnyAsync(isrc => isrc.Id == this.Item.Id);

            if (!exists)
            {
                this.ApplyChangesToItem();
                this.dbContext.IncomeSources.Add(this.Item);
            }
            else
            {
                IncomeSource incomeSource = await this.dbContext.IncomeSources.FindAsync(this.Item.Id)
                    ?? throw new InvalidOperationException($"IncomeSource '{this.Item.Id}' was not found.");

                if (!ReferenceEquals(this.Item, incomeSource))
                {
                    this.Item = incomeSource;
                }

                this.ApplyChangesToItem();
            }

            bool isNew = !exists;
            await this.dbContext.SaveChangesAsync();

            // Broadcast the generic change message to update listing screens dynamically
            WeakReferenceMessenger.Default.Send(
                new EntityChangedMessage<IncomeSource>(
                    this.Item,
                    isNew ? EntityChangeAction.Created : EntityChangeAction.Updated));

            await Shell.Current.GoToAsync("..?cancel=true");
        }

        /// <summary>
        /// Deletes the income source.
        /// </summary>
        protected override async Task DeleteItemAsync(IncomeSource item)
        {
            if (this.dbContext.Entry(item).State == EntityState.Detached)
            {
                this.dbContext.IncomeSources.Attach(item);
            }

            this.dbContext.IncomeSources.Remove(item);
            await this.dbContext.SaveChangesAsync();

            // Broadcast delete event
            WeakReferenceMessenger.Default.Send(
                new EntityChangedMessage<IncomeSource>(item, EntityChangeAction.Deleted));
        }

        /// <summary>
        /// Creates a new income source entity.
        /// </summary>
        protected override IncomeSource CreateNewItem()
        {
            return new IncomeSource();
        }

        /// <summary>
        /// Loads the income source by ID from EF Core.
        /// </summary>
        protected override async Task<IncomeSource?> LoadItemByIdAsync(string id)
        {
            return await this.dbContext.IncomeSources.FindAsync(id);
        }

        /// <summary>
        /// Callback when the active income source changes.
        /// </summary>
        protected override void OnItemChanged(IncomeSource? item)
        {
            base.OnItemChanged(item);
            this.IncomeSourceId = item?.Id;

            if (item is null)
            {
                this.IncomeSourceId = null;
                this.IncomeSourceName = string.Empty;
                this.IncomeSourceDescription = string.Empty;
                this.Street = string.Empty;
                this.City = string.Empty;
                this.State = string.Empty;
                this.PostalCode = string.Empty;
                this.SelectedCountry = null;
                return;
            }
        }

        #endregion
    }
}
