using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Notification.TaskMessages;

namespace Notification.Deserilizer
{
    public class MessageSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(TaskMessage).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }
}