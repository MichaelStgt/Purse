# Task: Move Category Creation to Dropdown Footer

## Objective
The `NoResultsFoundTemplate` in Syncfusion's `SfComboBox` is aggressively clipping content. To provide a stable and consistent UX, move the "Create Category" buttons to the persistent `DropdownFooterView`.

## Execution Steps

### 1. Update TransactionDetailView.xaml
Locate the `SfComboBox` used for the category selector inside the in-page overlay.

*   **Enable Footer Properties:** Add the following properties directly to the `<SfComboBox>` element to explicitly enable and size the footer area:
    ```xml
    ShowDropdownFooterView="True"
    DropdownFooterViewHeight="120"
    ```
*   **Replace Template with Footer:** Remove the entire `<SfComboBox.NoResultsFoundTemplate>` block. Replace it with the following `<SfComboBox.DropdownFooterView>` block:

```xml
<SfComboBox.DropdownFooterView>
    <VerticalStackLayout BackgroundColor="{AppThemeBinding Light={StaticResource White}, Dark={StaticResource Black}}" Padding="10" Spacing="10">
        <Button Command="{Binding BindingContext.CreateExpenseCategoryCommand, Source={RelativeSource AncestorType={x:Type ContentPage}}}" Text="Create Expense Category"/>
        <Button Command="{Binding BindingContext.CreateIncomeCategoryCommand, Source={RelativeSource AncestorType={x:Type ContentPage}}}" Text="Create Income Category"/>
    </VerticalStackLayout>
</SfComboBox.DropdownFooterView>