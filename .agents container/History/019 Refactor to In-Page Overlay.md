# Task: Refactor Popup to In-Page Overlay

## Objective
Remove the `CommunityToolkit.Maui` `IPopupService` dependency entirely due to fatal WinUI 3 teardown bugs. Convert the `TransactionLineItemPopup` into an in-page `Grid` overlay inside `TransactionDetailView.xaml`. Consolidate the popup's logic into `TransactionDetailViewModel.cs` and implement a clickable label for editing splits.

## Execution Steps

### 1. Merge ViewModel Logic (`TransactionDetailViewModel.cs`)
*   **Remove IPopupService:** Remove `IPopupService` and `TransactionLineItemPopupParameters` from the constructor and fields.
*   **Add Overlay State Properties:** Add the following `[ObservableProperty]` fields:
    *   `private bool isOverlayVisible;`
    *   `private decimal overlayAmount;`
    *   `private string? overlaySearchText;`
    *   `private Category? overlaySelectedCategory;`
    *   `private bool isNewCategoryPromptVisible;`
*   **Add Tracking Field:** Add a private field to track what is being edited: `private TransactionLineItem? currentEditingSplit;`
*   **Migrate Category Logic:** Copy the `EvaluateCategoryMatch`, `CreateExpenseCategoryAsync`, and `CreateIncomeCategoryAsync` methods from the old `TransactionLineItemPopupViewModel.cs` into this ViewModel. Ensure `OnOverlaySearchTextChanged` triggers `EvaluateCategoryMatch`.
*   **Update Open Logic:** Replace `DisplayTransactionLineItemPopup` with `OpenOverlayCommand(TransactionLineItem? split)`. This command should populate `OverlayAmount` and `OverlaySearchText` from the split (or defaults if null), set `currentEditingSplit = split`, and set `IsOverlayVisible = true`.
*   **Create Close/Save Commands:** 
    *   `CancelOverlayCommand()`: Sets `IsOverlayVisible = false`.
    *   `SaveOverlayCommand()`: Updates `currentEditingSplit` with the overlay values (or adds a new split to `this.Splits` if null), calls `UpdateSplitProperties()`, and sets `IsOverlayVisible = false`.

### 2. Implement the Overlay UI (`TransactionDetailView.xaml`)
*   **Ensure Root is a Grid:** Ensure the outermost element of the page is a `<Grid>`. If it is a `ScrollView`, wrap it in a `<Grid>`.
*   **Add the Overlay Grid:** At the very bottom of the root `<Grid>` (so it renders on top of everything else), add:
    ```xml
    <Grid BackgroundColor="#A0000000" IsVisible="{Binding IsOverlayVisible}" ZIndex="100">
        <!-- Recreate the Popup UI here inside a Border -->
        <Border HorizontalOptions="Center" Padding="20" VerticalOptions="Center" WidthRequest="350">
            <!-- Add Amount Entry bound to OverlayAmount -->
            <!-- Add Category Search Entry bound to OverlaySearchText -->
            <!-- Add New Category Buttons bound to IsNewCategoryPromptVisible -->
            <!-- Add Save and Cancel Buttons bound to SaveOverlayCommand and CancelOverlayCommand -->
        </Border>
    </Grid>
    ```

### 3. Implement the Clickable Label UX (`TransactionDetailView.xaml`)
*   Locate the `DataTemplate` for the `TransactionLineItem` bound to the `Splits` collection.
*   Comment out or remove the `SfComboBox`.
*   Replace it with a `Label` bound to `Category`. Add a `TapGestureRecognizer` to this Label (or its container) bound to `OpenOverlayCommand` on the parent BindingContext, passing the current split (`{x:Binding .}`) as the CommandParameter.
*   Leave the `Amount` entry and Delete button as they are.

### 4. Cleanup
*   Delete `TransactionLineItemPopup.xaml` and `TransactionLineItemPopup.xaml.cs`.
*   Delete `TransactionLineItemPopupViewModel.cs`.