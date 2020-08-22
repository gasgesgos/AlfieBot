namespace AlfieBot.Commands
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using System.Linq;
    using System.Globalization;
    using System.Diagnostics.Contracts;

    public class DiceRoller : BaseCommandModule
    {
        /// <summary>
        /// Rolls dice, specified in the message.
        /// </summary>
        /// <param name="ctx">The command context.</param>
        /// <param name="dice">A tokenized string, containing substrings that (hopefully) specify types of dice to roll.</param>
        [Command("roll")]
        public async Task ProcessRoll(CommandContext ctx, params string[] dice)
        {
            Contract.Assert(ctx != null);

            var response = new StringBuilder();

            var parsedDice = dice
                .Select(x => x.Trim('d'))
                .Select(x => int.Parse(x, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture));
            
            foreach (var die in parsedDice)
            {
                response.RollDie(die);
            }
            await ctx.RespondAsync(response.ToString()).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Rolls a single die and adds the result to the <see cref="StringBuilder"/>.
    /// </summary>
    public static class IStringBuilderExtensions
    {
        internal static void RollDie(this StringBuilder builder, int die)
        {
            var rand = new Random();

            var result = (int)(rand.NextDouble() * die) + 1;
            builder.Append($"--- d{die}: {result} ---");
        }
    }
}
