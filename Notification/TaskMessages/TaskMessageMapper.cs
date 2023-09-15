using System.IO;
using Newtonsoft.Json;
using Notification.Notifications;

namespace Notification.TaskMessages
{
    public class TaskMessageMapper : IMessageMapper
    {
        private readonly JsonSerializer _serializer = JsonSerializer.Create();
        public TaskMessage FromMessage<TaskMessage>(string message)
        {
            return JsonConvert.DeserializeObject<TaskMessage>(message); 
        }

        public string ToMessage<TaskMessage>(TaskMessage item)
        {
            return JsonConvert.SerializeObject(item);
        }
    }
}