namespace AlfieBot.Abstractions
{
    using Microsoft.Azure.ServiceBus;

    public interface IQueueClientFactory<T>
    {
        // Remove dependency on IQueueClient at some point....
        IQueueClient GetClient();
    }
}