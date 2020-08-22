namespace AlfieBot.Abstractions.Models
{ 
    public class UserLevel : UserBasedEntity, IKeyedEntity
    {
        public string PartitionKey => ServerName;

        public string RowKey => UserName;

        public int Level { get; set; }
    }
}
