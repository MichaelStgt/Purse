# Task: Refactor Category Model and Initialization

## Objective
Remove the restrictive "System" category concept and introduce a "Monthly Budget" feature. Update the `Category` model, refactor the database seeder to stop locking default categories, and prepare the environment for an Entity Framework Core migration.

## Execution Steps

### 1. Update the Category Model (`Purse.Shared\Category.cs`)
Open `Purse.Shared\Category.cs`.
*   **Remove IsSystem:** Delete the `public bool IsSystem { get; set; }` property entirely.
*   **Add MonthlyBudget:** Add a new property to track the budget target:
    ```csharp
    /// <summary>
    /// The target monthly budget limit for this category.
    /// </summary>
    public decimal MonthlyBudget { get; set; }
    ```

### 2. Update the Database Seeder (`Purse.Data\Services\DbContextInitializer.cs`)
Open `Purse.Data\Services\DbContextInitializer.cs`.
*   Locate the `Initialize` method where the default categories are loaded from the `.resx` resource files.
*   Remove any code that assigns `IsSystem = true` to the newly created categories.
*   Ensure the initial `MonthlyBudget` is set to `0` (or left to its default struct value).

### 3. Clean Up the UI Locks
*   **CategoryDetailViewModel.cs:** Remove any logic that checks `IsSystem` to disable editing or saving.
*   **CategoryDetailView.xaml:** Remove any `IsReadOnly` or `IsEnabled` bindings that relied on `IsSystem` to lock the UI inputs.

## Expected Outcome
The `Category` model has been updated, the seeder no longer flags items as system-owned, and the UI no longer locks based on the removed property. The code compiles, but will require an EF Core Migration before running.