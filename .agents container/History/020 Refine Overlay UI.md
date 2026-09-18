# Task: Refine In-Page Overlay UI

## Objective
The functional logic for the in-page overlay is working, but the UI was poorly generated. The order of inputs is backward (Amount is first, Category is second), and the Category input is just a plain entry instead of the proper selection/search UI. Rebuild the overlay layout to match the intended, polished design.

## Execution Steps

### 1. Fix the Input Sequence and Layout (`TransactionDetailView.xaml`)
Locate the `Grid` containing the overlay (`IsVisible="{Binding IsOverlayVisible}"`).
Inside its `Border`, replace the lazy entries with a well-structured `VerticalStackLayout` using a `Spacing="15"`.

The visual order MUST be:
1.  **Title:** A Label indicating "Edit Split" or "Add Split".
2.  **Category Selector:** Restored from previous logic (see Step 2).
3.  **New Category Prompt:** (see Step 3).
4.  **Amount Entry:** The `ExtendedEntry` bound to `OverlayAmount`.
5.  **Action Buttons:** Save and Cancel buttons in a horizontal layout.

### 2. Restore Category Selection UX
Replace the simple Category entry with a proper UI for selecting a category. 
*   If using an `SfComboBox`, bind its `ItemsSource` to the loaded `Categories` (or `AvailableCategories`) and bind its input to `OverlaySearchText` or `OverlaySelectedCategory`.
*   Ensure it has a placeholder like "Select or type category...".

### 3. Restore "Create New Category" Prompt
Immediately below the Category selector, add a container (e.g., `VerticalStackLayout`) bound to `IsVisible="{Binding IsNewCategoryPromptVisible}"`.
Inside it, add two cleanly styled buttons:
*   "Create Expense Category" -> Command: `CreateExpenseCategoryCommand`
*   "Create Income Category" -> Command: `CreateIncomeCategoryCommand`

### 4. Polish the Amount Entry and Action Buttons
*   Ensure the Amount entry uses `IsNumeric="True"`, `Keyboard="Numeric"`, and `HorizontalTextAlignment="End"`.
*   Place the "Cancel" and "Save" buttons at the bottom in a `Grid` or `HorizontalStackLayout` with proper spacing, binding them to `CancelOverlayCommand` and `SaveOverlayCommand`.

## Expected Outcome
The overlay looks like a premium, native modal. Category is on top, Amount is on the bottom. The Category field properly allows searching/selecting, and typing a new category name dynamically reveals the "Create" buttons before the user moves on to the Amount field.