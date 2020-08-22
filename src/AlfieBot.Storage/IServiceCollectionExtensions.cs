namespace AlfieBot.Storage
{
    using System;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using AlfieBot.Config;
    using AlfieBot.Storage.Providers;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudTableClient(this IServiceCollection collection)
        {
            return collection.AddSingleton<CloudTableClient>(
                (ctx) =>
                    {
                        
                        var settings = ctx.GetRequiredService<IOptions<StorageSettings>>()?.Value ??  throw new InvalidOperationException("Unable to find storage settings. Cannot configure storage.");
                        var credentials = new StorageCredentials(settings.StorageAccountName, settings.StorageAccountKey);

                        return new CloudTableClient(settings.StorageUri, credentials);
                    });
        }

        public static IServiceCollection AddTableStorageProvider(this IServiceCollection collection)
        {
            return collection.AddTransient(typeof(IStorageProvider<>), typeof(TableStorageProvider<>));
        }
    }
}
