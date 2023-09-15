using System.ServiceModel;
using Contract;

namespace Notification.TaskMessages
{
    /// <summary>
    /// 
    /// </summary>
    [MessageContract]
    public class DeleteTaskMessage: TaskMessage
    {
        
        /// <summary>
        /// 
        /// </summary>
        public DeleteTaskMessage()
            :base(EventMessage.DeleteTask)
        {}
        
        /// <summary>
        /// 
        /// </summary>
        public PlanTaskApi DeletedTask { get; set; }
    }
}