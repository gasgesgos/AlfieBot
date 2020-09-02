namespace AlfieBot.Abstractions.Models
{
    public abstract class UserEntity : UserBasedEntity, IKeyedEntity
    {
        private const string partitionKeyFormat = "{0}:{1}";

        /// <summary>
        /// A partition key based on the user name.
        /// </summary>
        public string PartitionKey => FormatPartitionKey(this.ServerId, this.UserId);

        public abstract string RowKey { get; }

        protected static string FormatPartitionKey(ulong serverId, ulong userId) => string.Format(partitionKeyFormat, serverId, userId);
    }
}
