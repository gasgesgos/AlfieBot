namespace AlfieBot.Conversations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AlfieBot.Config;
    using DSharpPlus.EventArgs;

    public class WhereDoYouLive
    {
        /// <summary>
        /// Asks the bot where it's currently living.
        /// </summary>
        /// <param name="e">Current message creation event args and context.</param>
        /// <remarks>Handy for debugging, to determine where the current bot connection is being hosted.</remarks>
        public static async Task WhereDoesTheBotLive(MessageCreateEventArgs e)
        {
            if (e.Message.MentionedUsers.Contains(e.Client.CurrentUser))
            {
                if (e.Message.Content.Contains("where do you live?", StringComparison.OrdinalIgnoreCase))
                {
                    await e.Message.RespondAsync(
                        ConfigurationProvider.Instance.ConfigSettings.IsLocalDev
                        ? "I live in someone's computer."
                        : "I live in the cloud.").ConfigureAwait(false);
                }
                else
                {
                    await e.Message.RespondAsync("I'm not sure what you're going on about.").ConfigureAwait(false);
                }
            }
        }
    }
}
