Please act as my .NET MAUI coding assistant for the "Purse" project. 

TASK:
We need to update our core data models. Please generate the code for the following files and tell me where to place them:

1. Create a new `Vendor` model inheriting from `OfflineClientEntity`. 
It should include properties for `Name` and `Description`. Add Address fields (`Street`, `City`, `State`, `PostalCode`, `Country`). This is used for expenses.

2. Create a new `IncomeSource` model inheriting from `OfflineClientEntity`. 
It should include properties for `Name` and `Description`. Add Address fields (`Street`, `City`, `State`, `PostalCode`, `Country`). This is used for income.

3. Run the appropriate Entity Framework Core migration commands to update the database schema with these new models.
4. Please add the commands to the `Purse.Data/Commands/Terminal/Migration Commands.md` file, so I can later see the history of migrations.
5. Please add the commands to the `Purse.Data/Commands/PackageManagerConsole/migration commands.md` file, so I can later see the history of migrations (this is only for completeness, as I still need to learn and understand the difference between command line commands and Package Manager Console commands).