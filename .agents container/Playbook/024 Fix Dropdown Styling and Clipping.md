# Task: Fix NoResultsFoundTemplate Styling and Clipping

## Objective
Resolve the white background issue in dark mode and fix the height clipping that hides the "Create Category" buttons inside the `SfComboBox.NoResultsFoundTemplate`.

## Execution Steps

### 1. Fix Template Background and Height (`TransactionDetailView.xaml`)
Locate the `VerticalStackLayout` inside the `SfComboBox.NoResultsFoundTemplate`.

*   **Fix Background Theme:** Add an `AppThemeBinding` to the `BackgroundColor` so it automatically adapts to light and dark modes. (Use the appropriate standard color resources from `Styles.xaml` or `ApplicationColors.xaml`, such as `White` and `Gray900` or `Black`).
*   **Force Minimum Height:** Add a `MinimumHeightRequest` of at least `160` to force the Syncfusion dropdown container to allocate enough physical space for the label and both buttons.

*Example:*
```xml
<VerticalStackLayout BackgroundColor="{AppThemeBinding Light={StaticResource White}, Dark={StaticResource Black}}" MinimumHeightRequest="160" Padding="15" Spacing="10">