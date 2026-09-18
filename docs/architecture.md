# Purse: Application Architecture

## Data Layer: Local-First, Sync-Ready
- **Database:** SQLite via Entity Framework Core.
- **Base Entity:** All models inherit from `OfflineClientEntity`.
- **Sync Properties:** `Id` (Guid), `OwnerId` (string?), `LastModified` (DateTimeOffset), `IsDeleted` (bool).
- **Migration Strategy:** Migrations are strictly generated using the isolated `Purse.Data.Migrator` console application to prevent MAUI multi-targeting conflicts.

## Presentation Layer
- **Framework:** .NET MAUI 10.
- **MVVM Toolkit:** `CommunityToolkit.Mvvm`.
- **Data Synchronization (UI):** ViewModels communicate state changes (Update, Insert, Soft-Delete) via `CommunityToolkit.Mvvm.Messaging`. Collection Views must listen to these messages to update their `ObservableCollection` in-memory, avoiding redundant database queries.

## UI Components
- **Third-Party:** Syncfusion (`SfComboBox` used for complex data selection).
- **Custom Controls:** `SafePicker` implemented using the "Bridge Property" pattern (`SafeSelectedItem`) to prevent MAUI binding engine race conditions.