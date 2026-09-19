# Task: RECOVERY - Decommission Planned Amount

## Objective
The agent accidentally restored the `PlannedAmount` property and its complex cross-validations. Remove it completely from the active UI and ViewModel logic to restore application stability.

## Execution Steps

### 1. Clean the UI (`TransactionDetailView.xaml`)
*   Locate the `PlannedAmount` input field (Entry/Label) and any associated validation error labels.
*   Comment out (`<!-- ... -->`) these elements.

### 2. Clean the ViewModel (`TransactionDetailViewModel.cs`)
*   Delete the `[ObservableProperty] public partial decimal PlannedAmount { get; set; }` property.
*   Delete any custom validation attributes or methods that compare `TotalAmount` or the split sums against `PlannedAmount`.
*   Ensure the standard `TotalAmount` validation only requires it to be a valid decimal, not necessarily "> 0" at initialization.

## Expected Outcome
The project compiles. The UI no longer displays the "Planned Amount" field, and the user is not blocked by validation errors requiring the amount to be greater than zero upon opening a new transaction.