using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Contract
{
    [ServiceContract]
    public interface IPlanTaskService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "tasks", RequestFormat = WebMessageFormat.Json)]
        void CreateTask(PlanTaskApi task);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "tasks/{id}", RequestFormat = WebMessageFormat.Json)]
        void UpdateTask(string id, PlanTaskApi newTask);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "tasks/{id}", RequestFormat = WebMessageFormat.Json)]
        void DeleteTask(string id);

        [OperationContract]
        [WebInvoke(Method ="GET", UriTemplate = "tasks/{id}", ResponseFormat = WebMessageFormat.Json)]
        PlanTaskApi GetTask(string id);

        [OperationContract]        
        [WebInvoke(Method ="GET", UriTemplate = "tasks?status={status}&sortBy={sortBy}",
            ResponseFormat = WebMessageFormat.Json)]       
        IEnumerable<PlanTaskApi> GetAllTasks(string status, string sortBy);
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "tasks/{id}/completed")]
        void SetStatusCompleted(string id);
    }
}
