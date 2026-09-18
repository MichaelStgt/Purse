\# Antigravity Agent Instructions



You are the execution engine for the "Purse" application. Your primary directive is to write clean, maintainable, and strictly formatted C# and XAML code based on the architectural guidelines defined by the engineering team.



\## Core Directives

1\. \*\*No Assumptions:\*\* If a structural change is required outside the immediate scope of your prompt, pause and ask for confirmation.

2\. \*\*Global Namespaces:\*\* Exclusively use .NET MAUI 10 global namespaces (`xmlns="http://schemas.microsoft.com/dotnet/maui/global"`). Assume standard namespaces (`Model`, `View`, `ViewModel`, `Controls`) are handled globally in `GlobalXmlns`.

3\. \*\*Strict Naming Conventions:\*\*

&#x20;  - Base ViewModel: `ObservableViewModel`

&#x20;  - Collection Views: `\[ModelName]\[Meaning]CollectionView` / `\[ModelName]\[Meaning]CollectionViewModel`

&#x20;  - Detail Views: `\[ModelName]\[Meaning]DetailView` / `\[ModelName]\[Meaning]DetailViewModel`

4\. \*\*Zero Hardcoded Strings:\*\* All UI text must be referenced from `AppResources.resx` (e.g., `Text="{x:Static AppResources.Title}"`).

5\. \*\*Clean XAML:\*\* Do not use inline styling (colors, margins, padding). All visual styling is handled globally in `Styles.xaml`.

6\. \*\*Singular Namespaces:\*\* Use `Purse.Model`, `Purse.View`, and `Purse.ViewModel` (No pluralization).

