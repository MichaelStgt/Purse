# Task: Vendor Seeding and Overlay Componentization

## Phase 1: German Vendor Seed Data
*   Ensure the provided `german_vendors.csv` file is placed in the `Purse.Shared\Resources\Raw` folder and that its Build Action is set to `MauiAsset`.

## Phase 2: Database Initialization
Update `Purse.Data\Services\DbContextInitializer.cs`.
*   Check if the `Vendors` table is empty.
*   If empty, read the `german_vendors.csv` file using standard stream reading (account for headers).
*   Parse the CSV rows into `Vendor` entities and save them to the database.

## Phase 3: Componentize the Overlays (`Purse\View\Components\`)
Create two new `ContentView` files: `CategorySelectionView.xaml` and `VendorSelectionView.xaml`.

**1. CategorySelectionView.xaml:**
*   Extract the existing `SfComboBox` and overlay layout for Categories from `TransactionDetailView.xaml`.
*   In the `DropdownFooterView`, you must have **three** buttons:
    1.  Create Expense Category (Preserve existing command)
    2.  Create Income Category (Preserve existing command)
    3.  Edit Selected Category (New command, only enabled if a category is currently selected)

**2. VendorSelectionView.xaml:**
*   Create a similar layout binding to an `ObservableCollection<Vendor>` fetched from the database (not a hardcoded string list).
*   In the `DropdownFooterView`, implement **two** buttons:
    1.  Create New Vendor
    2.  Edit Selected Vendor (Only enabled if a vendor is currently selected)

*Note: Ensure all new button text is localized in `AppResources.resx`.*

## Phase 4: ViewModel Integration
Update `TransactionDetailViewModel.cs` and `TransactionDetailView.xaml`.
*   Replace the hardcoded string list of vendors with the database-backed `ObservableCollection<Vendor>`.
*   Replace the inline XAML in `TransactionDetailView.xaml` with instances of `<components:CategorySelectionView />` and `<components:VendorSelectionView />`.
*   Bind their visibility to modern `partial` properties (e.g., `IsCategoryOverlayVisible`, `IsVendorOverlayVisible`).