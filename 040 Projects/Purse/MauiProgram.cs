namespace Purse
{
    using CommunityToolkit.Maui;
    using CommunityToolkit.Maui.Core;
    using CommunityToolkit.Maui.Storage;
    using LibraryTen;
    using LibraryTen.Authentication;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using MVVMBaseTen.Common;
    using Purse.Data;
    using Purse.Data.Services;
    using Purse.Internal;
    // using Purse.Services.Data;
    using Syncfusion.Maui.Core.Hosting;

    /// <summary>
    /// The maui program.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Creates maui app.
        /// </summary>
        /// <returns>A MauiApp</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureAutoFonts()
                .UseMauiCommunityToolkitCore()
                .UseMauiCommunityToolkit(static options =>
                {
                    options.SetShouldUseStatusBarBehaviorOnAndroidModalPage(true);
                })
                .ConfigureSyncfusionCore();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            Type[] inputElement = [typeof(Entry), typeof(Editor)];
            builder.InputElementBorder(removeBorder: true, args: inputElement);
            builder.UseLibraryToolkit();
            RegisterEssentials(builder);
            RegisterNavigation(builder);
            builder.RegisterSingletonServices(typeof(ISingletonDependency));
            builder.RegisterSingletonServices(typeof(ITransientDependency));
            RegisterDependencies(builder);
            builder.InitializeServiceHelper();

            AppPreferences.UseBiometrics = true;
            InitializeDataSync(builder);

#if DEBUG
            builder.Logging.AddDebug();
            ////if (Debugger.IsAttached)
            ////{
            //// For Testing Purposes, we can override the Application Language
            //// Do these, before any other stuff is done
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture; // new System.Globalization.CultureInfo("en-US");

#endif

            MauiApp app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize();
            }

            return app;
        }

        /// <summary>
        /// Initializes data sync.
        /// </summary>
        /// <param name="builder">The builder.</param>
        static void InitializeDataSync(in MauiAppBuilder builder)
        {

            builder.Services.AddScoped<IDbInitializer, DbContextInitializer>();

            // Todo: We just return from here, if the Setup and Initalization of the Data Sync is not yet ready, to avoid having a broken implementation in the sample. Once we have a working implementation, we can remove the return statement and implement the data sync properly.
            // return;

            // 1. Setup the HTTP Client pointing to your server
            //var clientOptions = new HttpClientOptions
            //{
            //    // HttpEndpoint = new Uri("https://YOUR-SERVER-URL.com")
            //    Endpoint = new Uri("https://purse-datasync-service.azurewebsites.net")
            //};

            //builder.Services.AddSingleton(new DatasyncServiceClient<Transaction>(clientOptions));

            //// 2. Setup the Local Offline Database (SQLite)
            //string dbPath = Path.Combine(FileSystem.AppDataDirectory, "purse.db");
            //builder.Services.AddDbContext<LocalDbContext>(options =>
            //    options.UseSqlite($"Data Source={dbPath}"));

            //// 3. Register your Sync Service
            //builder.Services.AddTransient<SyncService>();

            // Info: In a real app, you would likely have a more complex setup for the SyncService, including handling authentication, conflict resolution, and more. The above code is a simplified example to illustrate the basic setup.
            string dbFolder = FileSystem.AppDataDirectory;
            if (!System.IO.Directory.Exists(dbFolder))
            {
                System.IO.Directory.CreateDirectory(dbFolder);
            }
            string dbPath = System.IO.Path.Combine(dbFolder, "purse.db");

            //Debug.WriteLine($"The Database for this App is located at: {dbPath}");
            builder.Services.AddDbContext<LocalDbContext>(options =>
                options.UseSqlite(
                    $"Data Source={dbPath}",
                    x => x.MigrationsAssembly(typeof(LocalDbContext).Assembly.FullName)));
        }

        /// <summary>
        /// Registers the navigation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void RegisterNavigation(in MauiAppBuilder builder)
        {
            builder.Services.AddTransient<Purse.ViewModel.TransactionHistoryCollectionViewModel>();
            builder.Services.AddTransient<Purse.View.TransactionHistoryCollectionView>();

            builder.Services.AddTransient<Purse.ViewModel.TransactionDetailViewModel>();
            builder.Services.AddTransient<Purse.View.TransactionDetailView>();

            builder.Services.AddTransient<Purse.ViewModel.CategoryCollectionViewModel>();
            builder.Services.AddTransient<Purse.View.CategoryCollectionView>();
            builder.Services.AddTransient<Purse.ViewModel.CategoryDetailViewModel>();
            builder.Services.AddTransient<Purse.View.CategoryDetailView>();

            builder.Services.AddTransient<Purse.ViewModel.VendorsCollectionViewModel>();
            builder.Services.AddTransient<Purse.View.VendorsCollectionView>();
            builder.Services.AddTransient<Purse.ViewModel.VendorDetailViewModel>();
            builder.Services.AddTransient<Purse.View.VendorDetailView>();

            builder.Services.AddTransient<Purse.ViewModel.IncomeSourcesCollectionViewModel>();
            builder.Services.AddTransient<Purse.View.IncomeSourcesCollectionView>();
            builder.Services.AddTransient<Purse.ViewModel.IncomeSourceDetailViewModel>();
            builder.Services.AddTransient<Purse.View.IncomeSourceDetailView>();

            // builder.Services.AddSingleton<AuthenticationViewModel>();
            // builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddSingletonWithShellRoute<AboutView, AboutViewModel>();
            builder.Services.AddSingletonWithShellRoute<ControlGalleryView, ControlGalleryViewModel>();
#if WINDOWS
            builder.Services.AddSingletonWithShellRoute<View.Desktop.MainView, MainViewModel>();
#else
            builder.Services.AddSingletonWithShellRoute<View.MainView, MainViewModel>();
#endif


            // builder.Services.AddTransient<MainView>();

            // this does not make sense, as we create the AppShell in the App constructor of CreateWindow
            // builder.Services.AddSingleton<AppShellViewModel>();
            // builder.Services.AddTransient<AppShell>();
        }

        /// <summary>
        /// Registers the essentials.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void RegisterEssentials(in MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDeviceDisplay>(DeviceDisplay.Current);
            builder.Services.AddSingleton<IDeviceInfo>(DeviceInfo.Current);
            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
            builder.Services.AddSingleton<IFileSystem>(FileSystem.Current);
            builder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);
            // builder.Services.AddSingleton<IBadge>(Badge.Default);
            // builder.Services.AddKeyedSingleton<ISpeechToText, SpeechToTextImplementation>("Online");
            // builder.Services.AddKeyedSingleton<ISpeechToText, OfflineSpeechToTextImplementation>("Offline");
            // builder.Services.AddSingleton<ITextToSpeech>(TextToSpeech.Default);
        }

        /// <summary>
        /// Registers the dependencies.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void RegisterDependencies(in MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IBiometric>(provider => BiometricAuthenticationService.Default);
            builder.Services.AddSingleton<IWindowCreator, Services.WindowCreator>();
            //builder.Services.AddSingleton<ICloudService, Services.SQLiteAsyncService>();
            //builder.Services.AddSingleton<DataSeedingService>();
            //builder.Services.AddSingleton<LibraryTen.Services.DeviceOrientationService>();
        }
    }
}