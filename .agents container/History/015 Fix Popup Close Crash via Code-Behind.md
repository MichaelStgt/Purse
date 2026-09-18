# Task: Fix PlatformView Crash by Bypassing Popup Service for Teardown

## Objective
Resolve the WinUI 3 `PlatformView cannot be null here` crash by bypassing `IPopupService.ClosePopupAsync`. We will introduce an `Action` delegate in the ViewModel that allows the code-behind to safely execute the native `CommunityToolkit.Maui.Views.Popup.Close(result)` method.

## Execution Steps

### 1. Update TransactionLineItemPopupViewModel.cs
Locate the `TransactionLineItemPopupViewModel` class.

*   **Add an Action Delegate:** In the `#region Properties`, add the following property to allow the View to hook into the close command:
    ```csharp
    /// <summary>
    /// Action delegate to trigger the native popup close method in the code-behind.
    /// </summary>
    public Action<TransactionLineItem>? ClosePopupAction { get; set; }
    ```

*   **Modify SaveLineItemAsync():** Locate the `SaveLineItemAsync` method. Remove the `popupService.ClosePopupAsync` call entirely and replace it with the new action delegate. The bottom of the method should look exactly like this:
    ```csharp
    if (this.OnSaveCallback != null)
    {
        await this.OnSaveCallback(lineItem);
    }

    // Bypass the popupService and invoke the native close action in the code-behind
    this.ClosePopupAction?.Invoke(lineItem);
    ```

### 2. Update TransactionLineItemPopup.xaml.cs (Code-Behind)
Locate the code-behind file for the popup view (`TransactionLineItemPopup.xaml.cs`).

*   **Hook up the Action Delegate:** In the constructor, immediately after assigning the `BindingContext`, hook the ViewModel's `ClosePopupAction` to the native `this.Close()` method. 

    It should look like this:
    ```csharp
    public TransactionLineItemPopup(TransactionLineItemPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        
        // Safely close the popup using its own native PlatformView context
        vm.ClosePopupAction = (result) => 
        {
            this.Close(result);
        };
    }
    ```

## Expected Outcome
The application should no longer crash when saving an edited or added split. The ViewModel passes the data to the code-behind, which gracefully closes the popup natively, returning the result to `ShowPopupAsync` without relying on `Shell.Current`.