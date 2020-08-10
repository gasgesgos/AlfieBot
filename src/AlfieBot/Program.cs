namespace AlfieBot
{
    using System;
    using System.Threading.Tasks;
    using AlfieBot.Commands;
    using AlfieBot.Config;
    using Conversations;
    using DSharpPlus.CommandsNext;

    public static class Program
    {
        private static DiscordClientManager manager;

        // bot auth link: https://discordapp.com/oauth2/authorize?client_id=709207294159618109&scope=bot&permissions=738323520
        public static async Task Main()
        {
            manager = new DiscordClientManager(ConfigurationProvider.Instance.BotSettings.BotToken ?? throw new Exception("Missing BotToken config value."));

            var client = manager.Client;

            client.AddConversations();

            var commands = client.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefix = "!!",
                CaseSensitive = false
            });

            commands.RegisterBasicCommands();

            await client.ConnectAsync().ConfigureAwait(false);

            // Busy loop, that outputs to console/log, to help indicate that the service is still alive when viewing console/logs.
            var message = "tick";
            while(true)
            {
                Console.WriteLine(message);
                await Task.Delay(3000).ConfigureAwait(false);
                message = message.Equals("tick", StringComparison.OrdinalIgnoreCase) ? "tock" : "tick";
            }
        }
    }
}
