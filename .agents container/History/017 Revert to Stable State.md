# Task: Revert to Last Known Stable State

## Objective
Revert all experimental UI and ViewModel changes related to popup closing delegates, main thread invocations, and row tap gestures. Restore the application to the state where adding a new split via `IPopupService` worked correctly.

## Execution Steps

### 1. Revert TransactionDetailView.xaml
*   **Restore ComboBox:** Uncomment and restore the `SfComboBox` and `ExtendedEntry` layout for the `TransactionLineItem` DataTemplate.
*   **Remove TapGesture:** Delete the `Label` and `TapGestureRecognizer` that were added to make the row clickable.
*   **Restore Buttons:** Ensure the DataTemplate only uses the standard layout without the experimental Edit button overlays.

### 2. Revert TransactionDetailViewModel.cs
*   **Remove Flags:** Delete the `private bool isPopupOpen;` field.
*   **Revert Display Method:** Restore `DisplayTransactionLineItemPopup` to its baseline implementation. Remove the `try-finally` block, and remove `MainThread.BeginInvokeOnMainThread`. Keep the standard `IPopupService.ShowPopupAsync(Shell.Current, ...)` call.

### 3. Revert TransactionLineItemPopupViewModel.cs
*   **Remove Delegates & Flags:** Delete the `ClosePopupAction` property and the `private bool isClosing;` field.
*   **Revert Save Method:** Restore `SaveLineItemAsync` to use the standard toolkit call:
    ```csharp
    if (this.popupService != null)
    {
        await Task.Delay(100);
        await this.popupService.ClosePopupAsync(Shell.Current, lineItem);
    }
    ```

### 4. Revert TransactionLineItemPopup.xaml.cs (Code-Behind)
*   **Remove Delegate Hook:** Delete the assignment to `vm.ClosePopupAction` inside the constructor. The constructor should only call `InitializeComponent()` and assign the `BindingContext`.