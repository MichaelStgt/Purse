using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public partial class MyViewModel : ObservableValidator
{
    public ObservableCollection<ValidationResult> ValidationResults { get; } = new();

    // Tracks which ValidationResults belong to which property
    private readonly Dictionary<string, List<ValidationResult>> _resultsByProperty =
        new(StringComparer.Ordinal);

    public MyViewModel()
    {
        ErrorsChanged += OnErrorsChanged;
    }

    private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        // Usually e.PropertyName is set; handle null/empty defensively.
        if (string.IsNullOrWhiteSpace(e.PropertyName))
        {
            RebuildAllValidationResults();
            return;
        }

        var propertyName = e.PropertyName;

        // 1) Remove old results for this property
        if (_resultsByProperty.TryGetValue(propertyName, out var oldResults))
        {
            foreach (var vr in oldResults)
                ValidationResults.Remove(vr);
        }

        // 2) Add current results for this property
        var newResults = GetErrors(propertyName)
            .OfType<string>() // ObservableValidator returns strings
            .Select(msg => new ValidationResult(msg, new[] { propertyName }))
            .ToList();

        if (newResults.Count == 0)
        {
            _resultsByProperty.Remove(propertyName);
            return;
        }

        _resultsByProperty[propertyName] = newResults;
        foreach (var vr in newResults)
            ValidationResults.Add(vr);
    }

    private void RebuildAllValidationResults()
    {
        ValidationResults.Clear();
        _resultsByProperty.Clear();

        // Re-query errors for all public instance properties
        foreach (var prop in this.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
        {
            var propertyName = prop.Name;

            var results = GetErrors(propertyName)
                .OfType<string>()
                .Select(msg => new ValidationResult(msg, new[] { propertyName }))
                .ToList();

            if (results.Count == 0)
                continue;

            _resultsByProperty[propertyName] = results;
            foreach (var vr in results)
                ValidationResults.Add(vr);
        }
    }
}