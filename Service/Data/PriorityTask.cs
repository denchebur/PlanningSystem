using System.Runtime.Serialization;

namespace Service.Data
{    
    [DataContract]
    public enum PriorityTask
    {
        [EnumMember(Value = "High")]
        High,
        [EnumMember(Value = "Average")]
        Average,
        [EnumMember(Value = "Low")]
        Low
    }
}
