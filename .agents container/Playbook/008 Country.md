
TASK:
We have the Country Field in both the Vendor and IncomeSource models. We need to ensure that the Country field is properly defined and used in both models.
For this, I think a prompt table would be helpful to clarify the requirements and ensure consistency across both models.
We need to update our core data models. 
Please generate the code for the following code files:

1. Create a new `Country` model inheriting from `OfflineClientEntity`. 
It should include properties for `Name` and `Description`. This will be used for both the `Vendor` and `IncomeSource` models as a prompt/selection.

2. On the first Application start, this `Country` model should be populated with a list of countries.
This can be done using a seed method in the database context. As I don't want to have hard coded values to be filled in the database, we can use the same approach we used to fill in the Categories table, which is to have a `Country` resource file in the `Purse.Shared` project and read from it during the seeding process.

3. Update the `Vendor` and `IncomeSource` models to either reference the `Country` model instead of using a string for the country field, or, use a CountryID field holding a reference to the ID of the Selected Country later. This will ensure that both models are consistent and use the same data source for countries.