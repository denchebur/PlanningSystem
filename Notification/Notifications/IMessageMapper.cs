namespace Notification.Notifications
{
    public interface IMessageMapper
    {
        T FromMessage<T>(string message);
        string ToMessage<T>(T item);
    }
}