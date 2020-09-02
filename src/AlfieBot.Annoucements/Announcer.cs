namespace AlfieBot.Annoucements
{
    using System;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using AlfieBot.Abstractions;
    using AlfieBot.Abstractions.Models;
    using DSharpPlus;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Handles announcements
    /// </summary>
    public class Announcer : BackgroundService
    {
        private readonly ILogger<Announcer> logger;
        private readonly DiscordClient client;
        private readonly IQueueClient queueClient;
        private readonly IStorageProvider<AnnouncementDefinitionEntity> storageProvider;

        public Announcer(
            DiscordClient client, 
            IQueueClientFactory<AnnouncementMessage> queueClientFactory, 
            IStorageProvider<AnnouncementDefinitionEntity> storageProvider, 
            ILogger<Announcer> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.queueClient = queueClientFactory.GetClient();
            this.storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.queueClient.RegisterMessageHandler(
                async (Message message, CancellationToken token) =>
                {
                    var payload = JsonSerializer.Deserialize<AnnouncementMessage>(message.Body);

                    var serverId = payload.ServerId;
                    var channelId = payload.ChannelId;

                    // Verify that the announcement exists
                    var exists = await this.storageProvider.RecordExists(AnnouncementDefinitionEntity.GetFormattedPartitionKey(payload.ServerId, payload.ChannelId), payload.Name).ConfigureAwait(false);
                    
                    if (!exists)
                    {
                        this.logger.LogWarning("Not processing announcement, the definition no longer exists.");
                        return;
                    }

                    var channel = await this.client.GetChannelAsync(channelId).ConfigureAwait(false);

                    // Check to see if we have enough messages to satisfy the anti-spam rules and send if everything is good.
                    var canSendMessage = false;
                    var spamTrackingMessageId = payload.PreviousAnnouncementMessageId;

                    if (spamTrackingMessageId == null)
                    {
                        canSendMessage = true;
                    }
                    else
                    {
                        var messagesSinceLastAnnouncement = await (channel.GetMessagesAfterAsync(spamTrackingMessageId ?? 0)).ConfigureAwait(false);

                        if (messagesSinceLastAnnouncement.Count >= payload.AntiSpamMessageCount)
                        {
                            canSendMessage = true;
                        }
                    }
                    
                    if (canSendMessage && channel != null)
                    {
                        // Send the announcement
                        var announcementMessage = await this.client.SendMessageAsync(channel, payload.Message).ConfigureAwait(false);
                        spamTrackingMessageId = announcementMessage.Id;
                    }
                    else
                    {
                        var guild = await this.client.GetGuildAsync(serverId).ConfigureAwait(false);
                        this.logger.LogInformation("Not sending message {messageName} on server {serverName} for channel {channelName} due to anti-spam rules.", payload.Name, guild.Name, channel.Name);
                    }

                    // Create and schedule the next message 
                    var newMessagePayload = payload.Clone() as AnnouncementMessage;
                    newMessagePayload.PreviousAnnouncementMessageId = spamTrackingMessageId ?? channel.LastMessageId;

                    var newMessage = new Message()
                    {
                        Body = JsonSerializer.SerializeToUtf8Bytes(newMessagePayload),
                        ContentType = "application/json",
                        //TimeToLive = TimeSpan.FromSeconds(payload.NextMessageIntervalSeconds * 1.5)
                    };
                    var scheduledDelivery = new DateTimeOffset(DateTime.UtcNow).AddSeconds(payload.NextMessageIntervalSeconds);
                    ;

                    await this.queueClient.ScheduleMessageAsync(newMessage, scheduledDelivery).ConfigureAwait(false);
                }, 
                (ExceptionReceivedEventArgs e) => { this.logger.LogError(e.Exception, "Exception found while handling announcement message."); return Task.CompletedTask; }
                );

            // Busy loop, that outputs to console/log, to help indicate that the service is still alive when viewing console/logs.
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }
    }
}
