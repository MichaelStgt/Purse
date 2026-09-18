namespace Purse.ViewModel
{
    using Purse.Shared.Model;
    using System.Threading.Tasks;

    /// <summary>
    /// Shared service to pass parameters to the TransactionLineItemPopupViewModel.
    /// </summary>
    public class TransactionLineItemPopupParameters
    {
        public Category? SelectedCategory { get; set; }
        public string? SearchText { get; set; }
        public decimal Amount { get; set; }
        public TaskCompletionSource<TransactionLineItem?>? TaskCompletionSource { get; set; }
    }
}
