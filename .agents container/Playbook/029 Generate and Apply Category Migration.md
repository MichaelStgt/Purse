# Task: Generate and Apply Category Migration

## Objective
Generate an Entity Framework Core migration for the recent `Category` model changes (removing `IsSystem`, adding `MonthlyBudget`) and update the local SQLite database.

## Execution Steps

### 1. Read Migration Commands Reference
*   Read the contents of `Purse.Data\Commands\Terminal\Migration Commands.md` to ensure you use the exact project arguments, paths, and syntax required for this specific solution.

### 2. Add Migration
*   Execute the appropriate `dotnet ef migrations add` command in the terminal. 
*   Name the migration `RefactorCategoryModel`.
*   Ensure the command correctly targets the `Purse.Data` project and the `Purse.Data.Migrator` startup project as specified in your reference file.

### 3. Update Database
*   Execute the `dotnet ef database update` command in the terminal using the same project and startup-project targeting.
    
## Expected Outcome
A new migration file named `[Timestamp]_RefactorCategoryModel.cs` is successfully generated in the `Purse.Data\Migrations` folder. The local SQLite database schema is updated without errors, ready for the new UI logic.