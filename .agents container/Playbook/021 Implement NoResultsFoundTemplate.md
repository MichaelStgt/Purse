# Task: Move Category Creation to NoResultsFoundTemplate

## Objective
Refine the `SfComboBox` in the in-page overlay of `TransactionDetailView.xaml`. Move the "Create Expense Category" and "Create Income Category" buttons into the `SfComboBox.NoResultsFoundTemplate`. Implement a binding to explicitly close the dropdown from the ViewModel after a new category is created.

## Execution Steps

### 1. Update TransactionDetailViewModel.cs
Locate `TransactionDetailViewModel.cs`.
*   **Add Dropdown State Property:** Add a modern observable partial property to track the dropdown state:
    ```csharp
    [ObservableProperty]
    public partial bool IsCategoryDropdownOpen { get; set; }
    ```
*   **Modify Creation Commands:** Locate the `CreateExpenseCategoryAsync` and `CreateIncomeCategoryAsync` methods. At the very end of the creation logic, add:
    `this.IsCategoryDropdownOpen = false;`

### 2. Update TransactionDetailView.xaml (The SfComboBox)
Locate the `SfComboBox` used for the category selector inside the in-page overlay.
*   **Bind IsOpen:** Add the property `IsOpen="{Binding IsCategoryDropdownOpen, Mode=TwoWay}"` to the `SfComboBox`.
*   **Remove Old Buttons:** Delete the `VerticalStackLayout` (and its buttons) that was previously placed below the ComboBox to handle category creation.
*   **Implement the Template:** Uncomment or replace the existing placeholder template with the following XAML:

```xml
<SfComboBox.NoResultsFoundTemplate>
    <DataTemplate>
        <VerticalStackLayout Padding="15" Spacing="10">
            <Label FontSize="14" Text="Category not found. Create a new one:" TextColor="{AppThemeBinding Light={StaticResource Gray500}, Dark={StaticResource Gray400}}"/>
            <Button Command="{Binding BindingContext.CreateExpenseCategoryCommand, Source={RelativeSource AncestorType={x:Type ContentPage}}}" Text="Create Expense Category"/>
            <Button Command="{Binding BindingContext.CreateIncomeCategoryCommand, Source={RelativeSource AncestorType={x:Type ContentPage}}}" Text="Create Income Category"/>
        </VerticalStackLayout>
    </DataTemplate>
</SfComboBox.NoResultsFoundTemplate>