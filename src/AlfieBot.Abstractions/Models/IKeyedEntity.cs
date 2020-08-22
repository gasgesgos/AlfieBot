namespace AlfieBot.Abstractions.Models
{
    public interface IKeyedEntity
    {
        string PartitionKey { get; }

        string RowKey { get; }
    }
}
