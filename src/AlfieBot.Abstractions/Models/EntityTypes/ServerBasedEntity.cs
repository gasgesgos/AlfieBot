namespace AlfieBot.Abstractions.Models
{
    public abstract class ServerBasedEntity
    {
        /// <summary>
        /// The id of the server the entity relates to.
        /// </summary>
        public ulong ServerId { get; set; }
    }
}
