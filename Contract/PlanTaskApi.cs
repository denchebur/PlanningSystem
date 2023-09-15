using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Contract
{
    [DataContract]
    public class PlanTaskApi
    {
        //all string formatting to json, mb enum
        
        [DataMember]
        //possible parse to int
        public string Id { get; set; }
        [DataMember]
        [Required]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]       
        // date should be represent defined pattern
        public string Date { get; set; }
        [DataMember]
        // should be one of enum value
        public string Status { get; set; }
        [DataMember]
        [Required]
        // should be one of enum value
        public string Priority { get; set; }
    }
}