# Complete Address Fields in Vendor Detail View and ViewModel

This plan details the implementation to support full address details (Street, City, State, Postal Code, and Country selection) in the Vendor Detail screen, matching the address support already present for Income Sources.

## Proposed Changes

### Purse Project

---

#### [MODIFY] [VendorDetailViewModel.cs](file:///c:/Repos/040%20MauiNet10/Purse/040%20Projects/Purse/ViewModel/Vendors/VendorDetailViewModel.cs)
- Add new properties with validation and display attributes (matching `ValidationResources` and `AppResources` formats):
  - `Street` (string?, MaxLength: 100)
  - `City` (string?, MaxLength: 100)
  - `State` (string?, MaxLength: 100)
  - `PostalCode` (string?, MaxLength: 15)
  - `SelectedCountry` (`Country?`)
  - `Countries` (`ObservableCollection<Country>`)
- Load country list from database in the constructor:
  - Add `private async Task LoadCountriesAsync()` to query all countries ordered by Name from the `dbContext.Countries`.
  - Populate the `Countries` list.
- Update model-to-VM loading (`LoadFromItem`):
  - Populate Street, City, State, and PostalCode properties.
  - Set `SelectedCountry` to the matching country from the `Countries` collection based on the vendor's `CountryId`.
- Update VM-to-model mapping (`SaveToItem`):
  - Save Street, City, State, and PostalCode back to the `Vendor` entity.
  - Save `SelectedCountry?.Id` to the vendor's `CountryId` property.
- Update `OnItemChanged` callback to clear all address fields when `item` is null, or populate `SelectedCountry` when `item` is loaded.

#### [MODIFY] [VendorDetailView.xaml](file:///c:/Repos/040%20MauiNet10/Purse/040%20Projects/Purse/View/Vendors/VendorDetailView.xaml)
- Change Grid `RowDefinitions` from `Auto, Auto, Auto` to `Auto, Auto, Auto, Auto, Auto, Auto, Auto, Auto` to accommodate the 5 new fields and the Save Button.
- Add address inputs inside `LabeledContentView` controls, using localized placeholders:
  - **Street**: `Placeholder="{x:Static AppResources.EnterStreetAddress}"` (Row 2)
  - **City**: `Placeholder="{x:Static AppResources.EnterCity}"` (Row 3)
  - **State**: `Placeholder="{x:Static AppResources.EnterStateOrProvince}"` (Row 4)
  - **Postal Code**: `Placeholder="{x:Static AppResources.EnterPostalCode}"` (Row 5)
  - **Country**: `<Picker ItemsSource="{x:Binding Countries}" SelectedItem="{x:Binding SelectedCountry}" ItemDisplayBinding="{Binding Name}" />` (Row 6)
- Adjust the Save Button `Grid.Row` to `7`.

---

## Verification Plan

### Automated Tests
- Build verification using `dotnet build -f net10.0-windows10.0.19041.0`.

### Manual Verification
- Navigate to Add New Vendor screen.
- Verify Street, City, State, Postal Code, and Country dropdown are present.
- Enter address fields, select a country, and save.
- Reload the newly created vendor and verify all address fields are correctly populated and preserved.
