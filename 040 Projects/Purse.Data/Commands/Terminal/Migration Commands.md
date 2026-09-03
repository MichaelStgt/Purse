# Initial Migration (after reset)
dotnet ef migrations add InitialMigration --project "040 Projects\Purse.Data" --startup-project "040 Projects\Purse.Data.Migrator"

# Add Vendor and IncomeSource models
dotnet ef migrations add AddVendorAndIncomeSource --project "040 Projects\Purse.Data" --startup-project "040 Projects\Purse.Data.Migrator"

# Add Country model and replace Country string field with CountryId
dotnet ef migrations add AddCountryAndRefactorAddress --project "040 Projects\Purse.Data" --startup-project "040 Projects\Purse.Data.Migrator"

# Update/Apply migrations to the database
dotnet ef database update --project "040 Projects\Purse.Data" --startup-project "040 Projects\Purse.Data.Migrator"