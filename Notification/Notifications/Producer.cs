using System;
using System.Collections.Generic;
using Apache.NMS;

namespace Notification.Notifications
{
    public class Producer : IProducer
    {
        private readonly NotificationProperties _properties;
        private readonly IConnectionFactory _factory;

        public Producer(IConnectionFactory factory, NotificationProperties properties)
        {
            _properties = properties;
            _factory = factory;
        }
        public void Send(string notify)
        {
            Console.WriteLine($"Adding message to queue topic: {_properties.QueueName}");
            using (IConnection connection = _factory.CreateConnection())
            {
                connection.Start();
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(_properties.QueueName))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    producer.Send(session.CreateTextMessage(notify));
                    Console.WriteLine($"Sent message " + notify);
                }
            }
        }
    }
}