using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Contract;
using Newtonsoft.Json;
using NLog;
using Notification.Notifications;
using Notification.TaskMessages;
using Service.Data;
using Service.Providers;

namespace Service.Services
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.Single)]
    public class PlanTaskService : IPlanTaskService
    {
        private readonly IPlanTaskOperationProvider _taskProvider;
        private readonly IProducer _producer;
        private readonly IMessageMapper _mapper;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Create PlanTaskService instance
        /// </summary>
        /// <param name="tasksProvider"></param>
        /// <param name="producer"></param>
        public PlanTaskService(IPlanTaskOperationProvider tasksProvider, IProducer producer, IMessageMapper mapper)
        {
            _mapper = mapper;
            _taskProvider = tasksProvider;
            _producer = producer;
        }
        
        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="task">Task api entity</param>
        /// <exception cref="WebFaultException">400 (bad request)</exception>
        public void CreateTask(PlanTaskApi task)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(task);
            CreateTaskMessage notify = new CreateTaskMessage();
            if (!Validator.TryValidateObject(task, context, results, true))
            {
                throw new WebFaultException<string>("Incorrect input", HttpStatusCode.BadRequest);
            }
            var t = _taskProvider.CreateTask(task.ToCreate());
            notify.CreatedTask = t.ToApi();
            Logger.Info("sadasdf");
            _producer.Send(_mapper.ToMessage(notify));
        }
        
        /// <summary>
        /// Delete task by id
        /// </summary>
        /// <param name="id">Task id</param>
        /// <exception cref="WebFaultException">404 (not found)</exception>
        public void DeleteTask(string id)
        {
            DeleteTaskMessage notify = new DeleteTaskMessage();
            var task = _taskProvider.DeleteTask(id);
            if (task != null)
            {
                notify.DeletedTask = task.ToApi();
                _producer.Send(_mapper.ToMessage(notify));
            }
            else
            {
                throw new WebFaultException<string>("Not found", HttpStatusCode.NotFound);
            }
        }
        
        /// <summary>
        /// Returns list of tasks with search parameters 
        /// </summary>
        /// <param name="status?">Task status</param>
        /// <param name="sortBy?">Property for sort by {id, status, create date, priority}</param>
        /// <returns>List of PlanTaskApi</returns>
        /// <exception cref="WebFaultException">404 (not found)</exception>
        public IEnumerable<PlanTaskApi> GetAllTasks(string status, string sortBy)
        {
            var tasks = _taskProvider.GetAllTasks(null, null).ToList();
            if (!tasks.Any())
            {
                throw new WebFaultException<string>($"Tasks list empty", HttpStatusCode.NotFound);
            }
            if(status != null)
                if (Enum.TryParse<StatusTask>(status, out var q))
                    return tasks.Where(c => c.Status == q).ToApiList().ToList();
            if(sortBy != null)
            {
                switch (sortBy)
                {
                    case "id":
                        return tasks.OrderBy(i => i.Id).ToApiList().ToList();
                    case "status":
                        return tasks.OrderBy(i => i.Status).ToApiList().ToList();
                    case "priority":
                        return tasks.OrderBy(i => i.Priority).ToApiList().ToList();
                    case "datetime":
                        return tasks.OrderBy(i => i.Date).ToApiList().ToList();
                    default: throw new WebFaultException<string>("Incorrect input", HttpStatusCode.BadRequest);
                }
            }
            return _taskProvider.GetAllTasks(status, sortBy).ToApiList().ToList();
        }
        /// <summary>
        /// Returns task by id
        /// </summary>
        /// <param name="id">Task id</param>
        /// <returns>PlanTaskApi</returns>
        /// <exception cref="WebFaultException">404 (not found)</exception>
        public PlanTaskApi GetTask(string id)
        {
            var task = _taskProvider.GetTask(id);
            if (task == null)
            {
                throw new WebFaultException<string>("Not found", HttpStatusCode.NotFound);
            }
            return task.ToApi();
        }

        /// <summary>
        /// Send request to provider for update task by id
        /// </summary>
        /// <param name="id">Task id</param>
        /// <param name="newTask">new task properties</param>
        /// <exception cref="WebFaultException">400 (bad request)</exception>
        public void UpdateTask(string id, PlanTaskApi newTask)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(newTask);
            if (Validator.TryValidateObject(newTask, context, results, false))
            {
                throw new WebFaultException<string>("Incorrect input", HttpStatusCode.BadRequest);
            }
            UpdateTaskMessage notify = new UpdateTaskMessage();
            PlanTaskApi taskApi;
            lock (this)
            {
                taskApi = GetTask(id);
            }
            notify.PreviousTask = taskApi;
            if(newTask.Description != null) 
                taskApi.Description = newTask.Description;
            if (newTask.Status != null)
                taskApi.Status = newTask.Status;
            else
                newTask.Status = taskApi.Status; 
            notify.CurrentTask = _taskProvider.UpdateTask(id, taskApi.ToUpdate()).ToApi();
            _producer.Send(_mapper.ToMessage(notify));
        }
        
        /// <summary>
        /// Update task, set status "Completed" by id
        /// </summary>
        /// <param name="id">Task id</param>
        public void SetStatusCompleted(string id)
        {
            UpdateTaskMessage notify = new UpdateTaskMessage();
            var task = _taskProvider.SetStatusCompleted(id);
            if (task == null)
            {
                throw new WebFaultException<string>("Not found", HttpStatusCode.NotFound);
            }
            var taskApi = task.ToApi();
            notify.PreviousTask = taskApi;
            taskApi.Status = "Completed";
            notify.CurrentTask = taskApi;
            _producer.Send(_mapper.ToMessage(notify));
        }
    }
}
