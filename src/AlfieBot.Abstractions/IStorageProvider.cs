namespace AlfieBot.Abstractions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlfieBot.Abstractions.Models;

    public interface IStorageProvider<T> where T: IKeyedEntity
    {
        /// <summary>
        /// Determines if a record with the entity's key exists.
        /// </summary>
        /// <returns>True if the record exists, false otherwise.</returns>
        Task<bool> RecordExists(T entity);

        Task<bool> RecordExists(string partition, string key);

        Task AddOrUpdateAsync(T entity);

        Task<T> ReadAsync(string partition, string key);

        Task<IEnumerable<T>> ReadPartition(string partition);

        Task DeleteAsync(T entity);
    }
}
