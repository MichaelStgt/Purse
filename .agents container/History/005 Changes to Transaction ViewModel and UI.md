After implementing the Vendor and ImcomeSource functionality, we need to change the current UI for the Transactions.

First of all, we assume that a single transaction is of type income or expense.
1. The TransactionHistoryCollectionView should highlight 
The difference between income and expese. 
2. The TransactionDetailView should respect that we can have Income or Expense Categerories. Please add the same ChipSwith like in CategoryDetailView to change the Type of the Transaction.
3. The Category Selection in the View should only display Categories depending on the Selection of the Switch.
4. If there are any Splits already defined using a Category of the other Category Type, we ask the user if he does want to wipe out the exiting ones. 