using System.ServiceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Notification.Deserilizer;

namespace Notification.TaskMessages
{
    [MessageContract]
    [JsonConverter(typeof(MessageCoverter))]
    public abstract class TaskMessage
    {
        public TaskMessage(EventMessage param)
        {
            Event = param;
        }

        public TaskMessage()
        { }
        
        /// <summary>
        /// Represent operation event
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public EventMessage Event { get; set; }
    }
}