namespace AlfieBot.Abstractions.Models
{
    public abstract class ChannelEntity: ChannelBasedEntity, IKeyedEntity
    {
        private const string partitionKeyFormat = "{0}:{1}";

        /// <summary>
        /// A partition key that contains the server and channel names.
        /// </summary>
        public string PartitionKey => GetFormattedPartitionKey(this.ServerId, this.ChannelId);

        /// <summary>
        /// The row key for this entity.
        /// </summary>
        public abstract string RowKey { get; }

        public static string GetFormattedPartitionKey(ulong serverId, ulong channelId)
        {
            return string.Format(partitionKeyFormat, serverId, channelId);
        }

    }
}
