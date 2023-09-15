using System.Runtime.Serialization;

namespace Service.Data
{
    [DataContract]
    public enum StatusTask
    {   
        [EnumMember(Value = "ToDo")]
        ToDo,
        [EnumMember(Value = "InProgress")]
        InProgress,
        [EnumMember(Value = "Ready")]
        Ready,
        [EnumMember(Value = "Completed")]
        Completed
    }
}
