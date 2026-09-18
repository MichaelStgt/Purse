```markdown
# Task: Refactor ObservableProperties to Modern Syntax

## Objective
Sweep the existing ViewModels and update any legacy `[ObservableProperty]` private backing fields to the modern C# 11 `partial property` syntax as defined in `01-dotnet-standards.md`.

## Execution Steps

### 1. Refactor TransactionDetailViewModel.cs
*   Locate `Purse\ViewModel\Transactions\TransactionDetailViewModel.cs`.
*   Find `[ObservableProperty] private bool isOverlayVisible;` (and any other properties using private backing fields).
*   Refactor them to `public partial [Type] [PropertyName] { get; set; }`.

### 2. Sweep Other ViewModels
*   Briefly check `CategoryDetailViewModel.cs`, `VendorDetailViewModel.cs`, and `IncomeSourceDetailViewModel.cs` for any private backing fields using `[ObservableProperty]`.
*   Convert any found instances to the modern partial property syntax.

## Expected Outcome
The codebase contains zero instances of `[ObservableProperty] private <type> <name>;`. All ViewModels successfully compile using the modern partial property syntax.