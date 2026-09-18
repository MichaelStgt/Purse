// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#undef OFFLINE_SYNC_ENABLED

namespace Purse.Data.Services;

using Microsoft.EntityFrameworkCore;
using Purse.Shared.Model;
using Purse.Shared.Resources.Strings;
using System.Resources;
using System.Collections;
using System.Globalization;

/// <summary>
/// Use this class to initialize the database.
/// </summary>
/// <param name="context">The context for the database.</param>
public class DbContextInitializer(LocalDbContext context)
    : IDbInitializer
{
    public void Initialize()
    {
        context.Database.Migrate();

        if (!context.Categories.Any())
        {
            var incomeMgr = new ResourceManager($"{typeof(IncomeCategories).Namespace}.{nameof(IncomeCategories)}", typeof(Category).Assembly);
            var incomeSet = incomeMgr.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            if (incomeSet != null)
            {
                foreach (DictionaryEntry entry in incomeSet)
                {
                    string id = entry.Key?.ToString() ?? Guid.NewGuid().ToString("N");
                    string defaultName = entry.Value?.ToString() ?? string.Empty;
                    context.Categories.Add(new Category
                    {
                        Id = id,
                        Name = defaultName,
                        IsIncome = true,
                        MonthlyBudget = 0m,
                        IsDefault = false
                    });
                }
            }

            var expenseMgr = new ResourceManager($"{typeof(ExpenseCategories).Namespace}.{nameof(ExpenseCategories)}", typeof(Category).Assembly);
            var expenseSet = expenseMgr.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            if (expenseSet != null)
            {
                foreach (DictionaryEntry entry in expenseSet)
                {
                    string id = entry.Key?.ToString() ?? Guid.NewGuid().ToString("N");
                    string defaultName = entry.Value?.ToString() ?? string.Empty;
                    context.Categories.Add(new Category
                    {
                        Id = id,
                        Name = defaultName,
                        IsIncome = false,
                        MonthlyBudget = 0m,
                        IsDefault = false
                    });
                }
            }

            context.SaveChanges();
        }

        if (!context.Countries.Any())
        {
            var countryMgr = new ResourceManager($"{typeof(Countries).Namespace}.{nameof(Countries)}", typeof(Category).Assembly);
            var countrySet = countryMgr.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            if (countrySet != null)
            {
                foreach (DictionaryEntry entry in countrySet)
                {
                    string id = entry.Key?.ToString() ?? Guid.NewGuid().ToString("N");
                    string defaultName = entry.Value?.ToString() ?? string.Empty;
                    context.Countries.Add(new Country
                    {
                        Id = id,
                        Name = defaultName
                    });
                }
            }

            context.SaveChanges();
        }

        if (!context.Vendors.Any())
        {
            try
            {
                using var stream = Microsoft.Maui.Storage.FileSystem.OpenAppPackageFileAsync("german_vendors.csv").GetAwaiter().GetResult();
                using var reader = new System.IO.StreamReader(stream);
                var isHeader = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (isHeader)
                    {
                        isHeader = false;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(';');
                    if (parts.Length >= 6)
                    {
                        context.Vendors.Add(new Vendor
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            Name = parts[0],
                            Street = parts[1],
                            City = parts[2],
                            PostalCode = parts[3],
                            Description = $"Phone: {parts[4]}, Website: {parts[5]}"
                        });
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to seed vendors: {ex.Message}");
            }
        }
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);

        if (!await context.Categories.AnyAsync(cancellationToken))
        {
            var incomeMgr = new ResourceManager($"{typeof(IncomeCategories).Namespace}.{nameof(IncomeCategories)}", typeof(Category).Assembly);
            var incomeSet = incomeMgr.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            if (incomeSet != null)
            {
                foreach (DictionaryEntry entry in incomeSet)
                {
                    string id = entry.Key?.ToString() ?? Guid.NewGuid().ToString("N");
                    string defaultName = entry.Value?.ToString() ?? string.Empty;
                    context.Categories.Add(new Category
                    {
                        Id = id,
                        Name = defaultName,
                        IsIncome = true,
                        MonthlyBudget = 0m,
                        IsDefault = false
                    });
                }
            }

            var expenseMgr = new ResourceManager($"{typeof(ExpenseCategories).Namespace}.{nameof(ExpenseCategories)}", typeof(Category).Assembly);
            var expenseSet = expenseMgr.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            if (expenseSet != null)
            {
                foreach (DictionaryEntry entry in expenseSet)
                {
                    string id = entry.Key?.ToString() ?? Guid.NewGuid().ToString("N");
                    string defaultName = entry.Value?.ToString() ?? string.Empty;
                    context.Categories.Add(new Category
                    {
                        Id = id,
                        Name = defaultName,
                        IsIncome = false,
                        MonthlyBudget = 0m,
                        IsDefault = false
                    });
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Countries.AnyAsync(cancellationToken))
        {
            var countryMgr = new ResourceManager($"{typeof(Countries).Namespace}.{nameof(Countries)}", typeof(Category).Assembly);
            var countrySet = countryMgr.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            if (countrySet != null)
            {
                foreach (DictionaryEntry entry in countrySet)
                {
                    string id = entry.Key?.ToString() ?? Guid.NewGuid().ToString("N");
                    string defaultName = entry.Value?.ToString() ?? string.Empty;
                    context.Countries.Add(new Country
                    {
                        Id = id,
                        Name = defaultName
                    });
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Vendors.AnyAsync(cancellationToken))
        {
            try
            {
                using var stream = await Microsoft.Maui.Storage.FileSystem.OpenAppPackageFileAsync("german_vendors.csv");
                using var reader = new System.IO.StreamReader(stream);
                var isHeader = true;
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync(cancellationToken);
                    if (isHeader)
                    {
                        isHeader = false;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(';');
                    if (parts.Length >= 6)
                    {
                        context.Vendors.Add(new Vendor
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            Name = parts[0],
                            Street = parts[1],
                            City = parts[2],
                            PostalCode = parts[3],
                            Description = $"Phone: {parts[4]}, Website: {parts[5]}"
                        });
                    }
                }
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to seed vendors: {ex.Message}");
            }
        }
    }
}


