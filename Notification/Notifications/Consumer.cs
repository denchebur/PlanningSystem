using System;
using Apache.NMS;

namespace Notification.Notifications
{
    public class Consumer<T> : IConsumer<T>, IDisposable
    {
        private bool _isDispose = false;
        private readonly NotificationProperties _properties;
        private readonly IMessageMapper _messageMapper;
        private readonly IConnectionFactory _factory;

        public Consumer(NotificationProperties properties, IMessageMapper messageMapper, IConnectionFactory factory)
        {
            _properties = properties;
            _messageMapper = messageMapper;
            _factory = factory;
        }

        private bool Receive(out string message)
        {
            using (IConnection connection = _factory.CreateConnection())
            {
                connection.Start();
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue( _properties.QueueName))
                using (IMessageConsumer consumer = session.CreateConsumer(dest))
                { 
                    IMessage msg = consumer.Receive();
                    if (msg is ITextMessage)
                    {
                        ITextMessage txtMsg = msg as ITextMessage;
                        string body = txtMsg.Text;
                        message = body;
                        Console.WriteLine("rec");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
                    }
                }
                connection.Close();
            }
            message = null;
            return false;
        }
        
        public void StartListening()
        {
            while (!_isDispose && Receive(out var message))
            {
                OnMessage?.Invoke(_messageMapper.FromMessage<T>(message));
            }
        }

        public event Action<T> OnMessage;
        public void Dispose()
        {
            _isDispose = true;
        }
    }
}

// сделать через обсервер
// сделать ивент и чтоб автореджект подписался на этот ивент