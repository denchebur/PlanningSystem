using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notification.TaskMessages;

namespace Notification.Deserilizer
{
    public class MessageCoverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new MessageSpecifiedConcreteClassConverter() };
        

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            
            switch (jo["Event"].Value<string>())
            {
                case EventStringEnum.CreateTask:
                    return JsonConvert.DeserializeObject<CreateTaskMessage>(jo.ToString(), SpecifiedSubclassConversion);
                case EventStringEnum.DeleteTask:
                    return JsonConvert.DeserializeObject<DeleteTaskMessage>(jo.ToString(), SpecifiedSubclassConversion);
                case EventStringEnum.UpdateTask:
                    return JsonConvert.DeserializeObject<UpdateTaskMessage>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(TaskMessage));
        }
        
    }
    internal static class EventStringEnum
    {
        public const string CreateTask = "CreateTask";
        public const string UpdateTask = "UpdateTask";
        public const string DeleteTask = "DeleteTask";
    }
    
}



