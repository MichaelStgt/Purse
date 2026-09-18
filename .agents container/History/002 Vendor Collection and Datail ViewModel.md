TASK:
Now, generate the ViewModels for managing Vendors and Income Sources using the `CommunityToolkit.Mvvm.ComponentModel` and `CommunityToolkit.Mvvm.Input` and `MVVMBase.ViewModel` namespaces. 

Please create the following four ViewModels and guide me to their updated file locations:

1. `VendorsCollectionViewModel`: 
A collection ViewModel deriving from `ObservableCollectionViewModel<Vendor>`. 
Include an asynchronous `[RelayCommand]` to load ^vendors^ from the database, and a command to navigate to the detail page.
Use the 'ItemCollection' pattern for managing the collection. Please inspect the `ObservableCollectionViewModel<T>` class for guidance on how to implement this pattern. Ensure that the collection is properly initialized and that the load command populates it with data from the database.
2. `VendorDetailViewModel` deriving from 'ObservableDetailViewModel<Vendor>': 
A detail ViewModel containing an `[ObservableProperty]` for a single `Vendor`. Include a `[RelayCommand]` to save the vendor to the database. 
Please inspect the `ObservableDetailViewModel<T>` class for guidance on how to implement this pattern. 
3. `IncomeSourcesCollectionViewModel`: 
A collection ViewModel derived from `ObservableCollectionViewModel<IncomeSource>`. Include a load command and a navigation command.
4. `IncomeSourceDetailViewModel`: 
A detail ViewModel containing an `[ObservableProperty]` for a single `IncomeSource`. Include a save command.

Remember to strictly enforce the `this.` prefix for all class-level fields and methods within these ViewModels.