# Task: Implement Category Transaction Counters and Smart Delete

## Objective
Enhance the Category feature by calculating how many transaction splits use each category. Display this count in the `CategoryCollectionView` and use it to prevent deletion of in-use categories, showing a friendly alert instead.

## Execution Steps

### 1. Update the Category Model (`Purse.Shared\Category.cs`)
*   Add a new property to hold the transaction count. Use the `[NotMapped]` attribute so Entity Framework ignores it for database migrations:
    ```csharp
    using System.ComponentModel.DataAnnotations.Schema;

    // ... inside Category class
    [NotMapped]
    public int TransactionCount { get; set; }
    ```

### 2. Populate the Transaction Count (`Purse.Data\LocalDbContext.Query.cs` or relevant service)
*   Locate the method(s) responsible for fetching Categories (e.g., `GetCategoriesAsync`).
*   Update the Entity Framework query to calculate and project the `TransactionCount`. For example, using a join or a subquery against `TransactionLineItems` where the `CategoryId` matches. 
*   *Note: Ensure this is done efficiently so it doesn't cause an N+1 query problem.*

### 3. Implement Smart Delete (`CategoryDetailViewModel.cs` & `CategoryCollectionViewModel.cs`)
*   Locate the `Delete` command(s) for categories.
*   Before executing the database deletion, check `Category.TransactionCount`.
*   If `TransactionCount > 0`, intercept the deletion:
    *   Use the `AlertService` to display a message: 
        *   **Title:** "Cannot Delete Category"
        *   **Message:** "This category cannot be deleted because it is currently used in {TransactionCount} transaction(s)."
        *   **Cancel/OK Button:** "OK"
    *   Return early (`return;`) so the deletion does not occur.
*   If `TransactionCount == 0`, proceed with the normal deletion logic.

### 4. Display the Count in the UI (`CategoryCollectionView.xaml`)
*   Locate the `DataTemplate` for the categories list.
*   Add a subtle `Label` (e.g., secondary text color, smaller font size) next to or below the Category Name to display the usage count.
*   *Example binding:* `Text="{Binding TransactionCount, StringFormat='{0} transactions'}"`

## Expected Outcome
The categories list now displays the number of transactions associated with each category. Clicking delete on an actively used category safely aborts the operation and presents a clear, helpful alert dialog to the user explaining why. Unused categories can still be deleted normally.