# Developer Context: "Purse" Budget Tracking App

Welcome to the development environment of **Purse**, a premium, cross-platform budget-tracking application built with .NET MAUI. This document summarizes the application's goals, technical architecture, project structure, database models, synchronization design, and open development topics to establish a clear context for ongoing development.

---

## 1. Executive Summary & Core Objectives

**Purse** is designed as a "bread and butter" commercial app targeting the **Windows** and **Android** app stores. Its primary objective is to help users understand exactly where their money goes by offering secure, fast, and offline-first expense tracking with granular transactional details.

### Key Value Propositions
*   **Offline-First Operation:** The app works seamlessly without an internet connection, saving all data locally in an SQLite database and syncing with a secure cloud server automatically when a connection is available.
*   **Granular Spending Splits:** Unlike generic budgeting apps that only categorize entire purchases, *Purse* allows users to split a single transaction (e.g., a €100 grocery bill at Edeka) into distinct line items (e.g., €60 for groceries, €20 for alcohol, and €20 for leisure/hobbies), each with its own categories and tags.
*   **Predefined Vendor & Category Seeding:** Focuses initially on providing an out-of-the-box experience for European markets (especially Germany) with seeded discounters/supermarkets (Aldi Söd, Aldi Nord, Lidl, Edeka, REWE, dm, Rossmann, etc.) and core fixed/variable categories (Rent, Insurance, Electricity, Food, Leisure, Transport).
*   **Graphical Financial Dashboard:** Translates complex local relational data into dynamic charts (e.g., Syncfusion charts) showing category distributions and spending trends over time.

---

## 2. Technical Stack & Core Frameworks

The application relies on a modern, decoupled, and highly performant .NET stack:

| Technology Component | Framework / Library | Purpose |
| :--- | :--- | :--- |
| **Core Framework** | .NET MAUI (.NET 10 / 11 Preview) | Cross-platform application lifecycle & native UI rendering. |
| **Data Access Layer** | Entity Framework Core (EF Core) | Object-relational mapping (ORM) for both local SQLite and remote SQL Server. |
| **MVVM Architecture** | CommunityToolkit.Mvvm | Clean separation of UI and business logic (using source generators for commands and properties). |
| **Synchronization** | CommunityToolkit.Datasync (v8+) | Enterprise-grade offline-to-online data synchronization. |
| **UI Components** | CommunityToolkit.Maui & Syncfusion | Smooth status bars, expander views, and premium charting controls. |
| **Custom Codebase** | `LibraryTen` & `MVVMBaseTen` | Custom-developed developer libraries containing reusable authentication helpers (e.g., biometrics), navigation, and custom ViewModels. |

---

## 3. Project Architecture & Solution Structure

Due to limitations with running Entity Framework Core migration design tools directly inside platforms like Android, *Purse* uses a modular, decoupled project layout:

```
Purse/
├── 010 Assets/                  # Graphical assets, icons, and themes
├── 020 Documents/               # Requirements, references, and developer context
├── 030 Samples/                 # Reference projects (e.g. MauiEF, Datasync samples)
└── 040 Projects/
    ├── Purse.slnx               # Visual Studio Solution definition
    ├── Purse/                   # The Client Application (.NET MAUI Mobile & Desktop)
    ├── Purse.Data/              # Local DBContext, DB initializers, and repository services
    ├── Purse.Data.Migrator/     # Pure .NET console application used strictly to generate EF Core Migrations
    ├── Purse.Server/            # ASP.NET Core Web API backend linked to a SQL Server database
    └── Purse.Shared/            # Shared POCO models and Base Entities used by both Client and Server
```

---

## 4. Database Schema & Data Models

Synchronization using `CommunityToolkit.Datasync` (v8+) relies on Entity Framework Core on both client and server. The client models and server databases are unified through the shared library.

### The Unified Synchronization Base Class
To allow shared models to compile on the client while being accepted by ASP.NET Core `TableController<T>` endpoints on the server, the base entity implements standard sync metadata interfaces:

```csharp
namespace Purse.Shared.Models
{
    using CommunityToolkit.Datasync.Server.Abstractions;

    public abstract class OfflineClientEntity : BaseEntityTableData, ITableData, IEquatable<ITableData>
    {
        // String-based GUIDs are used to prevent ID collisions during offline creation across multiple devices
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public byte[] Version { get; set; } = Array.Empty<byte>();
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public bool Deleted { get; set; } = false;
    }
}
```

### Relational Domain Models
*Purse* models reflect a **One-to-Many** relationship between a purchase header and its split items:

```csharp
namespace Purse.Shared.Models
{
    // 1. Transaction Header
    public class Transaction : OfflineClientEntity
    {
        public string VendorName { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public double TotalAmount { get; set; }
    }

    // 2. Transaction Line Item (Splits)
    public class TransactionLineItem : OfflineClientEntity
    {
        // Foreign Key linking back to Transaction.Id
        public string TransactionId { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty; // Stored as comma-separated values
        public double Amount { get; set; }
    }
}
```

---

## 5. Offline-First Synchronization Workflow

Under `CommunityToolkit.Datasync` v8+, offline sync operates directly through local `OfflineDbContext` operations rather than older, obsolete SQLite store classes.

1.  **Local Database Configuration:**
    The client MAUI application inherits its `LocalDbContext` from `OfflineDbContext` (provided by `CommunityToolkit.Datasync.Client.Offline`), overriding `OnDatasyncInitialization` to bind remote database endpoints:
    ```csharp
    public class LocalDbContext : OfflineDbContext
    {
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionLineItem> LineItems => Set<TransactionLineItem>();

        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        protected override void OnDatasyncInitialization(DatasyncOfflineOptionsBuilder optionsBuilder)
        {
            // Binds client models to respective tables on the ASP.NET Core Web API Server
            optionsBuilder.UseServerTable<Transaction>("tables/transaction");
            optionsBuilder.UseServerTable<TransactionLineItem>("tables/transactionlineitem");
        }
    }
    ```
2.  **Order of Sync Operations:**
    To maintain relational integrity and prevent "orphan records" (e.g., line items uploaded before their parent transaction exists on the server), the client runs synchronization sequentially:
    *   **Step A (Local Save):** Save the `Transaction` header and its `TransactionLineItem` splits within a local EF database transaction to ensure atomicity.
    *   **Step B (Push Parent):** Push the local `Transaction` table up to the remote Web API.
    *   **Step C (Push Children):** Push the local `TransactionLineItem` table up.
    *   **Step D (Pull Remote):** Pull remote updates for both tables down to update the local dashboard.

---

## 6. Current Development Status & Identified Issues

### A. Dual Execution of `LoadTransactionsAsync` on Startup
*   **Symptom:** When launching the app, the data fetch command runs twice, generating redundant SQLite database queries.
*   **Root Cause:** In the view layer (`TransactionHistoryCollectionView.xaml.cs`), `OnAppearing` manually executes the load command:
    ```csharp
    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.ViewModel.LoadItemCollectionCommand.Execute(null);
    }
    ```
    This sets `IsBusy` (or `IsRefreshing`) to `true`. Because the `RefreshView` in XAML has its `IsRefreshing` property bound in `OneWay` mode:
    ```xml
    <RefreshView
        Command="{x:Binding LoadItemCollectionCommand}"
        IsRefreshing="{x:Binding IsBusy, Mode=OneWay}">
    ```
    Setting `IsBusy` programmatically triggers the `RefreshView` to start its visual spinner, which in turn calls its bound `Command` (the same `LoadItemCollectionCommand`), resulting in a second, redundant execution.
*   **Proposed Fix:** Modify either the binding mode, check if a load is already in progress within the ViewModel before running, or rely solely on `OnAppearing` or `RefreshView` to trigger the initial load without duplicate calls.

### B. Library Refactoring (MVVMBaseTen & LibraryTen)
*   Refactoring core ViewModel classes (`ObservableDetailViewModel<T>`, `ObservableCollectionViewModel<T>`) to provide standard, bug-free, and reusable synchronization and data loading hooks.
*   Resolving detail validation layers where `Sum(LineItems.Amount)` must exactly equal `Transaction.TotalAmount` before saving.

---

## 7. Next Steps & Development Roadmap

1.  **Resolve the Redundant Loading Bug:** Adjust the `RefreshView` and `OnAppearing` interactions in `TransactionHistoryCollectionView` to ensure data is fetched exactly once.
2.  **Refine Validation Rules:** Ensure that when a user creates split line items in `TransactionDetailViewModel`, save operations are blocked until total split amounts match the header amount.
3.  **Validate Migrations Pipeline:** Confirm that modifications to `Purse.Shared` flow correctly through the `Purse.Data.Migrator` project, generating correct SQLite schema modifications on local developer machines.
4.  **Implement Data Seeding:** Securely configure initial German Discounters/Supermarkets and category defaults in the offline DB context.
