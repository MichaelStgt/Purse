namespace Purse.ViewModel.Transactions
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class TransactionDetailViewModel : ObservableValidator
    {
        private string _amountInput;

        [Required]
        [CustomValidation(typeof(TransactionDetailViewModel), nameof(ValidateDoubleInput))]
        public string AmountInput
        {
            get => _amountInput;
            set => SetProperty(ref _amountInput, value, true); // True triggers validation
        }

        public static ValidationResult ValidateDoubleInput(string value, ValidationContext context)
        {
            if (double.TryParse(value, out double result))
            {
                // Optional: Add range or business logic here
                if (result < 0)
                    return new ValidationResult("Value cannot be negative.");
                return ValidationResult.Success;
            }
            return new ValidationResult("Please enter a valid numeric value.");
        }
    }
}