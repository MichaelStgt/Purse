// <copyright file="MaterialSymbolsOutlinned.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace HourTracker.Services
{
    using System.Globalization;
    using MVVMBase.Abstractions;

    /// <summary>
    /// The data seeding service.
    /// </summary>
    public class DataSeedingService
    {
        #region Fields

        ///// <summary>
        ///// The current.
        ///// </summary>
        //private static DataSeedingService? current;

        ///// <summary>
        ///// Gets the current.
        ///// </summary>
        ///// <value>A DataSeedingService?</value>
        //public static DataSeedingService? Current
        //{
        //    get
        //    {
        //        if (current == null)
        //        {
        //            current = new DataSeedingService();
        //        }

        //        return current;
        //    }
        //}

        /// <summary>
        /// Gets the cloud service.
        /// </summary>
        ICloudService cloudService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSeedingService"/> class.
        /// </summary>
        /// <param name="service">The service<see cref="ICloudService"/>.</param>
        public DataSeedingService(ICloudService service)
        {
            this.cloudService = service;
        }

        #endregion

        #region Methods



        /// <summary>
        /// Load the currencies.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task LoadCurrencies()
        {
            ICloudTable<CurrencyModel> currencyTable = this.cloudService!.GetTable<CurrencyModel>();
            int currencyCount = await currencyTable.CountItemsAsync();

            if (currencyCount > 0)
            {
                Debug.WriteLine($"Skip loading Currencies, they are already loaded to table CurrencyModel");
                return;
                // await currencyTable.DeleteAllItemsAsync();
            }

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            var currencies = new List<CurrencyModel>();

            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("currencycodes.csv");
                using var reader = new StreamReader(stream);

                // Skip header line
                await reader.ReadLineAsync();
                int i = 0;

                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var values = line.Split(';');
                    // Ignore, that there is no column for the WithdrawalDate, as there is no value in the table
                    // if it does not exist.
                    if (values.Length > 10)
                    {
                        Debug.WriteLine($"Data entry in csv file is invalid for country/region {values[0].Trim()}. Value is to long");
                    }
                    else
                        if (values.Length < 5)
                        {
                            Debug.WriteLine($"Data entry in csv file is invalid for country/region {values[0].Trim()}. Value is to short");
                        }
                        else if (values.Length >= 5)
                        {
                            // Use the Camel Case notation for the Entity names.
                            string entityName = textInfo.ToTitleCase(values[0].Trim().ToLower());
                            // Debug.WriteLine($"Entity SelectedColor {entityName}.");
                            string symbol = values[2].Trim();
                            CurrencyModel cm = new CurrencyModel();

                            cm.Id = Guid.NewGuid().ToString();
                            cm.Entity = entityName; // values[0].Trim();
                            cm.Name = values[1].Trim();
                            cm.AlphabeticCode = values[2].Trim();
                            cm.NumericCode = values[3].Trim();
                            cm.Symbol = symbol;

                            // Skip values where a WithdrawalDate is defined
                            if (values.Length >= 6 && values[5] != string.Empty)
                            {
                                continue;
                            }

                            if (values.Length >= 7 && values[6] != string.Empty)
                            {
                                cm.Symbol = values[6].Trim();
                            }

                            // if (values.Length == 8)
                            if (values.Length >= 8 && values[7] != string.Empty)
                            {
                                int.TryParse(values[7].Trim(), out int priority);
                                cm.Priority = priority;
                            }

                            if (values.Length >= 9 && values[8] != string.Empty)
                            {

                                cm.MinorUnit = values[8].Trim();
                            }

                            if (values.Length >= 10 && values[9] != string.Empty)
                            {
                                int.TryParse(values[9].Trim(), out int numberToBasic);
                                cm.NumberToBasic = numberToBasic;
                            }
                            currencies.Add(cm);
                        }
                        else
                        {
                            Debug.WriteLine($"Data entry in csv file is invalid for country/region {values[0].Trim()}");
                        }
                }

                // Use InsertAllAsync for better performance
                if (currencies.Any())
                {
                    var sortedAndDistinctCurrencies = currencies
                        .OrderByDescending(c => c.Priority)
                        .DistinctBy(db => db.AlphabeticCode)
                        .Select(g => g)
                        .OrderByDescending(db => db.Priority)
                        .ThenBy(tb => tb.AlphabeticCode);

                    sortedAndDistinctCurrencies.ForEach((c) =>
                    {
                        c.ItemOrder = i;
                        i++;
                    });

                    // bool success = await currencyTable.InsertAllAsync(currencies);
                    bool success = await currencyTable.InsertAllAsync(sortedAndDistinctCurrencies);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error loading currencies from CSV: {ex.Message}");
            }
        }

        #endregion
    }
}
