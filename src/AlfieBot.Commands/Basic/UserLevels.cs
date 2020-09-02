namespace AlfieBot.Commands
{
    using System;
    using System.Threading.Tasks;
    using AlfieBot.Abstractions.Models;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using AlfieBot.Abstractions;

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
            var partition = UserLevel.GeneratePartitionKey(ctx.Guild.Id, ctx.User.Id);
            var key = UserLevel.GenerateRowKey();
            
            var level = await this.storageProvider.ReadAsync(partition, key).ConfigureAwait(false);
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
            var partition = UserLevel.GeneratePartitionKey(ctx.Guild.Id, ctx.User.Id);
            var key = UserLevel.GenerateRowKey();


            var level = await this.storageProvider.ReadAsync(partition, key) ?? new UserLevel() { ServerId = ctx.Guild.Id, UserId = ctx.User.Id, Level = 0 };

            level.Level += 1;

            await this.storageProvider.AddOrUpdateAsync(level);
            
            await ctx.RespondAsync($"{ctx.User.Username} is now level {level.Level}.");
        }

    }
}
