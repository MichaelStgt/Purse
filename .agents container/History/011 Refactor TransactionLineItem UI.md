# Task: Refactor Transaction Line Item UI for Editing

## Objective
Simplify the split item rows in `TransactionDetailView.xaml`. The category should be displayed as a simple label that triggers the edit popup when tapped. The amount remains directly editable via the existing `ExtendedEntry`. No additional edit buttons should be added.

## Execution Steps

### 1. Update TransactionDetailView.xaml
Locate the `DataTemplate` for `TransactionLineItem` inside the `VerticalStackLayout` bound to `Splits`.

*   **Comment Out the ComboBox:** Comment out the `Border` and the `SfComboBox` completely.
*   **Add Clickable Label:** In `Grid.Column="0"`, add a new `Border` containing a `Label` bound to `Category`. Apply standard text styling.
*   **Add TapGestureRecognizer:** Attach a `TapGestureRecognizer` to this new `Border` (or the Label). 
    *   Bind the `Command` to `EditSplitCommand` on the parent `TransactionDetailViewModel` (using `RelativeSource AncestorType={x:Type ContentPage}`).
    *   Bind the `CommandParameter` to the current item (`{x:Binding .}`).
*   **Preserve Amount & Delete:** Leave the `ExtendedEntry` for `Amount` (in `Grid.Column="1"`) and the Delete `ImageButton` (in `Grid.Column="2"`) exactly as they are.

## Expected Outcome
The user sees a clean text label for the category. Tapping the category name triggers the `EditSplitCommand` and opens the popup with the existing data. The amount can still be typed directly into the row without opening the popup.