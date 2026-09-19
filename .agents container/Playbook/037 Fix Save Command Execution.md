# Task: Fix Save Command CanExecute Pipeline

## Objective
The "Save" ToolbarItem is currently disabled. Audit the `TransactionDetailViewModel` and its validation partial class to repair the `CanExecute` logic for the `SaveCommand` and ensure UI updates are triggered when valid data is entered.

## Execution Steps

### 1. Audit the CanSave Logic (`TransactionDetailViewModel.Validations.cs` / `TransactionDetailViewModel.cs`)
*   Locate the `SaveCommand` (usually defined as `[RelayCommand(CanExecute = nameof(CanSave))]`).
*   Review the `CanSave()` (or similar) predicate method.
*   Ensure it evaluates correctly based ONLY on current, active properties (e.g., `TotalAmount > 0`, `SelectedVendor != null`, and `!HasErrors`). 
*   Ensure there are no leftover checks for the decommissioned `PlannedAmount` property.

### 2. Restore Notification Triggers (`TransactionDetailViewModel.cs`)
For the Save button to enable dynamically as the user types or selects items, every property evaluated in `CanSave()` must notify the command to re-evaluate.
*   Check the `SelectedVendor` property. Ensure it has:
    `[NotifyCanExecuteChangedFor(nameof(SaveCommand))]`
*   Check the `TotalAmount` property. Ensure it has:
    `[NotifyCanExecuteChangedFor(nameof(SaveCommand))]`
*   Check `TransactionDate`, `ReceiptImage`, and any other required properties to ensure they also possess the `NotifyCanExecuteChangedFor` attribute.

### 3. Clear Ghost Validations
*   Check for any custom validation attributes applied to `TotalAmount` or `SelectedVendor` that might be failing silently. 
*   If `CanSave()` checks `!HasErrors`, ensure no hidden validation logic is running in the background and keeping `HasErrors` set to `true`.

## Expected Outcome
The Save button enables immediately when the user selects a Vendor and enters a valid Total Amount. It correctly disables if required fields are cleared.