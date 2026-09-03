namespace Purse.Messaging
{
    using CommunityToolkit.Mvvm.Messaging.Messages;
    using Purse.Shared.Model;

    /// <summary>
    /// Represents the action taken on a transaction.
    /// </summary>
    public enum TransactionChangeAction
    {
        Created,
        Updated,
        Deleted
    }

    /// <summary>
    /// Message sent when a transaction is created, updated, or deleted.
    /// </summary>
    public class TransactionChangedMessage : ValueChangedMessage<Transaction>
    {
        /// <summary>
        /// Gets the action.
        /// </summary>
        public TransactionChangeAction Action { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionChangedMessage"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="action">The action.</param>
        public TransactionChangedMessage(Transaction transaction, TransactionChangeAction action)
            : base(transaction)
        {
            this.Action = action;
        }
    }
}
