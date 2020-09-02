namespace AlfieBot.Abstractions.Models
{
    /// <summary>
    /// A keyed entity appropriate for table storage.
    /// </summary>
    public interface IKeyedEntity
    {
        string PartitionKey { get; } 

        string RowKey { get; }
    }
}
