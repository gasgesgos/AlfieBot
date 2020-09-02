namespace AlfieBot.Commands
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    public class Haldo : BaseCommandModule
    {
        /// <summary>
        /// Greets a user.
        /// </summary>
        [Command("haldo")]
        public async Task ProcessHaldo(CommandContext ctx)
        {
            Contract.Assert(ctx != null);

            await ctx.RespondAsync($"Haldo {ctx.User.Mention}!").ConfigureAwait(false);
        }
    }
}
