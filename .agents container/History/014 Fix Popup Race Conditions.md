# Task: Fix PlatformView Crash via Race Condition Prevention

## Objective
Resolve the `PlatformView cannot be null here` crash when opening and closing the `TransactionLineItemPopup`. This requires preventing double-execution of the open/close commands and increasing the teardown delay so the OS soft-keyboard can retract safely.

## Execution Steps

### 1. Protect the Popup Open Logic (TransactionDetailViewModel.cs)
Locate the `TransactionDetailViewModel.cs` file. We need to prevent the user from double-tapping a row and spawning two overlapping popups.

*   **Add a private flag:** At the top of the `#region Fields`, add:
    `private bool isPopupOpen;`
*   **Wrap the Method:** Locate `public async Task DisplayTransactionLineItemPopup(TransactionLineItem? splitToEdit = null)`. Update the method to check the flag, set it, and wrap the entire remaining logic in a `try-finally` block to release it.

```csharp
public async Task DisplayTransactionLineItemPopup(TransactionLineItem? splitToEdit = null)
{
    if (this.isPopupOpen) return;
    this.isPopupOpen = true;

    try 
    {
        if (this.popupService == null) return;
        
        // ... ALL EXISTING DICTIONARY, POPUPOPTIONS, AND SHOWPOPUPASYNC LOGIC ...
    }
    finally 
    {
        this.isPopupOpen = false;
    }
}

