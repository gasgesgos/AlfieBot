namespace AlfieBot
{
    using System;
    using AlfieBot.Annoucements;
    using Config;
    using Data;
    using DSharpPlus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    public static class Program
    {

        // bot auth link: https://discordapp.com/oauth2/authorize?client_id=709207294159618109&scope=bot&permissions=738323520

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddConfiguration()
                        .AddDiscordClient()
                        .AddDataHandlers()
                        .AddHostedService<Worker>()
                        .AddHostedService<Announcer>();
                }
            );
        }

        public static IServiceCollection AddDiscordClient(this IServiceCollection collection)
        {
            return collection.AddSingleton<DiscordClient>((opts) => 
            {
                var botConfig = opts.GetRequiredService<IOptions<BotSettings>>();

                var token = botConfig?.Value.BotToken ?? throw new InvalidOperationException("Cannot create discord client without a token.");

                return new DiscordClient(
                   new DiscordConfiguration()
                   {
                       TokenType = TokenType.Bot,
                       Token = token,
                       UseInternalLogHandler = true,
                       LogLevel = LogLevel.Debug
                   });
            });
        }
    }
}
