# Task: Bust Validation Ghosts (Non-Destructive)

## Objective
The diagnostic popup revealed two specific error messages blocking the Save command: 
1. "Name can not be empty"
2. "Planned amount should be greater than total amount."

Locate the exact source of these error messages in the `TransactionDetailViewModel` (and its partial files) and comment them out. **Do NOT remove or alter the diagnostic alert code inside the `Save` command.**

## Execution Steps

### 1. Quarantine the "Name" Ghost
*   Perform a text search in `Purse\ViewModel\Transactions\TransactionDetailViewModel.cs` and `TransactionDetailViewModel.Validations.cs` for the exact string: `"Name can not be empty"`.
*   It may be an `ErrorMessage` parameter on a `[Required]` attribute for a differently named property, or inside a custom validation method.
*   **Comment out** the attribute or the `yield return new ValidationResult(...)` line generating this specific error.

### 2. Quarantine the "Planned Amount" Ghost
*   Perform a text search in the same files for the exact string: `"Planned amount should be greater than total amount."`
*   **Comment out** the custom validation method, the `[CustomValidation]` attribute, or the specific `if` block that generates this error.

### 3. LEAVE DIAGNOSTICS ACTIVE
*   Do **NOT** touch the `Save` command. 
*   Leave the `ValidateAllProperties();` call and the `_alertService.ShowAlertAsync(...)` diagnostic popups exactly as they are currently written. 
*   We must keep the diagnostic code active to verify the fix.

## Expected Outcome
The exact lines of code producing the two ghost validation strings are commented out. The diagnostic alert logic remains fully active in the Save command to prove the validation pipeline is now clear.