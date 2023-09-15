using System;
using System.ServiceModel;
using Apache.NMS;
using AutoReject;
using Contract;
using Notification.Notifications;
using Notification.TaskMessages;
using Service.Providers;
using Service.Services;

namespace ConsoleApp1
{
    public class ApplicationContext
    {
        /// <summary>
        /// 
        /// </summary>
        public ApplicationContext()
        {
            MessageMapper = new TaskMessageMapper();
            NotificationProperties = new NotificationProperties();
            MqBrokerUri = new Uri(NotificationProperties.BrokerUri);
            MqConnectionFactory = new NMSConnectionFactory(MqBrokerUri);
            Producer = new Producer(MqConnectionFactory, NotificationProperties);
            Consumer = new Consumer<TaskMessage>( NotificationProperties, MessageMapper, MqConnectionFactory );
            AutoRejectProperties = new AutoRejectProperties();
            PlanTaskOperationProvider = new PlanTaskOperationProvider();
            PlanTaskService = new PlanTaskService(PlanTaskOperationProvider, Producer, MessageMapper);
            ServiceHost = new ServiceHost(PlanTaskService);
            AutoRejectService = new AutoRejectService(PlanTaskService, AutoRejectProperties, Consumer, MessageMapper);
        }
        /// <summary>
        /// 
        /// </summary>
        public IPlanTaskOperationProvider PlanTaskOperationProvider { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public IPlanTaskService PlanTaskService { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public ServiceHost ServiceHost { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public IAutoRejectService AutoRejectService { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public AutoRejectProperties AutoRejectProperties { get; }
        
        public IProducer Producer { get; }
        
        public IConsumer<TaskMessage> Consumer { get; }
        
        public IConnectionFactory MqConnectionFactory { get; }
        public Uri MqBrokerUri { get; }
        public NotificationProperties NotificationProperties { get; }
        public IMessageMapper MessageMapper { get; }
    }
}
