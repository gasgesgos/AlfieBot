namespace AlfieBot.Abstractions.Models
{
    public abstract class ChannelBasedEntity: ServerBasedEntity
    {
        public string ChannelName { get; set; }
    }
}
