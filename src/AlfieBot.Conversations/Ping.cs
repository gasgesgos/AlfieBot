namespace AlfieBot.Conversations
{
    using System;
    using System.Threading.Tasks;
    using DSharpPlus.EventArgs;

    public class Ping
    {
        /// <summary>
        /// No ping. Only pong.
        /// </summary>
        /// <param name="e">Message creation event arguments and context.</param>
        public static async Task PingPong(MessageCreateEventArgs e)
        {
            if (e.Message.Content.StartsWith("ping", StringComparison.OrdinalIgnoreCase))
            {
                await e.Message.RespondAsync("NO PING. ONLY PONG.").ConfigureAwait(false);
            }
        }
    }
}
