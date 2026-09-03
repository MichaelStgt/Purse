# Initial Migration
Add-Migration InitialCreate -Project Purse.Data -StartupProject Purse.Data.Migrator

# After Model base class or derived class changes
Add-Migration AddMeaningfullName -Project Purse.Data -StartupProject Purse.Data.Migrator

# Add Vendor and IncomeSource models
Add-Migration AddVendorAndIncomeSource -Project Purse.Data -StartupProject Purse.Data.Migrator

# Add Country model and replace Country string field with CountryId
Add-Migration AddCountryAndRefactorAddress -Project Purse.Data -StartupProject Purse.Data.Migrator

# Apply migrations to the database
Update-Database -Project Purse.Data -StartupProject Purse.Data.Migrator
