namespace AlfieBot.Abstractions.Models
{
    public abstract class ChannelBasedEntity: ServerBasedEntity
    {

        /// <summary>
        /// The id of the channel the entity relates to.
        /// </summary>
        public ulong ChannelId { get; set; }
    }
}
