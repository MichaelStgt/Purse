namespace Purse.Controls;

using SQLite;

public partial class NumericEntry2 : ContentView
{
    public static readonly BindableProperty TextValueProperty =
        BindableProperty.Create(nameof(TextValue), typeof(string), typeof(NumericEntry), string.Empty, BindingMode.TwoWay,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var control = (NumericEntry2)bindable;
                if (control.InternalEntry.Text != (string)newVal)
                    control.InternalEntry.Text = (string)newVal;
            });

    public string TextValue
    {
        get => (string)GetValue(TextValueProperty);
        set => SetValue(TextValueProperty, value);
    }

    public NumericEntry2() => InitializeComponent();

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        // DO NOT ignore null/empty. Sending string.Empty triggers [Required] validation.
        TextValue = e.NewTextValue;
    }
}