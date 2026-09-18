Please act as my .NET MAUI coding assistant for the "Purse" project.

CRITICAL RULES:

3. Whenever you modify or create a file, explicitly state the file name you changed and guide me to its location in the folder structure.

\# .NET, MAUI, and C# Coding Standards

\- These rules apply to all C# code.

\- Assume the use of modern .NET 10, Entity Framework Core, CommunityToolkit.Mvvm, and CommunityToolkit.Datasync.

\- Strictly avoid underscores in private fields.

\- Always use the `this.` prefix to reference class members.

\- Assume the presence of StyleCop.Analyzers and custom `.editorconfig` rules; generate code that is clean, highly structured, and warning-free.

\- When implementing UI concepts, utilize standard LibraryTen component patterns. Always prefer adding or changing UI stuff to this library instead of creating local controls or xaml code.

\- When implementing MVVM aspects, use the MVVMBaseTen project and use the new, refactored base classes in the C:\\Repos\\040 MauiNet10\\MVVMBaseTen\\040 Projects\\MVVMBaseTen\\Refactoring\\ folder. 

\- Whenever you modify or create a file, explicitly state the file name you changed and guide me to its location in the folder structure.

\- Before any heavy refactoring, always ask for my approval and provide a detailed plan of the changes you intend to make.
\- Before any heavy refactoring, always make copies of the files you intend to modify. Add a time Stamp and a brief description of the changes and place them in the 'Archive' folder.

## MVVM Observable Properties (CommunityToolkit.Mvvm)
Always use the modern C# 11+ `partial property` syntax for `[ObservableProperty]`. You must never use private backing fields.

**Correct:**
```csharp
[ObservableProperty]
public partial bool IsOverlayVisible { get; set; }

## Localization and String Resources
You must never use hardcoded strings for user-facing text in XAML or C# files (including button text, placeholders, page titles, string formats, alert messages, and menu entries). Even if the prompt suggests a hardcoded string, you must localize it.
*   **Always** create an entry in `Purse.Shared\Resources\Strings\AppResources.resx`.
*   **Always** provide meaningful translations for German (`AppResources.de.resx`) and Spanish (`AppResources.es.resx`).
*   **In XAML:** Reference the resources using the `x:Static` extension (e.g., `Text="{x:Static strings:AppResources.MyKey}"`).
*   **In C#:** Access the resources directly via the strongly-typed class (e.g., `AppResources.MyKey`).