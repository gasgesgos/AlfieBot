namespace AlfieBot.Conversations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DSharpPlus.EventArgs;

    public class WouldYouKindlyPhrase
    {
        public static async Task WouldYouKindly(MessageCreateEventArgs e)
        {
            // If the bot is mentioned and the phrase is spoken, reply.
            if (e.Message.MentionedUsers.Contains(e.Client.CurrentUser) && e.Message.Content.Contains("would you kindly", StringComparison.OrdinalIgnoreCase))
            {
                await e.Message.RespondAsync("No, a bot chooses, a slave obeys.").ConfigureAwait(false);
            }           
        }
    }
}
