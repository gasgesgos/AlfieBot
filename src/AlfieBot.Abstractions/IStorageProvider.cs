namespace AlfieBot.Storage.Providers
{
    using System.Threading.Tasks;
    using AlfieBot.Abstractions.Models;

    public interface IStorageProvider<T>
    {
        Task AddOrUpdateAsync(T entity);

        Task<T> ReadAsync(string partition, string key);

        Task DeleteAsync(T entity);
    }
}
