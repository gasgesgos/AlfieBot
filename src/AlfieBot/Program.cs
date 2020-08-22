namespace AlfieBot
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Storage;
    using Config;

    public static class Program
    {

        // bot auth link: https://discordapp.com/oauth2/authorize?client_id=709207294159618109&scope=bot&permissions=738323520

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddConfiguration()
                        .AddCloudTableClient()
                        .AddTableStorageProvider()
                        .AddHostedService<Worker>();
                });
        
    }
}
