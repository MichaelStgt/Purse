# Task: Clean Up Hardcoded XAML Styling

## Objective
The recently added `SfComboBox.NoResultsFoundTemplate` contains hardcoded values for `Padding`, `Spacing`, and `FontSize`. This violates the project's styling guidelines. Refactor the template to use the centralized styles and resources defined in `Styles.xaml`.

## Execution Steps

### 1. Update TransactionDetailView.xaml
Locate the `SfComboBox.NoResultsFoundTemplate` in the in-page overlay.
*   **Remove Hardcoded Sizes:** Strip out the hardcoded `Padding="15"`, `Spacing="10"`, and `FontSize="14"`.
*   **Apply Global Resources:** 
    *   Apply the appropriate standard `Spacing` and `Padding` `StaticResource` definitions (or a global `Style`) to the `VerticalStackLayout`.
    *   Apply the appropriate typography `Style` (e.g., a standard caption or body style) to the "Category not found" `Label` instead of explicitly declaring the font size.

## Expected Outcome
The `NoResultsFoundTemplate` contains zero hardcoded numeric values for layout or typography, relying entirely on the application's global resource dictionary.