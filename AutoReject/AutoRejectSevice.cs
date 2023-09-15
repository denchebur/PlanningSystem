using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Contract;
using Notification.Notifications;
using Notification.TaskMessages;
using Service.Data;
using Service.Services;
using Timer = System.Timers.Timer;

namespace AutoReject
{
    public class AutoRejectService : IAutoRejectService
    {
        public AutoRejectService(IPlanTaskService taskService, AutoRejectProperties appProperties,
            IConsumer<TaskMessage> consumer, IMessageMapper mapper)
        {
            _mapper = mapper;
            _taskService = taskService;
            _appProperties = appProperties;
            _consumer = consumer;
        }

        private readonly IMessageMapper _mapper;
        private readonly IConsumer<TaskMessage> _consumer;
        //private readonly JsonSerializer _serializer = JsonSerializer.Create();
        private readonly IPlanTaskService _taskService;
        private readonly AutoRejectProperties _appProperties;

        private ConcurrentDictionary<string, ElapsedEventHandler> dict =
            new ConcurrentDictionary<string, ElapsedEventHandler>();

        private int interval = 1000;
        private Timer _timer;
        
        private void AddDeleteTaskHandler(PlanTaskApi task)
        {
            ElapsedEventHandler handler = CreateTaskDeleteTimerEventHandler(task, _timer);
            dict.TryAdd(task.Id, handler);
            _timer.Elapsed += handler;
            
        }

        private void RemoveTaskDeletionHandler(PlanTaskApi task)
        {
            if (dict.TryRemove(task.Id, out var handler))
            {
                _timer.Elapsed -= handler;
            }
        }

        public async Task AutoRejectStart()
        {
            _timer = new Timer(interval);
            _timer.Enabled = true;
            _timer.Start();
            var list = _taskService.GetAllTasks("ToDo", null).ToList();
            
            list.Select(t =>
            {
                ElapsedEventHandler a = CreateTaskDeleteTimerEventHandler(t, _timer);
                dict.TryAdd(t.Id, a);
                return a;
            }).ToList().ForEach(a => _timer.Elapsed += a);

            await StartListening();

        }

        private ElapsedEventHandler CreateTaskDeleteTimerEventHandler(PlanTaskApi taskApi, Timer timer)
        {
            DateTime date = default;
            if (DateTime.TryParseExact(taskApi.Date, PlanTaskExtension.DateFormat,
                null, DateTimeStyles.AllowWhiteSpaces, out var d))
            {
                date = d.AddSeconds(_appProperties.DataReject);
            }

            return (sender, args) =>
            {
                if (date > DateTime.Now)
                {
                    return;
                }
                
                try
                {
                    if ((StatusTask) Enum.Parse(typeof(StatusTask), taskApi.Status) == StatusTask.ToDo)
                    {
                        _taskService.DeleteTask(taskApi.Id);
                    }
                }
                finally
                {
                    if (dict.TryRemove(taskApi.Id, out var handler))
                    {
                        timer.Elapsed -= handler;
                    }
                }
            };
        }

        private void TaskMessageHandler<T>(T taskMessage)
        {
            if(taskMessage is CreateTaskMessage createTaskMessage)
                CreateTaskMessageHandler(createTaskMessage);
            else if( taskMessage is DeleteTaskMessage deleteTaskMessage)
                DeleteTaskMessageHandler(deleteTaskMessage);
            else if(taskMessage is UpdateTaskMessage updateTaskMessage)
                UpdateTaskMessageHandler(updateTaskMessage);
            
            
        }

        private void CreateTaskMessageHandler(CreateTaskMessage taskMessage)
        {
            AddDeleteTaskHandler(taskMessage.CreatedTask);
            Console.WriteLine("received " + _mapper.ToMessage(taskMessage));
        }
        
        private void UpdateTaskMessageHandler(UpdateTaskMessage taskMessage)
        {
            if (taskMessage.CurrentTask.Status != StatusStringEnum.ToDo)
            {
                RemoveTaskDeletionHandler(taskMessage.CurrentTask);
            }
            else
            {
                AddDeleteTaskHandler(taskMessage.CurrentTask);
            }
            Console.WriteLine("received " + _mapper.ToMessage(taskMessage));
        }
        
        private void DeleteTaskMessageHandler(DeleteTaskMessage taskMessage)
        {
            RemoveTaskDeletionHandler(taskMessage.DeletedTask);
            Console.WriteLine("received " + _mapper.ToMessage(taskMessage));
        }
        
        private async Task StartListening()
        {
            _consumer.OnMessage += TaskMessageHandler;
            await Task.Run(() => _consumer.StartListening());
        }
    }
}