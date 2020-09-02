namespace AlfieBot.Abstractions.Models
{
    public abstract class ServerEntity : ServerBasedEntity, IKeyedEntity
    {
        /// <summary>
        /// A common partition key, containing server-specific data.
        /// </summary>
        public virtual string PartitionKey => $"{this.ServerId}";

        /// <summary>
        /// A row key.
        /// </summary>
        public abstract string RowKey { get; }
    }
}
