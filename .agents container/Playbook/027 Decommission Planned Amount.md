# Task: Decommission and Preserve "Planned Amount" Logic

## Objective
The "Planned Amount" feature is no longer needed for the active application flow, but its cross-property validation logic serves as an excellent reference. Remove its active implementation from the UI and main ViewModel, and move the commented-out C# logic into `TransactionDetailViewModel.Validations.cs` for preservation.

## Execution Steps

### 1. Comment Out UI Elements (`TransactionDetailView.xaml`)
*   Locate any UI elements (Labels, Entries, or layout containers) bound to `PlannedAmount` or displaying validation errors specifically for `PlannedAmount`.
*   Wrap these elements in XAML comments `<!-- ... -->` rather than deleting them.

### 2. Extract and Move ViewModel Logic (`TransactionDetailViewModel.cs`)
*   Locate the `PlannedAmount` property (likely an `[ObservableProperty]`).
*   Locate any custom validation attributes, methods, or cross-property validation logic that compares `PlannedAmount` to the `TotalAmount` or split sums.
*   Locate any initialization logic or save logic referencing `PlannedAmount`.
*   **Remove** this code from `TransactionDetailViewModel.cs`.

### 3. Preserve as Reference (`TransactionDetailViewModel.Validations.cs`)
*   Open `TransactionDetailViewModel.Validations.cs`.
*   At the bottom of the file (inside the class definition), create a clear reference block:
    ```csharp
    // ====================================================================
    // REFERENCE: Cross-Property Validation Example (Planned Amount)
    // The following code demonstrates how to validate values across 
    // multiple properties. It is currently decommissioned.
    // ====================================================================
    /*
    [Paste all the extracted properties, validation attributes, and methods here]
    */
    ```
*   Ensure the code is fully encapsulated in the `/* ... */` block so it does not interfere with the active compiler.

## Expected Outcome
The project compiles successfully. The "Planned Amount" field no longer appears in the UI or active transaction logic. The advanced validation logic is safely preserved as a commented reference block in `TransactionDetailViewModel.Validations.cs`.