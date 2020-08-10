namespace AlfieBot
{
    using System;
    using DSharpPlus;

    /// <summary>
    /// Manages and instantiates Discord clients.
    /// </summary>
    public sealed class DiscordClientManager : IDisposable
    {
        public DiscordClient Client { get; private set; }

        public DiscordClientManager(string botToken)
        {
            this.Client =
                new DiscordClient(
                    new DiscordConfiguration()
                    {
                        TokenType = TokenType.Bot,
                        Token = botToken,
                        UseInternalLogHandler = true,
                        LogLevel = LogLevel.Debug
                    });
        }

        public void Dispose()
        {
            ((IDisposable)this.Client).Dispose();
        }
    }
}
