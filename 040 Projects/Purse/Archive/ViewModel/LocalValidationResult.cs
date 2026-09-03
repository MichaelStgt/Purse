namespace Purse.ViewModel.Transactions
{
    using System;

    /// <summary>
    /// Defines the <see cref="LocalValidationResult" />.
    /// </summary>
    public partial class LocalValidationResult : ObservableObject // : System.ComponentModel.DataAnnotations.ValidationResult
    {
        #region Constructors

        //public ValidationResult(string? errorMessage)
        //    // : base(errorMessage, null)
        //{

        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="validationResult">The validationResult<see cref="System.ComponentModel.DataAnnotations.ValidationResult"/>.</param>
        public LocalValidationResult(System.ComponentModel.DataAnnotations.ValidationResult validationResult)
        {
            ArgumentNullException.ThrowIfNull(validationResult);

            this.ErrorMessage = validationResult.ErrorMessage;
            this.MemberNames = string.Join(", ", validationResult.MemberNames);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ErrorMessage.
        /// </summary>
        [ObservableProperty]
        public partial string ErrorMessage { get; set; } = string.Empty;

        // public IEnumerable<string> MemberNames

        /// <summary>
        /// Gets or sets the MemberNames.
        /// </summary>
        [ObservableProperty]
        public partial string MemberNames
        {
            get; set;
        }

        #endregion
    }
}
