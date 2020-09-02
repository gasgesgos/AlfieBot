namespace AlfieBot.Data
{
    using System;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using AlfieBot.Config;
    using AlfieBot.Data.Providers;
    using AlfieBot.Abstractions;
    using Microsoft.Azure.ServiceBus;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDataHandlers(this IServiceCollection collection)
        {
            return collection
                .AddCloudTableClient()
                .AddTableStorageProvider()
                .AddServiceBusConnection()
                .AddTransient(typeof(IQueueClientFactory<>), typeof(QueueClientFactory<>));
        }

        private static IServiceCollection AddCloudTableClient(this IServiceCollection collection)
        {
            return collection.AddSingleton<CloudTableClient>(
                (ctx) =>
                    {
                        
                        var settings = ctx.GetRequiredService<IOptions<StorageSettings>>()?.Value ??  throw new InvalidOperationException("Unable to find storage settings. Cannot configure storage.");
                        var credentials = new StorageCredentials(settings.StorageAccountName, settings.StorageAccountKey);

                        return new CloudTableClient(settings.StorageUri, credentials);
                    });
        }

        private static IServiceCollection AddTableStorageProvider(this IServiceCollection collection)
        {
            return collection.AddTransient(typeof(IStorageProvider<>), typeof(TableStorageProvider<>));
        }

        private static IServiceCollection AddServiceBusConnection(this IServiceCollection collection)
        {
            return collection.AddSingleton<ServiceBusConnection>(
                (ctx) =>
                {
                    var settings = ctx.GetRequiredService<IOptions<ServiceBusSettings>>()?.Value ?? throw new InvalidOperationException("Unable to find service bus settings. Cannot configure queue client.");

                    return new ServiceBusConnection(settings.ConnectionString, RetryPolicy.Default);
                });
        }
    }
}
