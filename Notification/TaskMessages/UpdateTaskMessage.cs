using System.ServiceModel;
using Contract;

namespace Notification.TaskMessages
{
    [MessageContract]
    public class UpdateTaskMessage : TaskMessage
    {
        
        /// <summary>
        /// Sets event message to "Update"
        /// </summary>
        public UpdateTaskMessage()
        :base(EventMessage.UpdateTask)
        { }
        
        /// <summary>
        /// Represent entity before update
        /// </summary>
        public PlanTaskApi PreviousTask { get; set; }
        
        /// <summary>
        /// Represent entity after update
        /// </summary>
        public PlanTaskApi CurrentTask { get; set; }
    }
}