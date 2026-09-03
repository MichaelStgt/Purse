using System.Runtime.CompilerServices;

namespace Purse.Controls
{
    public class SafePicker : Picker
    {
        public static readonly BindableProperty SafeSelectedItemProperty = BindableProperty.Create(
            nameof(SafeSelectedItem),
            typeof(object),
            typeof(SafePicker),
            null,
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var picker = (SafePicker)bindable;

                if (picker.ItemsSource != null && picker.SelectedItem != newValue)
                {
                    picker.SyncSelection();
                }

                //if (newValue != null && ((SafePicker)bindable).SelectedItem != newValue)
                //{
                //    ((SafePicker)bindable).SyncSelection();
                //}
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

        //protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);

        //    if (propertyName == ItemsSourceProperty.PropertyName)
        //    {
        //        // SCENARIO A: The items just arrived from the database.
        //        // We use Dispatcher so MAUI finishes completely rendering the native 
        //        // list behind the scenes BEFORE we try to select the item.
        //        if (this.ItemsSource != null)
        //        {
        //            if (this.SafeSelectedItem != null)
        //            {
        //                Dispatcher.Dispatch(SyncSelection);
        //            }
        //        }
        //    }
        //    else if (propertyName == SelectedItemProperty.PropertyName)
        //    {
        //        if (SelectedItem != null)
        //        {
        //            // SCENARIO B: The user physically picked a valid item from the UI.
        //            // We sync it back to our Safe property.
        //            if (SafeSelectedItem != SelectedItem)
        //            {
        //                SafeSelectedItem = SelectedItem;
        //            }
        //        }
        //        else if (SelectedItem == null && SafeSelectedItem != null)
        //        {
        //            // SCENARIO C: MAUI forcefully nulled the native picker 
        //            // because of a lifecycle event. We fight back and re-apply our safe value.
        //            if (this.ItemsSource != null)
        //            {
        //                Dispatcher.Dispatch(SyncSelection);
        //            }
        //        }
        //    }
        //}

        //private void SyncSelection()
        //{
        //    if (SafeSelectedItem != null && ItemsSource != null)
        //    {
        //        // Verify the item actually exists in the newly loaded list
        //        var items = ItemsSource.Cast<object>().ToList();

        //        if (items.Contains(SafeSelectedItem) && SelectedItem != SafeSelectedItem)
        //        {
        //            // Re-assert the selection back to the native control
        //            SelectedItem = SafeSelectedItem;
        //        }
        //    }
        //}
    }
}