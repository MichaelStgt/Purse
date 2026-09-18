# Task: Implement Smart Empty View for Transaction Splits

## Objective
Remove the default empty split generated for new transactions. Implement a `CollectionView.EmptyView` that provides three distinct starting paths (Manual, Upload, Camera) for populating the transaction.

## Execution Steps

### 1. Remove Default Split Injection (`TransactionDetailViewModel.cs`)
*   Locate the initialization logic for a new `Transaction` (likely in the constructor or an `Initialize` method).
*   Ensure the `Splits` collection (or `ObservableCollection<TransactionLineItem>`) is initialized as completely empty. Remove any code that automatically adds a blank or $0.00 `TransactionLineItem`.

### 2. Wire Up Empty State Commands (`TransactionDetailViewModel.cs`)
*   Create three new `[RelayCommand]` methods to handle the empty state actions:
    *   `AddSplitManually()`: This should execute the exact same logic as your existing add button (e.g., calling `OpenOverlayCommand(null)` or setting `IsOverlayVisible = true`).
    *   `UploadReceiptAsync()`: Create an empty placeholder method returning `Task.CompletedTask` for future file picker implementation.
    *   `ScanReceiptAsync()`: Create an empty placeholder method returning `Task.CompletedTask` for future camera implementation.

### 3. Implement the EmptyView (`TransactionDetailView.xaml`)
*   Locate the `CollectionView` bound to the `Splits` collection.
*   Define the `CollectionView.EmptyView` to display the three action buttons when the list is empty. Use existing global styles for spacing and sizing.

```xml
<CollectionView.EmptyView>
    <VerticalStackLayout HorizontalOptions="Center" Padding="20" Spacing="15" VerticalOptions="Center">
        <Label FontSize="18" HorizontalOptions="Center" Margin="0,0,0,10" Text="No items added yet." TextColor="{AppThemeBinding Light={StaticResource Gray500}, Dark={StaticResource Gray400}}"/>
        
        <Button Command="{Binding AddSplitManuallyCommand}" Style="{StaticResource PrimaryButtonStyle}" Text="✍️ Add Manually"/>
        <Button Command="{Binding UploadReceiptCommand}" Style="{StaticResource SecondaryButtonStyle}" Text="📁 Upload Receipt"/>
        <Button Command="{Binding ScanReceiptCommand}" Style="{StaticResource SecondaryButtonStyle}" Text="📷 Scan Receipt"/>
    </VerticalStackLayout>
</CollectionView.EmptyView>