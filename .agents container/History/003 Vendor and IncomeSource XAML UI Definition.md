TASK:
We need to build the UI for the Vendors and Income Sources.

Please generate the XAML and code-behind for the following four Views, and explicitly tell me where they are located in the project structure:

1. `VendorsCollectionView.xaml`: A page bound to `VendorsCollectionViewModel`. It should contain a `CollectionView` to list vendors and a "Add New Vendor" Button.
As this will be our third "CollectionPage", we should define Styles for the elements in this View, as example the Border around an item in the CollectionViews DataTemplate.
This will make sure, we have the same look and feel on all pages that display a CollectionView.
2. `VendorDetailView.xaml`: A page bound to `VendorDetailViewModel`. It should contain `Entry` fields for the Vendor's Name and Description, and a "Save" button.
3. `IncomeSourcesCollectionView.xaml`: A page bound to `IncomeSourcesCollectionViewModel` with a `CollectionView` for income sources and an "Add New Income Source" button.
4. `IncomeSourceDetailView.xaml`: A page bound to `IncomeSourceDetailViewModel` with `Entry` fields for Name and Description, and the address fields (`Street`, `City`, `State`, `PostalCode`, `Country`) and a "Save" button.