using System;
using System.Collections.Generic;
using Contract;
using Service.Data;

namespace Service.Services
{
    public static class PlanTaskExtension
    {
        public const string DateFormat = "M/dd/yyyy HH:mm:ss";
        
        /// <summary>
        /// Create instance from PlanTask to PlanTaskApi
        /// </summary>
        /// <param name="task">Instance, that convert to api</param>
        /// <returns>PlanTaskApi</returns>
        public static PlanTaskApi ToApi(this PlanTask task)
        {
            return new PlanTaskApi()
            {
                Id = task.Id.ToString(),
                Name = task.Name,
                Date = task.Date.ToString(format:DateFormat ),
                Description = task.Description,
                Priority = task.Priority.ToString(),
                Status = task.Status.ToString()
            };
        }
        
        /// <summary>
        /// Create list with PlanTaskApi from list of PlanTask
        /// </summary>
        /// <param name="tasks">A list of tasks, that convert to list of api</param>
        /// <returns>List of PlanTaskApi</returns>
        public static IEnumerable<PlanTaskApi> ToApiList(this IEnumerable<PlanTask> tasks)
        {
            var lst = new List<PlanTaskApi>();
            foreach (var v in tasks)
            {
                lst.Add(v.ToApi());
            }
            return lst;
        }

        /// <summary>
        ///  Prepare PlanTaskApi instance for Create method
        /// </summary>
        /// <param name="task">Instance, that will be converted to PlanTask for added it to database</param>
        /// <returns>PlanTask</returns>
        public static PlanTask ToCreate(this PlanTaskApi task)
        {
            return new PlanTask()
            {
                Name = task.Name,
                Date = DateTime.Now,
                Description = task.Description,
                Priority = task.Priority.ToEnum<PriorityTask>(),
                Status = StatusTask.ToDo
            };
        }
        
        /// <summary>
        /// Prepare PlanTaskApi instance for Update method
        /// </summary>
        /// <param name="task">Instance, that will be converted to PlanTask</param>
        /// <returns>PlanTask</returns>
        public static PlanTask ToUpdate(this PlanTaskApi task)
        {
            return new PlanTask()
            {
                Id = Convert.ToInt32(task.Id),
                Name = task.Name,
                Date = DateTime.ParseExact(task.Date, DateFormat, null),
                Priority = task.Priority.ToEnum<PriorityTask>(),
                Description = task.Description,
                Status = task.Status.ToEnum<StatusTask>()
            };
        }
        
        private static T ToEnum<T>(this string value) where T: Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}