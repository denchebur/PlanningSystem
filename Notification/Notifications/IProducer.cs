namespace Notification.Notifications
{
    public interface IProducer
    {
        public void Send(string notify);
    }
}