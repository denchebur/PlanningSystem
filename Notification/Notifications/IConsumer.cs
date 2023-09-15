using System;

namespace Notification.Notifications
{
    public interface IConsumer<out T>
    { 
        void StartListening();
        event Action<T> OnMessage;
    }
}