namespace AlfieBot.Abstractions.Models
{ 
    public abstract class UserBasedEntity: ServerBasedEntity
    {
        public string UserName { get; set; }
    }
}
