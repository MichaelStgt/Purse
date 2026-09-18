import re

with open("040 Projects/Purse/ViewModel/Transactions/TransactionDetailViewModel.cs", "r", encoding="utf-8") as f:
    content = f.read()

# Replace Vendors List<string> with ObservableCollection<Vendor>
content = re.sub(
    r"public partial List<string> Vendors\s*\{\s*get;\s*set;\s*\}",
    r"public partial ObservableCollection<Vendor> Vendors { get; set; }",
    content
)

# Update constructor to use ObservableCollection<Vendor>
content = re.sub(
    r'this\.Vendors = new\(\) \{ "Aldi".*?\};',
    r"this.Vendors = new ObservableCollection<Vendor>();",
    content
)

# Inject Overlay properties
overlay_props = """
        [ObservableProperty] public partial bool IsCategoryOverlayVisible { get; set; }
        [ObservableProperty] public partial bool IsVendorOverlayVisible { get; set; }
        [ObservableProperty] public partial bool IsCategoryDropdownOpen { get; set; }
        [ObservableProperty] public partial bool IsVendorDropdownOpen { get; set; }
        [ObservableProperty] public partial Category? OverlaySelectedCategory { get; set; }
        [ObservableProperty] public partial string? OverlaySearchText { get; set; }
        [ObservableProperty] public partial Vendor? OverlaySelectedVendor { get; set; }
        [ObservableProperty] public partial string? OverlayVendorSearchText { get; set; }
        [ObservableProperty] public partial bool IsNewCategoryPromptVisible { get; set; }
        
        private TransactionLineItem? currentEditingSplit;
"""

content = re.sub(
    r"(public partial string\? SelectedVendor\s*\{\s*get;\s*set;\s*\})",
    r"\1\n" + overlay_props,
    content
)

# Load Vendors from DB
load_vendors_code = """
                var dbVendors = await this.dbContext.Vendors.ToListAsync();
                this.Vendors.Clear();
                foreach (var vendor in dbVendors.OrderBy(v => v.Name))
                {
                    this.Vendors.Add(vendor);
                }
                if (string.IsNullOrWhiteSpace(this.SelectedVendor))
                {
                    this.SelectedVendor = this.Vendors.FirstOrDefault()?.Name;
                }
"""

content = re.sub(
    r"(this\.Categories = new\(\) \{.*?;\n\s+this\.defaultCategoryName = this\.Categories\.FirstOrDefault\(\);\n\s+\})",
    r"\1" + load_vendors_code,
    content
)
content = re.sub(
    r"(this\.defaultCategoryName = defaultCat\.DisplayName;\n\s+\}\n\s+\})",
    r"\1" + load_vendors_code,
    content
)

# Remove old LoadCategoriesAsync fallback for Vendors
content = re.sub(r'this\.SelectedVendor = this\.Vendors\.FirstOrDefault\(\);', r'', content)

# Inject Commands
commands_code = """
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
            this.IsNewCategoryPromptVisible = false;
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
            string categoryName = this.OverlaySelectedCategory?.DisplayName ?? this.OverlaySelectedCategory?.Name ?? this.OverlaySearchText ?? string.Empty;
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
            this.SelectedVendor = vendorName;
            this.IsVendorOverlayVisible = false;
        }
        
        [RelayCommand]
        private void OpenVendorOverlay()
        {
            this.OverlayVendorSearchText = this.SelectedVendor;
            this.OverlaySelectedVendor = this.Vendors.FirstOrDefault(v => string.Equals(v.Name, this.SelectedVendor, StringComparison.OrdinalIgnoreCase));
            this.IsVendorOverlayVisible = true;
        }

        [RelayCommand]
        private async Task CreateExpenseCategoryAsync()
        {
            string name = this.OverlaySearchText?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name)) return;
            var cat = new Category { Name = name, DisplayName = name, IsIncome = false, IsSystem = false };
            this.dbContext.Categories.Add(cat);
            await this.dbContext.SaveChangesAsync();
            this.AvailableCategories.Add(cat);
            this.OverlaySelectedCategory = cat;
        }

        [RelayCommand]
        private async Task CreateIncomeCategoryAsync()
        {
            string name = this.OverlaySearchText?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name)) return;
            var cat = new Category { Name = name, DisplayName = name, IsIncome = true, IsSystem = false };
            this.dbContext.Categories.Add(cat);
            await this.dbContext.SaveChangesAsync();
            this.AvailableCategories.Add(cat);
            this.OverlaySelectedCategory = cat;
        }

        [RelayCommand]
        private async Task CreateVendorAsync()
        {
            string name = this.OverlayVendorSearchText?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name)) return;
            var v = new Vendor { Name = name };
            this.dbContext.Vendors.Add(v);
            await this.dbContext.SaveChangesAsync();
            this.Vendors.Add(v);
            this.OverlaySelectedVendor = v;
        }

        [RelayCommand]
        private async Task EditSelectedCategoryAsync()
        {
            if (this.OverlaySelectedCategory != null)
            {
                await Shell.Current.GoToAsync($"///CategoryDetail?CategoryId={this.OverlaySelectedCategory.Id}");
            }
        }

        [RelayCommand]
        private async Task EditSelectedVendorAsync()
        {
            if (this.OverlaySelectedVendor != null)
            {
                await Shell.Current.GoToAsync($"///VendorDetail?VendorId={this.OverlaySelectedVendor.Id}");
            }
        }

        partial void OnOverlaySelectedCategoryChanged(Category? value)
        {
            if (value != null) this.IsNewCategoryPromptVisible = false;
        }
"""

content = re.sub(r'(\}\n)$', commands_code + r'\n\1', content)

# Make fields public partial ObservableProperty
content = re.sub(
    r"\[ObservableProperty\]\s*private\s+(.+?)\s+(\w+);",
    lambda m: f"[ObservableProperty] public partial {m.group(1)} {m.group(2).capitalize()} {{ get; set; }}",
    content
)

with open("040 Projects/Purse/ViewModel/Transactions/TransactionDetailViewModel.cs", "w", encoding="utf-8") as f:
    f.write(content)

print("Done")
