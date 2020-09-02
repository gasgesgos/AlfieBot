namespace AlfieBot.Data.Providers
{
    using System;
    using AlfieBot.Abstractions;
    using AlfieBot.Abstractions.Models;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;
    using System.Linq;

    public class QueueClientFactory<T> : IQueueClientFactory<T>
    {
        private ServiceBusConnection connection;
        private ILogger<QueueClientFactory<T>> logger;

        public QueueClientFactory(ServiceBusConnection connection, ILogger<QueueClientFactory<T>> logger)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IQueueClient GetClient()
        {
            var queueName = typeof(T).Name;

            // Use the value from a queue name attribute if it's defined.
            var attribute = (QueueNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(QueueNameAttribute));
            if (attribute != null)
            {
                queueName = attribute.QueueName;
            }
            
            this.logger.LogDebug($"Creating client for {queueName}.");
            return new QueueClient(this.connection, queueName, ReceiveMode.ReceiveAndDelete, RetryPolicy.Default);
        }
    }
}
