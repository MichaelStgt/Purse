namespace Purse.Controls
{
    using Microsoft.Maui.Controls;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    namespace Purse.Controls
    {
        public class SafePicker2 : Picker
        {
            public static readonly BindableProperty SafeSelectedItemProperty = BindableProperty.Create(
                nameof(SafeSelectedItem),
                typeof(object),
                typeof(SafePicker2),
                null,
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var picker = (SafePicker2)bindable;

                    // PERFORMANCE FIX: Only sync if the ItemsSource is already loaded.
                    // This ignores the useless initial load trigger, but handles 
                    // programmatic updates from the ViewModel later in the app's lifecycle.
                    if (picker.ItemsSource != null && picker.SelectedItem != newValue)
                    {
                        picker.SyncSelection();
                    }
                });

            public object SafeSelectedItem
            {
                get => this.GetValue(SafeSelectedItemProperty);
                set => this.SetValue(SafeSelectedItemProperty, value);
            }

            protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == ItemsSourceProperty.PropertyName)
                {
                    // SCENARIO A: The items just arrived from the database.
                    if (this.ItemsSource != null && this.SafeSelectedItem != null)
                    {
                        this.Dispatcher.Dispatch(this.SyncSelection);
                    }
                }
                else if (propertyName == SelectedItemProperty.PropertyName)
                {
                    if (this.SelectedItem != null)
                    {
                        // SCENARIO B: The user physically picked a valid item.
                        if (this.SafeSelectedItem != this.SelectedItem)
                        {
                            this.SafeSelectedItem = this.SelectedItem;
                        }
                    }
                    else if (this.SelectedItem == null && this.SafeSelectedItem != null)
                    {
                        // SCENARIO C: MAUI forcefully nulled the native picker.
                        if (this.ItemsSource != null)
                        {
                            this.Dispatcher.Dispatch(this.SyncSelection);
                        }
                    }
                }
            }

            private void SyncSelection()
            {
                if (this.SafeSelectedItem != null && this.ItemsSource != null)
                {
                    var items = this.ItemsSource.Cast<object>().ToList();

                    if (items.Contains(this.SafeSelectedItem) && this.SelectedItem != this.SafeSelectedItem)
                    {
                        this.SelectedItem = this.SafeSelectedItem;
                    }
                }
            }
        }
    }
}
