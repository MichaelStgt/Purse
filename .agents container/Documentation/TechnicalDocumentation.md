# Technical Documentation

This document provides a comprehensive technical overview of the architectural patterns, navigation life cycles, validation schemes, and project structures implemented in the solution.

---

## 1. Solution Architecture & Module Structure

The solution is divided into clean, decoupled layers to separate the UI and business application logic from the MVVM framework base classes:

```
                  ┌──────────────────────┐
                  │      Purse (App)     │
                  └──────────┬───────────┘
                             │ (References)
                             ▼
┌──────────────┐  ┌──────────────────────┐  ┌──────────────┐
│  Purse.Data  ├─►│     Purse.Shared     │◄─┤ MVVMBaseTen  │
└──────────────┘  └──────────────────────┘  └──────────────┘
```

*   **`Purse`**: The main .NET MAUI application containing pages (XAML), custom controls (`LabeledContentView`, `ExtendedEntry`), and application-specific ViewModels (`VendorDetailViewModel`, etc.).
*   **`Purse.Shared`**: Holds data models (`Vendor.cs`, `IncomeSource.cs`, `Country.cs`) derived from `OfflineClientEntity`. Both `Purse` and `Purse.Data` reference this project.
*   **`Purse.Data`**: Handles database interactions using EF Core with SQLite, providing migration and local persistence service layers.
*   **`MVVMBaseTen`**: The core framework library. It contains generic interfaces (`IEditableDetailViewModel<TModel>`), base view model implementations (`ObservableViewModel`, `ObservableDetailViewModel<TModel>`), and navigation helpers. Crucially, it does **not** reference the main application (`Purse`), remaining entirely decoupled.

---

## 2. Decoupled Navigation Interception (Soft Block)

To ensure that data entered on detail screens is not lost and is properly validated, the app implements a Shell-level navigation interceptor that communicates with ViewModels via the decoupled `IEditableDetailViewModel` interface.

### Sequence diagram of POP Navigation Interception:

```
[User / Hardware Back] ──► AppShell (OnNavigating)
                                │
                        [Is POP Navigation?]
                                │
            Get currentPage.BindingContext as IEditableDetailViewModel
                                │
                 IsNavigatingBackApproved == true?
                     ┌──────────┴──────────┐
               Yes (True)               No (False)
                     │                     │
               Allow Pop Navigation   args.Cancel()
                                           │
                                    editableVm.Validate()
                                           │
                                    HasErrors == true?
                                ┌──────────┴──────────┐
                          Yes (True)               No (False)
                                │                     │
                         DisplayAlertAsync        editableVm.Save()
                         "Discard changes?"           │
                       ┌────────┴────────┐      IsNavigatingBackApproved = true
                  Discard               Stay          │
                     │                   │       GoToAsync("..")
             IsNavigatingBackApproved=true Wait       │
                     │                               Pop Allowed
               GoToAsync("..")
                     │
                Pop Allowed
```

### Key Components:
1.  **`IEditableDetailViewModel` Interface**: Declares state properties (`IsNavigatingBackApproved`, `CanGoBack`, `HasErrors`) and operations (`Validate()`, `SaveCommandImplementation()`, `CancelCommand`).
2.  **`ObservableDetailViewModel<TModel>.Commands.cs`**: Implements a shared virtual `CancelAsync` command:
    ```csharp
    [RelayCommand]
    public virtual async Task CancelAsync()
    {
        this.IsNavigatingBackApproved = true;
        await Shell.Current.GoToAsync("..");
    }
    ```
3.  **`AppShell.xaml.cs`**: Intercepts POP navigation in `OnNavigating` to check if validation is required. If errors exist, it blocks the navigation and presents a soft block alert dialog offering the user options to "Stay and Fix" or "Discard and Go Back".

---

## 3. Dynamic Validation & Localization

The detail input forms resolve property display titles and validation error messages dynamically using localized resource bundles, avoiding hardcoded string values:

### Data Annotation Mapping:
ViewModels annotate property properties with standard DataAnnotations:
```csharp
[ObservableProperty]
[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
[NotifyDataErrorInfo]
[Required(ErrorMessageResourceName = nameof(ValidationResources.RequiredErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
[MinLength(1, ErrorMessageResourceName = nameof(ValidationResources.MinLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
[MaxLength(100, ErrorMessageResourceName = nameof(ValidationResources.MaxLengthErrorMessage), ErrorMessageResourceType = typeof(ValidationResources))]
[Display(Name = nameof(AppResources.Name), ResourceType = typeof(AppResources))]
public partial string VendorName { get; set; } = string.Empty;
```

*   **`AppResources.resx` / `AppResources.de.resx`**: Localized resource dictionaries containing UI field titles, placeholders, and error dialog copy.
*   **`ValidationResources.resx`**: Holds parameterized validation messages (e.g. `RequiredErrorMessage` -> `"{0} is required."`) where `{0}` is replaced dynamically by the property's localized display title.
*   **`LabeledContentView`**: Custom control that resolves validation errors from `INotifyDataErrorInfo` automatically, mapping visual state and drawing red border boundaries when the bound property fails validation rules.
