using System.Configuration;
namespace Notification.Notifications
{
    public class NotificationProperties
    {
        public string QueueName { get; }
        public string BrokerUri { get; }
        public NotificationProperties()
        {
            var config = ConfigurationManager.AppSettings;
            BrokerUri = config["BrokerUri"];
            QueueName = config["QueueName"];
        }
    }
}
// перенести в контекст
