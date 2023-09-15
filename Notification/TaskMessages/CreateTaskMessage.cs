using System.ServiceModel;
using Contract;

namespace Notification.TaskMessages
{
    /// <summary>
    /// 
    /// </summary>
    [MessageContract]
    public class CreateTaskMessage : TaskMessage
    {
        
        /// <summary>
        /// 
        /// </summary>
        public CreateTaskMessage()
            :base(EventMessage.CreateTask)
        {}
        
        /// <summary>
        /// 
        /// </summary>
        public PlanTaskApi CreatedTask { get; set; }
    }
}