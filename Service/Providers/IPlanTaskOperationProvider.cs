using System.Collections.Generic;
using Service.Data;
#nullable enable

namespace Service.Providers
{
    public interface IPlanTaskOperationProvider
    {
        IEnumerable<PlanTask> GetAllTasks(string status, string sortBy);
        PlanTask? GetTask(string id);
        PlanTask? DeleteTask(string id);
        PlanTask? CreateTask(PlanTask task);
        PlanTask? UpdateTask(string id, PlanTask newTask);
        PlanTask? SetStatusCompleted(string id);
    }
}
