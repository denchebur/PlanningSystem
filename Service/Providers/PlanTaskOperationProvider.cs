using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using Service.Data;
#nullable enable

namespace Service.Providers
{    
    public class PlanTaskOperationProvider : IPlanTaskOperationProvider
    {
        
        /// <summary>
        /// Send request to database for create new task
        /// </summary>
        /// <param name="task">PlanTask instance which will be created</param>
        /// <returns>Created PlanTask instance</returns>
        public PlanTask? CreateTask(PlanTask task)
        {
            using var ctx = new PlanTaskContext();
            task.Date = DateTime.Now;
            task.Status = StatusTask.ToDo;
            ctx.Tasks.Add(task);                                                
            ctx.SaveChanges();
            return task;
        }
        
        /// <summary>
        /// send request to database for delete task
        /// </summary>
        /// <param name="id">id task which will be deleted</param>
        /// <returns>null if task not found, PlanTask instance which will be deleted</returns>
        public PlanTask? DeleteTask(string id)
        {
            using var ctx = new PlanTaskContext();
            if (!ctx.Tasks.Any(t => t.Id.ToString() == id))
            {
                return null;
            }
            PlanTask task = ctx.Tasks.First(t => t.Id.ToString() == id);
            ctx.Tasks.Remove(task);
            ctx.SaveChanges();
            return task;
        }
        
        /// <summary>
        /// Send request to database for show all tasks (can be show with parameters)
        /// </summary>
        /// <param name="status?">Task status</param>
        /// <param name="sortBy?">Property for sort by {id, status, create date, priority}</param>
        /// <returns>List of PlanTask</returns>
        /// <exception cref="WebFaultException">400 (bad request)</exception>
        public IEnumerable<PlanTask> GetAllTasks(string status, string sortBy)
        {
            using var ctx = new PlanTaskContext();
            if (!ctx.Tasks.Any())
                return new List<PlanTask>();
            var taskList = from i in ctx.Tasks.ToList()
                select i;
            return taskList.ToList();
        }

        /// <summary>
        /// Send request to database for search one task by id
        /// </summary>
        /// <param name="id">id of task, which will be returned</param>
        /// <returns>null if task not found, PlanTask instance if task found</returns>
        public PlanTask? GetTask(string id)
        {
            using var ctx = new PlanTaskContext();
            if (!ctx.Tasks.Any(t => t.Id.ToString() == id))
                return null;
            return ctx.Tasks.First(t => t.Id.ToString() == id);
        }

        /// <summary>
        /// Send request to database for update task
        /// </summary>
        /// <param name="id">id of task, which will be updated</param>
        /// <param name="newTask">PlanTask instance for update</param>
        /// <returns>null if task not found, PlanTask instance if task found</returns>
        public PlanTask? UpdateTask(string id, PlanTask newTask)
        {
            using var ctx = new PlanTaskContext();
            if (!ctx.Tasks.Any(t => t.Id.ToString() == id))
                return null;
            PlanTask oldTask = ctx.Tasks.First(t => t.Id.ToString() == id);
            ctx.Entry(ctx.Tasks.First(t => t.Id.ToString() == id)).CurrentValues.SetValues(newTask);
            ctx.SaveChanges();
            return oldTask;
        }
        
        /// <summary>
        /// Send request to database for update task (set status "Conmleted")
        /// </summary>
        /// <param name="id">id of task, which will be updated</param>
        /// <returns>null if task not found, PlanTask instance if task found</returns>
        public PlanTask? SetStatusCompleted(string id)
        {
            using var ctx = new PlanTaskContext();
            if (!ctx.Tasks.Any(t => t.Id.ToString() == id))
                return null;
            PlanTask oldTask = ctx.Tasks.First(t => t.Id.ToString() == id);
            PlanTask taskForReturn = oldTask;
            ctx.Entry(ctx.Tasks.First(t => t.Id.ToString() == id))
                .CurrentValues.SetValues(oldTask.Status = StatusTask.Completed);
            ctx.SaveChanges();
            return taskForReturn;
        }
    }
}
