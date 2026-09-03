namespace Purse
{
    // using Purse.ViewModel;

    public partial class AppShell : Shell
    {
        public AppShell()
        {
            this.InitializeComponent();
        }

        public AppShell(AppShellViewModel? viewModel)
        {
            this.InitializeComponent();
            this.BindingContext = this;
            this.ViewModel = viewModel;

            this.RegisterRoutes(typeof(IShellNavigationTarget));

            // Routing.RegisterRoute(nameof(TransactionDetailView), typeof(TransactionDetailView));

        }
        public AppShellViewModel? ViewModel
        {
            get; set;
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            // base.OnNavigating(args);

            if (args.Source == ShellNavigationSource.Pop || args.Source == ShellNavigationSource.PopToRoot)
            {
                var currentPage = Shell.Current.CurrentPage;
                if (currentPage?.BindingContext is MVVMBaseTen.ViewModel.IEditableDetailViewModel editableVm)
                {
                    if (editableVm.IsNavigatingBackApproved)
                    {
                        return;
                    }

                    args.Cancel();

                    editableVm.Validate();

                    if (editableVm.HasErrors)
                    {
                        bool stay = await currentPage.DisplayAlertAsync(
                            Purse.Resources.Strings.AppResources.UnsavedChangesTitle,
                            Purse.Resources.Strings.AppResources.UnsavedChangesErrorsMessage,
                            Purse.Resources.Strings.AppResources.StayAndFixButton,
                            Purse.Resources.Strings.AppResources.DiscardAndLeaveButton);

                        if (!stay)
                        {
                            editableVm.IsNavigatingBackApproved = true;
                            await Shell.Current.GoToAsync("..");
                        }
                    }
                    else
                    {
                        editableVm.IsNavigatingBackApproved = true;
                        await editableVm.SaveCommandImplementation();
                    }
                }
            }
            // base.OnNavigating(args);
        }
    }
}
