# Task: Diagnostic - Unblock Save Command

## Objective
The Save button is locked. Temporarily bypass the command restrictions by commenting them out (preserving the original code for easy restoration later), and inject UI alerts inside the execution to reveal exactly which validation rule is failing.

## Execution Steps

### 1. Comment Out ViewModel Command Locks (`TransactionDetailViewModel.cs` or `.Validations.cs`)
*   Locate the `Save` method.
*   Comment out the existing `RelayCommand` attribute and add a temporary, unrestricted one directly below it:
    ```csharp
    // PRESERVED FOR RECOVERY: [RelayCommand(CanExecute = nameof(...))]
    [RelayCommand] 
    private async Task Save()
    ```

### 2. Comment Out XAML Locks (`TransactionDetailView.xaml`)
*   Locate the `<ToolbarItem Text="Save" ... />`.
*   If it has an `IsEnabled` binding, comment out the original tag and duplicate it without the restriction:
    ```xml
    <!-- PRESERVED FOR RECOVERY: 
    <ToolbarItem Command="{Binding SaveCommand}" IsEnabled="{Binding ...}" Text="Save"/> 
    -->
    <ToolbarItem Command="{Binding SaveCommand}" Text="Save"/>
    ```

### 3. Inject Diagnostic Alerts (`TransactionDetailViewModel.cs`)
*   Inside the `Save` method, add this diagnostic logic at the very top to catch and display the exact error:
    ```csharp
    ValidateAllProperties(); // Force validation check

    if (HasErrors)
    {
        var errorDetails = string.Join("\n", GetErrors().Select(e => $"{string.Join(",", e.MemberNames)}: {e.ErrorMessage}"));
        await _alertService.ShowAlertAsync("Diagnostic: Validation Failed", errorDetails, "OK");
        return;
    }

    if (SelectedVendor == null)
    {
        await _alertService.ShowAlertAsync("Diagnostic: Missing Data", "SelectedVendor is null.", "OK");
        return;
    }
    ```
    *(Note for Agent: Ensure the alert call matches the exact injected service variable, e.g., `_alertService` or `AlertService`).*

## Expected Outcome
The original command restrictions are preserved as comments. The Save button is temporarily unrestricted and clickable immediately upon opening the page. Clicking it reveals an explicit alert showing the exact property names and error messages that are currently in a failed state.