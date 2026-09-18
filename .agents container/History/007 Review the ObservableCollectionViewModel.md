Task
Review the ObservableCollectionViewModel<TModel>
When using the ObservableCollectionViewModel Base class, we override the logic regarding the SelectedItem in derived objects.
When I look at the base model, the OnSelectedItemChangedImplementationAsync calls the OnSelectedItemChangedImplementation. This method does nothing. Why is there at all?
We could either just simply remove it, or, as we agree to use a strict naming of the ViewModel and corresponding view, Implement navigation already in the base class.