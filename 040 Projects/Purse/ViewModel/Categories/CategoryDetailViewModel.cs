// <copyright file="CategoryDetailViewModel.cs" company="Behr, Michael">
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
    using Purse.Shared.Model;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The category detail view model.
    /// </summary>
    public partial class CategoryDetailViewModel : ObservableDetailViewModel<Category>
    {
        private readonly LocalDbContext dbContext;

        #region Properties

        /// <summary>
        /// Gets or sets the CategoryId.
        /// </summary>
        [ObservableProperty]
        public partial string? CategoryId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Display(Name = "CategoryName", ResourceType = typeof(AppResources))]
        [Required(ErrorMessage = "Category Name is required.")]
        [MinLength(1, ErrorMessage = "Category Name cannot be empty.")]
        [MaxLength(100, ErrorMessage = "Category Name cannot exceed 100 characters.")]
        public partial string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this is an income category.
        /// </summary>
        [ObservableProperty]
        [Display(Name = "IsIncome", ResourceType = typeof(AppResources))]
        public partial bool IsIncome
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is a pre-defined system category.
        /// </summary>
        [ObservableProperty]
        public partial bool IsSystem
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is a default category.
        /// </summary>
        [ObservableProperty]
        [Display(Name = "IsDefault", ResourceType = typeof(AppResources))]
        public partial bool IsDefault
        {
            get; set;
        }

        /// <summary>
        /// Gets a value indicating whether this category is editable (system categories are read-only).
        /// </summary>
        [ObservableProperty]
        // [Display(Name = "IsEditable", ResourceType = typeof(AppResources))]
        public partial bool IsEditable { get; set; } = true;

        /// <summary>
        /// Gets the expected query parameter name for loading an existing category.
        /// </summary>
        protected override string QueryIdParameterName => nameof(this.CategoryId);

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryDetailViewModel"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CategoryDetailViewModel(LocalDbContext dbContext)
        {
            this.Title = Purse.Resources.Strings.AppResources.CategoryDetail;
            this.dbContext = dbContext;
        }

        #region Methods

        /// <summary>
        /// Copies the model values into the VM properties.
        /// </summary>
        public override void LoadFromItem(Category item)
        {
            ArgumentNullException.ThrowIfNull(item);

            this.CategoryId = item.Id;
            this.CategoryName = item.DisplayName; // Display translated name
            this.IsIncome = item.IsIncome;
            this.IsSystem = item.IsSystem;
            this.IsDefault = item.IsDefault;

            // System categories are read-only (user cannot rename or delete them)
            this.IsEditable = !item.IsSystem;
        }

        /// <summary>
        /// Copies the VM properties back into the model.
        /// </summary>
        public override void SaveToItem(Category item)
        {
            ArgumentNullException.ThrowIfNull(item);

            // User can only modify custom categories
            if (!item.IsSystem)
            {
                item.Name = this.CategoryName;
                item.IsIncome = this.IsIncome;
            }
            item.IsDefault = this.IsDefault;
        }

        /// <summary>
        /// Saves the category and updates the database context.
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
                throw new InvalidOperationException("No category is loaded.");
            }

            bool exists = await this.dbContext.Categories.AnyAsync(c => c.Id == this.Item.Id);

            // If we mark this category as default, reset all other categories of the same type (Income vs Expense)
            if (this.IsDefault)
            {
                var defaultsToReset = await this.dbContext.Categories
                    .Where(c => c.IsIncome == this.IsIncome && c.Id != this.Item.Id && c.IsDefault)
                    .ToListAsync();

                foreach (var other in defaultsToReset)
                {
                    other.IsDefault = false;
                }
            }

            if (!exists)
            {
                this.ApplyChangesToItem();
                this.dbContext.Categories.Add(this.Item);
            }
            else
            {
                Category category = await this.dbContext.Categories.FindAsync(this.Item.Id)
                    ?? throw new InvalidOperationException($"Category '{this.Item.Id}' was not found.");

                if (!ReferenceEquals(this.Item, category))
                {
                    this.Item = category;
                }

                this.ApplyChangesToItem();
            }

            bool isNew = !exists;
            await this.dbContext.SaveChangesAsync();

            // Broadcast the generic change message to update listing screens dynamically
            WeakReferenceMessenger.Default.Send(
                new EntityChangedMessage<Category>(
                    this.Item,
                    isNew ? EntityChangeAction.Created : EntityChangeAction.Updated));

            await Shell.Current.GoToAsync("..?cancel=true");
        }

        /// <summary>
        /// Shows the delete confirmation prompt.
        /// </summary>
        protected override async Task<bool> ConfirmDeleteAsync()
        {
            if (Shell.Current is null)
            {
                return false;
            }

            if (this.IsSystem)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Action Denied",
                    "System categories cannot be deleted.",
                    "OK");
                return false;
            }

            return await Shell.Current.DisplayAlertAsync(
                "Delete Category",
                $"Do you want to delete the category '{this.CategoryName}'?",
                "Delete",
                "Cancel");
        }

        /// <summary>
        /// Deletes the category.
        /// </summary>
        protected override async Task DeleteItemAsync(Category item)
        {
            if (item.IsSystem)
            {
                return;
            }

            if (this.dbContext.Entry(item).State == EntityState.Detached)
            {
                this.dbContext.Categories.Attach(item);
            }

            this.dbContext.Categories.Remove(item);
            await this.dbContext.SaveChangesAsync();

            // Broadcast delete event
            WeakReferenceMessenger.Default.Send(
                new EntityChangedMessage<Category>(item, EntityChangeAction.Deleted));
        }

        /// <summary>
        /// Creates a new category entity.
        /// </summary>
        protected override Category CreateNewItem()
        {
            return new Category
            {
                IsIncome = false,
                IsSystem = false,
                IsDefault = false
            };
        }

        /// <summary>
        /// Loads the category by ID from EF Core.
        /// </summary>
        protected override async Task<Category?> LoadItemByIdAsync(string id)
        {
            return await this.dbContext.Categories.FindAsync(id);
        }

        /// <summary>
        /// Callback when the active category changes.
        /// </summary>
        protected override void OnItemChanged(Category? item)
        {
            base.OnItemChanged(item);
            this.CategoryId = item?.Id;

            if (item is null)
            {
                this.CategoryId = null;
                this.CategoryName = string.Empty;
                this.IsIncome = false;
                this.IsSystem = false;
                this.IsDefault = false;
                this.IsEditable = true;
                return;
            }
        }

        #endregion
    }
}
