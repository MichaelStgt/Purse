TASK:
Now we must prepare the logic for the actual Transactions. A transaction can either be "Income" or "Expense". The UI needs to react dynamically based on the type of transaction the user is creating.

Please draft the structure for `TransactionDetailViewModel`. It must include:
1. An `enum` for `TransactionType` (Income, Expense).
2. An `[ObservableProperty]` for the selected `TransactionType`.
3. Logic (likely hooked into the `partial void OnTransactionTypeChanged` method provided by the source generator) that clears out the currently selected vendor or income source when the type changes.
4. Two separate observable collections: one for available `Vendors` (visible/enabled only when type is Expense) and one for available `IncomeSources` (visible/enabled only when type is Income). Name the collections 'VendorCollections' and 'IncomeSourcesCollection'
5. Remove the now existing 'Vendors' List and related Code to initialize it