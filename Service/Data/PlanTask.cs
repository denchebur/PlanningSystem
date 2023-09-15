using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Service.Data
{
    [DataContract]
    public class PlanTask
    {
        
        /// <summary>
        /// Unique identifier that stored in database
        /// </summary>
        [DataMember]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]       
        public int Id { get; set; }
        
        /// <summary>
        /// Task name
        /// </summary>
        [DataMember]
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// Task description
        /// Should be updated
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// Date of create task
        /// </summary>
        [DataMember]        
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Task status
        /// Should be updated
        /// </summary>
        [DataMember]
        public StatusTask Status { get; set; }
        
        /// <summary>
        /// Task priority
        /// </summary>
        [DataMember]
        [Required]
        [Range(0,2)]
        public PriorityTask Priority { get; set; }
    }
}