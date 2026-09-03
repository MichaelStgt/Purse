namespace Purse.Controls;

using SQLite;

public partial class NumericEntry : ContentView
{
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(NumericEntry), 0.0, BindingMode.TwoWay);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public NumericEntry()
    {
        InitializeComponent();
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ((Entry)sender).Text = e.NewTextValue;
            return;
        }

        // Ignore empty space
        // 1. Allow temporary invalid states like empty or just a decimal point
        if (/*string.IsNullOrWhiteSpace(e.NewTextValue) ||*/ e.NewTextValue == ".")
        {
            return;
        }

        // 2. Try to parse the input
        if (double.TryParse(e.NewTextValue, out double result))
        {
            this.Value = result;
        }
        else
        {
            // 3. Revert to old value if input is invalid (e.g., "1.2.3")
            ((Entry)sender).Text = e.OldTextValue;
        }
    }
}
