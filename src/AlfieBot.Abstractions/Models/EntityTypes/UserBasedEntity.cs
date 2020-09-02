namespace AlfieBot.Abstractions.Models
{
    public abstract class UserBasedEntity : ServerBasedEntity
    {
        public ulong UserId { get; set; }
    }
}
