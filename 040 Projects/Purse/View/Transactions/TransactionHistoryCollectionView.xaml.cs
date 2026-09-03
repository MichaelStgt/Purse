namespace Purse.View
{
    // using global::Purse.ViewModels;
    // using Purse.ViewModel;


    /// <summary>
    /// The transaction history collection view.
    /// </summary>
    public partial class TransactionHistoryCollectionView : ContentPage
    {
        /// <summary>
        /// Indicates whether this is the first time the page is appearing.
        /// </summary>
        private bool isFirstAppearing = true;

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>A TransactionHistoryCollectionViewModel</value>
        private TransactionHistoryCollectionViewModel ViewModel
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHistoryCollectionView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public TransactionHistoryCollectionView(TransactionHistoryCollectionViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
            this.BindingContext = this.ViewModel;
        }

        /// <summary>
        /// On appearing.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load the list only the first time the page appears.
            // Subsequent updates are handled dynamically via transaction changed messenger messages.
            if (this.isFirstAppearing)
            {
                this.isFirstAppearing = false;
                this.ViewModel.LoadItemCollectionCommand.Execute(null);
            }
        }
    }
}
