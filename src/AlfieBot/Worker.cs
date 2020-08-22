namespace AlfieBot
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AlfieBot.Commands;
    using AlfieBot.Config;
    using Conversations;
    using DSharpPlus.CommandsNext;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly BotSettings botSettings;
        private readonly EnvironmentSettings envSettings;
        private readonly IServiceProvider serviceCollection;

        public Worker(ILogger<Worker> logger, IOptions<BotSettings> botSettings, IOptions<EnvironmentSettings> envSettings, IServiceProvider serviceCollection)
        {
            this.serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.botSettings = botSettings?.Value ?? throw new ArgumentNullException(nameof(botSettings));
            this.envSettings = envSettings?.Value ?? throw new ArgumentNullException(nameof(envSettings));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var manager = new DiscordClientManager(botSettings.BotToken ?? throw new Exception("Missing BotToken config value."));
            var client = manager.Client;

            client.AddConversations(this.envSettings);

            var commands = client.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new List<string>() { "!!" },
                CaseSensitive = false,
                Services = this.serviceCollection
            });

            commands.RegisterBasicCommands();

            await client.ConnectAsync().ConfigureAwait(false);


            // Busy loop, that outputs to console/log, to help indicate that the service is still alive when viewing console/logs.
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }
    }
}
