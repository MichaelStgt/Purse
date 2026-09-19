# Task: Implement Object-Based Vendor Selection

## Objective
The `SelectedVendor` must be treated as a strong `Vendor` object, not a string. Update the ViewModel property to ensure it uses object-level validation, and wire the `SfComboBox` to bind directly to this object.

## Execution Steps

### 1. Fix SelectedVendor Property (`TransactionDetailViewModel.cs`)
*   Locate the `SelectedVendor` property.
*   Change its type from `string` to `Vendor` (if it isn't already).
*   **Delete** any string-specific validation attributes like `[MinLength(1)]` or `[MaxLength(100)]`.
*   Ensure it has the `[Required]` attribute to catch `null` selections, alongside the notification triggers:
    ```csharp
    [Required(ErrorMessageResourceName = nameof(ValidationResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [ObservableProperty]
    public partial Vendor SelectedVendor { get; set; }
    ```

### 2. Clean TotalAmount Attributes (`TransactionDetailViewModel.cs`)
*   Locate the `TotalAmount` property.
*   **Delete** the obsolete `[NotifyPropertyChangedFor(nameof(PlannedAmount))]` attribute.
*   Leave all other attributes (`[Range]`, `[NotifyDataErrorInfo]`, `[NotifyCanExecuteChangedFor]`) completely intact.

### 3. Wire the SfComboBox (`TransactionDetailView.xaml`)
*   Locate the `<syncfusion:SfComboBox>` used for the Vendor.
*   Ensure the bindings strictly use `SelectedItem` to pass the `Vendor` object, while using `DisplayMemberPath` and `TextMemberPath` to show the `Name` string to the user.
    ```xml
    <syncfusion:SfComboBox 
        ItemsSource="{Binding Vendors}"
        SelectedItem="{Binding SelectedVendor, Mode=TwoWay}"
        DisplayMemberPath="Name"
        TextMemberPath="Name"
        IsClearButtonVisible="True"
        Placeholder="{x:Static strings:AppResources.SelectVendorPlaceholder}" />
    ```

## Expected Outcome
The `SfComboBox` cleanly displays the `Name` of the vendors but assigns the full `Vendor` object to the `SelectedVendor` property. Clearing the combobox sets the object to `null`, which natively triggers the `[Required]` validation error and locks the Save command.