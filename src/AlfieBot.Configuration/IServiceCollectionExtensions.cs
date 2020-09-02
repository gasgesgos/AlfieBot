namespace AlfieBot.Config
{
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection collection)
        {
            var builder = new ConfigurationBuilder();

            builder.AddEnvironmentVariables();
            builder.AddUserSecrets(Assembly.GetAssembly(typeof(EnvironmentSettings)), true);

            var configRoot = builder.Build();

            //this.BotSettings = configRoot.GetSection(nameof(BotSettings)).Get<BotSettings>() ?? new BotSettings();

            return collection
                .Configure<BotSettings>(configRoot.GetSection(nameof(BotSettings)))
                .Configure<StorageSettings>(configRoot.GetSection(nameof(StorageSettings)))
                .Configure<ServiceBusSettings>(configRoot.GetSection(nameof(ServiceBusSettings)))
                .Configure<EnvironmentSettings>(configRoot.GetSection(nameof(EnvironmentSettings)));
        }
    }
}
