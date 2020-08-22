namespace AlfieBot.Conversations
{
    using AlfieBot.Config;
    using DSharpPlus;

    public static class DiscordClientExtensions
    {
        /// <summary>
        /// Adds conversations to the client.
        /// </summary>
        /// <param name="client">The client to add conversations to.</param>
        public static void AddConversations(this DiscordClient client, EnvironmentSettings envSettings)
        {
            client.MessageCreated += Ping.PingPong;
            client.MessageCreated += WhereDoYouLive.WhereDoesTheBotLive(envSettings);
            client.MessageCreated += WouldYouKindlyPhrase.WouldYouKindly;
        }
    }
}
