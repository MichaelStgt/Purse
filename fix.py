import re

with open(r'040 Projects\Purse\ViewModel\Transactions\TransactionDetailViewModel.cs', 'r', encoding='utf-8') as f:
    content = f.read()

# Fix Vendors collection type
content = re.sub(r'public partial ObservableCollection<string> Vendors', r'public partial ObservableCollection<Vendor> Vendors', content)
content = re.sub(r'public partial List<string> Vendors', r'public partial ObservableCollection<Vendor> Vendors', content)
content = re.sub(r'this.Vendors = new ObservableCollection<string>\(\);', r'this.Vendors = new ObservableCollection<Vendor>();', content)
content = re.sub(r'this.Vendors = new List<string>\(\);', r'this.Vendors = new ObservableCollection<Vendor>();', content)

# Fix Name error on Vendors list
content = re.sub(r'this.SelectedVendor = this.Vendors.FirstOrDefault\(\)\?.Name;', r'this.SelectedVendor = this.Vendors.FirstOrDefault()?.Name;', content)
# Ensure it hasn't been written incorrectly like string.Name
content = re.sub(r'this.Vendors.Add\(vendor.Name\);', r'this.Vendors.Add(vendor);', content)

# Inject currentEditingSplit if missing
if 'private TransactionLineItem? currentEditingSplit;' not in content:
    content = re.sub(r'\[ObservableProperty\]\s*public partial bool IsCategoryOverlayVisible', 
        r'private TransactionLineItem? currentEditingSplit;\n        [ObservableProperty] public partial bool IsCategoryOverlayVisible', content)

with open(r'040 Projects\Purse\ViewModel\Transactions\TransactionDetailViewModel.cs', 'w', encoding='utf-8') as f:
    f.write(content)
