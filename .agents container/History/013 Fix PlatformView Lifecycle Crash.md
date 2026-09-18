# Task: Fix PlatformView Null Crash on Popup Return

## Objective
Resolve the `PlatformView cannot be null here` crash that occurs when returning data from the Transaction Line Item popup. This requires removing the fatal `throw;` statement in the parent ViewModel and decoupling the `Splits` collection update from the popup teardown lifecycle using `MainThread.BeginInvokeOnMainThread`.

## Execution Steps

### 1. Update TransactionDetailViewModel.cs
Locate the `DisplayTransactionLineItemPopup(TransactionLineItem? splitToEdit = null)` method.

*   **Remove the `throw;`:** In the `catch (Exception ex)` block at the bottom of the method, delete the `throw;` statement so exceptions are logged but do not crash the application.
*   **Wrap the Result Processing:** Find the block where `popupResult != null && !popupResult.WasDismissedByTappingOutsideOfPopup` is evaluated. Wrap the inner logic that modifies the `Splits` collection inside `MainThread.BeginInvokeOnMainThread`.

The success block must look exactly like this:
```csharp
if (popupResult != null && !popupResult.WasDismissedByTappingOutsideOfPopup)
{
    if (popupResult.Result is TransactionLineItem newLineItem)
    {
        // Decouple the UI redraw from the popup teardown
        MainThread.BeginInvokeOnMainThread(() => 
        {
            if (splitToEdit == null)
            {
                this.Splits.Add(newLineItem);
            }
            else
            {
                splitToEdit.Amount = newLineItem.Amount;
                splitToEdit.Category = newLineItem.Category;
            }
            
            this.UpdateSplitProperties();
        });
    }
}