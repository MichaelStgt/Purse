// <company file="AboutViewModel.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </company>

namespace Purse.ViewModel
{
    using System.Reflection;
    using Purse.Resources.Strings;

    /// <summary>
    /// Defines the <see cref="AboutViewModel" />.
    /// </summary>
    public partial class AboutViewModel : ObservableViewModel
    {
        #region Constructors

        // Todo: Set the Project Meatadata like the copyright, company and version in the csproj file and read it from there. See https://learn.microsoft.com/en-us/dotnet/core/tools/csproj#metadata-elements for more information.
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        public AboutViewModel()
        {
            // this.DataCreatorService = dataCreatorService;

            this.Title = AppResources.About;
            Task.Run(async () =>
            {
                await this.SetAssemblyAttributes();
                this.AssemblyMetaAttributes();

                // this.OnPropertyChanged(nameof(this.AssemblyMetaAttributes));
            });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the AppInfoName.
        /// </summary>
        [ObservableProperty]
        public partial string? AppInfoName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Company.
        /// </summary>
        [ObservableProperty]
        public partial string? Company
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Copyright.
        /// </summary>
        [ObservableProperty]
        public partial string? Copyright
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Version.
        /// </summary>
        [ObservableProperty]
        public partial string? Version
        {
            get; set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Assemblies meta attributes.
        /// </summary>
        public void AssemblyMetaAttributes()
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(Attribute), false);
            attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyMetadataAttribute), false);

            foreach (AssemblyMetadataAttribute a in attributes)
            {
                if (a.Key == "Microsoft.Maui.ApplicationModel.AppInfo.SelectedColor")
                {
                    // here, the correct Title of the App is determined
                    Debug.WriteLine(a.Value);
                }

                if (a.Key == "Microsoft.Maui.ApplicationModel.AppInfo.PublisherName")
                {
                    Debug.WriteLine(a.Value);
                }

                if (a.Key == "Microsoft.Maui.ApplicationModel.AppInfo.Version")
                {
                    Debug.WriteLine(a.Value);
                }

                // Microsoft.Maui.ApplicationModel.AppInfo.Version
                Debug.WriteLine(a.Key);
            }

            for (int i = 0; i < attributes.Length - 1; i++)
            {
                Debug.WriteLine(attributes[i]);
            }

            if (attributes.Length > 0)
            {
                AssemblyMetadataAttribute titleAttribute = (AssemblyMetadataAttribute)attributes[0];

                // if (titleAttribute.Title != string.Empty) return titleAttribute.Title;
            }
        }

        /// <summary>
        /// The SetAssemblyAttributes.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task SetAssemblyAttributes()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            if (attributes.Length > 0)
            {
                var copyright = (AssemblyCopyrightAttribute)attributes[0];
                string s = "© " + copyright.Copyright;
                this.Copyright = copyright.Copyright != null ? s : "Copyright Attribute not set.";
                // this.Copyright = copyright.Copyright ?? "Copyright Attribute not set.";
            }

            attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

            if (attributes.Length > 0)
            {
                var company = (AssemblyCompanyAttribute)attributes[0];
                this.Company = company.Company ?? "Copyright Attribute not set.";
            }

            attributes = assembly.GetCustomAttributes(typeof(AssemblyVersionAttribute), false);

            if (attributes.Length > 0)
            {
                // AssemblyVersionAttribute version = (AssemblyVersionAttribute)attributes[0];
                this.Version = ((AssemblyVersionAttribute)attributes[0]).Version ?? "Version Attribute not set.";
            }

            attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyMetadataAttribute), false);
            var attribute = attributes.Cast<AssemblyMetadataAttribute>().FirstOrDefault(w => w.Key == "Microsoft.Maui.ApplicationModel.AppInfo.SelectedColor");

            if (attribute != null && attribute.Value != null)
            {
                this.AppInfoName = attribute.Value;
            }

            attribute = attributes.Cast<AssemblyMetadataAttribute>().FirstOrDefault(w => w.Key == "Microsoft.Maui.ApplicationModel.AppInfo.Version");

            if (attribute != null && attribute.Value != null)
            {
                this.Version = attribute.Value;
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}