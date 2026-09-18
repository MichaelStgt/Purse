# Task: Global Localization Sweep

## Objective
Eliminate all hardcoded user-facing strings across the UI and ViewModel layers. Move all text to `AppResources.resx` and generate corresponding German (`AppResources.de.resx`) and Spanish (`AppResources.es.resx`) translations.

## Execution Steps

### 1. Extract and Translate Strings
Sweep the application for hardcoded user-facing text and add them to the `.resx` files. Specifically target:
*   **CategoryCollectionView.xaml:** The `StringFormat='{0} transactions'` binding. (Create a format string resource like `"{0} transactions"`, `"M{0} Transaktionen"`, `"{0} transacciones"`).
*   **TransactionDetailView.xaml:** The `EmptyView` elements ("No items added yet.", "✍️ Add Manually", "📁 Upload Receipt", "📷 Scan Receipt"). Include the emojis in the resource values.
*   **AppShell.xaml:** All FlyoutItem, Tab, and ShellContent `Title` attributes.
*   **All ViewModels:** The `Title = "..."` assignments in constructors, and the hardcoded `AlertService` strings we just added for the Smart Delete feature.
*   **All Views:** Any remaining hardcoded `Text`, `Placeholder`, or `Title` attributes.

### 2. Update Resource Files
For every string found:
*   Add the key and English value to `Purse.Shared\Resources\Strings\AppResources.resx`.
*   Add the German translation to `Purse.Shared\Resources\Strings\AppResources.de.resx`.
*   Add the Spanish translation to `Purse.Shared\Resources\Strings\AppResources.es.resx`.

### 3. Replace Hardcoded Values
*   **In XAML:** Replace the hardcoded text with static resource bindings. 
    *   *Example:* `Text="{x:Static strings:AppResources.AddManuallyBtn}"`
    *   *(Agent Note: Ensure the `xmlns:strings="clr-namespace:Purse.Shared.Resources.Strings;assembly=Purse.Shared"` namespace is declared at the top of the XAML files).*
*   **In C#:** Replace the hardcoded strings with `AppResources.[KeyName]`.

## Expected Outcome
The codebase contains zero hardcoded user-facing strings. The application fully supports English, German, and Spanish out of the box.