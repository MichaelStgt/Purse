Task

Currently, the User would need to navigate to the Vendor, IncomeSource first to add custom entries, before he would be able to 
add a Transaction using these custom entries. This would prevent easy usage of the Transaction functionlity.

1. Vendor Selection
Instead of using a picker, we will use a Label. Once the user clicks or taps the label, we open a CMT Popup.
The Popup has a search field at top and list of all the current defined Vendors.
If the Search field is empty, we show all current Vendors in a CollectionView.
If he enters any character, we filter to list to his entry
If the List is empty after he enters his list, we provide the user with a button defining a new Vendor and Navigate him to the VendorDetailView.
Once he saves the new creted Vendor we navigate back to the TransactionDetailView and set the selected Vendor to that value. If he cancels to create a new Vendor, we'll also navigate back to the TransactionDetailView.
The List of Vendords in the Popup would not only contain the name of Vendor, but also a button to edit an existing Vendor. Once he clicks on that button, we'll Navigate to the VendorDetailView and let the user edit the existing vendor.
No matter if he saves his change or cancels to update the Vendor, well take that one as the SelectedVendor.
At the bottom of the Popup, we provide a button to close the Popup and return to the TransactionDetailView without selecting a Vendor.
Well have a second button to add a new Vendor, which will navigate to the VendorDetailView. Once he saves the new creted Vendor we navigate back to the TransactionDetailView and set the selected Vendor to that value. If he cancels to create a new Vendor, we'll also navigate back to the TransactionDetailView.
We'll use the same logic for the IncomeSource Selection, but with the IncomeSourceDetailView instead of the VendorDetailView.

2. Catetegory Selection
We'll add the same logic for the Category Selection, but with the CategoryDetailView instead of the VendorDetailView.
The user will be able to select a Category from a list, and if the desired Category does not exist, they can create a new one directly from the selection popup. The newly created Category will then be set as the selected Category in the TransactionDetailView.
As the current Version does work perfectly using the Syncfusion ComboBox, we will comment that part in the xaml code to switch back to the current version, if any thing goes wrong.

3. UI Changes
For now, we'll implement this in the current project. But as we already have two places where we need to select a Vendor, IncomeSource  or Category, we should consider creating a reusable component for the selection popup. This will help maintain consistency and reduce code duplication in the future.
This component can be designed to accept parameters for the type of entity being selected (Vendor, IncomeSource, or Category) and handle the common logic for displaying the list, filtering, and navigating to the detail views for adding or editing entries. The new component should go into the LibraryTen Project as this 
is a reusable component. The component should be designed to be flexible and easily integrated into different parts of the application where selection of these entities is required.
 