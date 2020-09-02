namespace AlfieBot.Annoucements
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AlfieBot.Abstractions;
    using AlfieBot.Abstractions.Models;
    using AlfieBot.Permissions;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;

    public class AnnouncementCommands : BaseCommandModule
    {
        IStorageProvider<AnnouncementDefinitionEntity> storageProvider;
        IQueueClient queueClient;
        ILogger<AnnouncementCommands> logger;

        public AnnouncementCommands(IStorageProvider<AnnouncementDefinitionEntity> provider, IQueueClientFactory<AnnouncementMessage> queueClientFactory, ILogger<AnnouncementCommands> logger)
        {
            this.storageProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.queueClient = queueClientFactory.GetClient();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Sets up or overwrites an announcement definition.
        /// </summary>
        /// <param name="ctx">The command context.</param>
        /// <param name="name">The name of the announcement message. Used to refer to the announcement later.</param>
        /// <param name="minutes">The number of minutes between announcements.</param>
        /// <param name="antiSpamMessageBufferCount">The number of messages required before an announcement will trigger again. Prevents bot spam.</param>
        /// <param name="message">The message of the announcement.</param>
        [Command("SetAnnouncement")]
        [RequireRoles(RoleCheckMode.Any, WellKnownRoles.AlfieBotOperator, WellKnownRoles.Mods, WellKnownRoles.Staff)]
        [Description("Sets up an announcement.")]
        public async Task AddAnnouncement(CommandContext ctx,
            [Description("A handy name for the announcement.")]
            string name,
            [Description("The number of minutes between announcements.")]
            int minutes,
            [Description("The number of messages between announcements. Helps protect the channel from announcement spam.")]
            int antiSpamMessageBufferCount,
            [Description("The message itself.")]
            string message)
        {
            var nextMessageSeconds = minutes * 60;

            var definition = new AnnouncementDefinitionEntity()
            {
                ServerId = ctx.Guild.Id,
                ChannelId = ctx.Channel.Id,
                Name = name,
                AnnouncementText = message,
                AntiSpamBuffer = antiSpamMessageBufferCount,
                RepeatSeconds = nextMessageSeconds,
            };

            await this.storageProvider.AddOrUpdateAsync(definition).ConfigureAwait(false);

            var queueMessageBody = new AnnouncementMessage()
            {
                ServerId = ctx.Guild.Id,
                ChannelId = ctx.Channel.Id,
                Message = message,
                Name = name,
                NextMessageIntervalSeconds = nextMessageSeconds,
                AntiSpamMessageCount = antiSpamMessageBufferCount
            };

            var queueMessage = new Message()
            {
                Body = JsonSerializer.SerializeToUtf8Bytes(queueMessageBody),
                ContentType = "application/json",
                PartitionKey = definition.PartitionKey,
            };

            var scheduledDelivery = new DateTimeOffset(DateTime.UtcNow).AddSeconds(nextMessageSeconds);

            await this.queueClient.ScheduleMessageAsync(queueMessage, scheduledDelivery).ConfigureAwait(false);

            await ctx.RespondAsync($"Announcement {name} updated. It will run every {minutes} minutes and will wait for {antiSpamMessageBufferCount} messages before being announced multiple times.").ConfigureAwait(false);
        }

        /// <summary>
        /// Removes an announcement definition.
        /// </summary>
        /// <param name="ctx">The command context.</param>
        /// <param name="name">The name of the announcement message. Used to refer to the announcement.</param>
        [Command("RemoveAnnouncement")]
        [RequireRoles(RoleCheckMode.Any, WellKnownRoles.AlfieBotOperator, WellKnownRoles.Mods, WellKnownRoles.Staff)]
        [Description("Removes an announcement.")]
        public async Task RemoveAnnouncement(CommandContext ctx,
            [Description("A handy name for the announcement.")]
            string name)
        {
            var existingEntry = await this.storageProvider.ReadAsync(AnnouncementDefinitionEntity.GetFormattedPartitionKey(ctx.Guild.Id, ctx.Channel.Id), name).ConfigureAwait(false);

            if (existingEntry != null)
            {
                this.logger.LogInformation("Removing {announcementName} from server {serverId} and channel {channelId}.", existingEntry.Name, existingEntry.ServerId, existingEntry.ChannelId);

                await this.storageProvider.DeleteAsync(existingEntry).ConfigureAwait(false);
                await ctx.RespondAsync($"Removing {name}...").ConfigureAwait(false);
            }
            else
            {
                await ctx.RespondAsync($"Unable to locate an announcement named {name}. Was it already removed?").ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Shows existing announcement entries.
        /// </summary>
        /// <param name="ctx">The command context.</param>
        [Command("GetAnnouncements")]
        [RequireRoles(RoleCheckMode.Any, WellKnownRoles.AlfieBotOperator, WellKnownRoles.Mods, WellKnownRoles.Staff)]
        [Description("Displays information about announcements.")]
        public async Task GetAnnouncements(CommandContext ctx)
        {
            var existingEntries = await this.storageProvider.ReadPartition(AnnouncementDefinitionEntity.GetFormattedPartitionKey(ctx.Guild.Id, ctx.Channel.Id));

            var embed = new DiscordEmbedBuilder();


            if (existingEntries.Count() == 0)
            {
                embed.Title = "There are no announcements for this channel.";
            }
            else
            {
                embed.Title = "Announcements defined for this channel";
                foreach (var item in existingEntries)
                {
                    var name = item.Name;
                    var minutes = item.RepeatSeconds / 60;

                    embed.AddField(name, $"repeats every {minutes} minutes.");
                }
            }
            
            await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
        }
    }
}
