namespace Purse.Messaging
{
    // using CommunityToolkit.Mvvm.Messaging.Messages;
    // using LibraryTen.Messaging;
    using LibraryTen.Messaging.Messages;

    public class CountPropertyChangedMessage : PropertyChangedMessage<int>
    {
        public CountPropertyChangedMessage(object sender, string? propertyName, int oldValue, int newValue)
            : base(sender, propertyName, oldValue, newValue)
        {

        }
    }

    public class ConversionCompletedMessage : ValueChangedMessage<decimal>
    {
        public ConversionCompletedMessage(decimal value)
            : base(value)
        {
        }
    }
}
