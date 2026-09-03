namespace Purse
{
    using Purse.Services;

    public partial class App : Application
    {
        public const string SyncfusionLicenseKey = "Ngo9BigBOggjHTQxAR8/V1JHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWXxcd3ZVRWBZVUR+WEdWYEo=";

        // public const string SyncfusionLicenseKey = "Ngo9BigBOggjHTQxAR8/V1JGaF5cX2dCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5cdnVUQmdeUEF2WEFWYEs=";
        public IServiceProvider? Services
        {
            get;
        }

        public App()
        {
            //this.Services = new ServiceCollection()
            //// .AddTransient<MainViewModel>()
            //// .AddTransient<IAlertService, AlertService>()
            //.AddScoped<IDbInitializer, DbContextInitializer>()
            //.AddDbContext<LocalDbContext>(options => options.UseSqlite(SqliteHelper.Connection))
            //.BuildServiceProvider();

            //using (IServiceScope scope = this.Services.CreateScope())
            //{
            //    IDbInitializer initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            //    initializer.Initialize();
            //}

            if (!this.IsInitialized)
            {
                this.InitializeApplication();
            }

            this.InitializeComponent();
        }

        public bool IsInitialized
        {
            get; set;
        }

        #region Methods

        /// <summary>
        /// The InitializeApplication.
        /// </summary>
        private async void InitializeApplication()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(SyncfusionLicenseKey);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Debug.WriteLine(e.ExceptionObject.ToString());
            };

            // DataSeedingService? dataSeedingService = DataSeedingService.Current;
            // DataSeedingService? dataSeedingService = new DataSeedingService(this.cloudService);
            // await dataSeedingService!.LoadCurrencies().ConfigureAwait(false);
            this.IsInitialized = true;
        }

        #endregion
    }
}