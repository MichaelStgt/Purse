# Task: Refactor Selection UI to SfComboBox

## Objective
Abandon the custom overlay approach for Vendor and Category selection. Restore the use of Syncfusion's `SfComboBox` for selection by moving it from the dormant components back into the main view. Quarantine all overlay-specific properties and commands into a partial class.

## Execution Steps

### 1. Restore Vendor SfComboBox (`TransactionDetailView.xaml`)
*   Open `Purse\View\Components\VendorSelectionView.xaml` and copy the `syncfusion:SfComboBox` declaration.
*   Open `Purse\View\Transactions\TransactionDetailView.xaml`.
*   Completely remove the custom overlay component tags at the bottom of the root grid (`<components:CategorySelectionView ... />` and `<components:VendorSelectionView ... />`).
*   In the Vendor selection area at the top of the form, remove the custom `Border` and `TapGestureRecognizer` trigger.
*   Paste the copied `SfComboBox` in its place.
*   **Crucial Binding Updates:** Change the bindings on the pasted `SfComboBox` so it interacts with the actual transaction data, not the overlay state:
    *   Change `SelectedItem="{Binding OverlaySelectedVendor}"` to `SelectedItem="{Binding SelectedVendor, Mode=TwoWay}"`
    *   Change `Text="{Binding OverlayVendorSearchText}"` to `Text="{Binding VendorSearchText, Mode=TwoWay}"` (if applicable).
    *   Remove any footer templates or Add/Edit buttons from this combobox.

### 2. Preserve Component Files
*   Leave `CategorySelectionView.xaml`/`.cs` and `VendorSelectionView.xaml`/`.cs` in the `Purse\View\Components\` folder as dormant reference files. Do not delete them.

### 3. Quarantine ViewModel Overlay Logic
*   Create a new file: `Purse\ViewModel\Transactions\TransactionDetailViewModel.Selection.cs`.
*   Ensure it is declared as `public partial class TransactionDetailViewModel`.
*   Move ALL overlay-related state, properties, and commands from `TransactionDetailViewModel.cs` into this new file. You must explicitly move:
    *   `IsCategoryOverlayVisible`
    *   `IsVendorOverlayVisible`
    *   `OverlaySelectedVendor`
    *   `OverlayVendorSearchText`
    *   `OverlaySelectedCategory` (and its search text equivalent, if present)
    *   `_activeSplitForCategory`
    *   `OpenVendorOverlayCommand`
    *   `OpenCategoryOverlayCommand`
    *   Any `Save`, `Cancel`, or `Selected` commands specific to the overlays.
*   Ensure `TransactionDetailViewModel.cs` is now completely stripped of these temporary properties.

## Expected Outcome
The `TransactionDetailView` uses a standard `SfComboBox` to select a Vendor, bound directly to `SelectedVendor`. The main ViewModel is clean, with all `Overlay...` properties and commands safely isolated in `TransactionDetailViewModel.Selection.cs`.