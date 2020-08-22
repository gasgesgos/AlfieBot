namespace AlfieBot.Commands
{
    using System;
    using System.Threading.Tasks;
    using AlfieBot.Abstractions.Models;
    using Storage.Providers;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    public class UserLevels : BaseCommandModule
    {
        private readonly IStorageProvider<UserLevel> storageProvider;

        public UserLevels(IStorageProvider<UserLevel> storageProvider)
        {
            this.storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
        }

        [Command("getlevel")]
        public async Task ReadLevel(CommandContext ctx, string username)
        {
            var level = await this.storageProvider.ReadAsync(ctx.Guild.Name, ctx.User.Username).ConfigureAwait(false);
            if (level != null)
            {
                await ctx.RespondAsync($"{username} is level {level.Level}.").ConfigureAwait(false);
            }
            else
            {
                await ctx.RespondAsync("wat.");
            }
        }

        [Command("levelup")]
        public async Task AddLevel(CommandContext ctx)
        {
            var level = await this.storageProvider.ReadAsync(ctx.Guild.Name, ctx.User.Username) ?? new UserLevel() { ServerName = ctx.Guild.Name, UserName = ctx.User.Username, Level = 0 };

            level.Level += 1;

            await this.storageProvider.AddOrUpdateAsync(level);
            
            await ctx.RespondAsync($"{level.UserName} is now level {level.Level}.");
        }

    }
}
