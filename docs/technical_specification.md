# Purse App: Functional Requirements

## 1. Data Management & Synchronization
- **Local-First Architecture:** The application relies on a local SQLite database accessed via Entity Framework Core.
- **Entity Base:** All database models (including `Vendor`, `IncomeSource`, `Transaction`, and `TransactionLineItem`) inherit from the base entity class.
- **Soft Deletion:** Records are never permanently deleted from the local database. They are flagged with `IsDeleted = true` to facilitate future remote synchronization.
- **Sync Readiness:** Every entity must track its `Id` (Guid), `OwnerId` (string?), and `LastModified` (DateTimeOffset) timestamp.

## 2. UI & State Management
- **Asynchronous Data Loading:** Collection Views must load data asynchronously. To prevent MAUI binding race conditions, dropdowns and pickers use either the custom `SafePicker` control or Syncfusion components.
- **Real-Time UI Updates:** Instead of reloading the database on every `OnAppearing` event, the app uses `CommunityToolkit.Mvvm.Messaging`. When a Detail ViewModel saves, inserts, or soft-deletes an entity, it broadcasts a message. The corresponding Collection ViewModel intercepts this message and updates its `ObservableCollection` in memory.
- **Zero Hardcoded Text:** All user-facing strings, including validation errors and labels, must be referenced statically from the localized `AppResources` or specific category/country resource files (e.g., `Countries.resx`, `ExpenseCategories.resx`)[cite: 2].

## 3. Navigation & Routing
- **Reflection-Based Registration:** (Planned) The app utilizes a strict `*View` and `*ViewModel` naming convention[cite: 2]. This enables automatic, reflection-based dependency injection and route registration at startup, eliminating manual boilerplate in `MauiProgram.cs`[cite: 2].
- **Data Passing:** Navigation between Collection Views and Detail Views passes the specific entity ID or model via MAUI Shell's dictionary-based parameter passing.

## 4. Core Workflows
- **Master Data Management:** Users can view lists of master data like Vendors and Income Sources, add new entries, edit existing details, and soft-delete them[cite: 2].
- **Transactions:** Users create and manage transactions (and transaction split lines) that link to specific vendors or income sources, categorized utilizing the localized resource dictionaries[cite: 2].
