
namespace AlfieBot.Storage.Providers
{
    using System;
    using System.Threading.Tasks;
    using AlfieBot.Abstractions.Models;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;

    public class TableStorageProvider<T> : IStorageProvider<T> where T : class, IKeyedEntity
    {
        private readonly CloudTableClient client;
        private readonly ILogger<TableStorageProvider<T>> logger;
        
        private string TableName => typeof(T).Name;

        public TableStorageProvider(CloudTableClient client, ILogger<TableStorageProvider<T>> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddOrUpdateAsync(T entity)
        {
            var adapter = new TableEntityAdapter<T>(entity, entity.PartitionKey, entity.RowKey);
            var table = this.client.GetTableReference(this.TableName);

            await table.CreateIfNotExistsAsync().ConfigureAwait(false);

            var operation = TableOperation.InsertOrReplace(adapter);
            await ExecuteOperation(entity, table, operation).ConfigureAwait(false);
        }

        public async Task DeleteAsync(T entity)
        {
            var adapter = new TableEntityAdapter<T>(entity, entity.PartitionKey, entity.RowKey);
            var table = this.client.GetTableReference(this.TableName);

            var operation = TableOperation.Delete(adapter);
            await ExecuteOperation(entity, table, operation).ConfigureAwait(false);
        }

        public async Task<T> ReadAsync(string partition, string key)
        {
            var table = this.client.GetTableReference(this.TableName);
            var operation = TableOperation.Retrieve<TableEntityAdapter<T>>(partition, key);
            var result = await ExecuteOperation(table, operation).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Executes an operation that does not require an entity, and returns a result.
        /// </summary>
        private async Task<T> ExecuteOperation(CloudTable table, TableOperation operation)
        {
            var result = await table.ExecuteAsync(operation).ConfigureAwait(false);

            if (result?.HttpStatusCode == 404)
            {
                return null;
            }

            if (result.HttpStatusCode < 200 || result.HttpStatusCode > 299)
            {
                logger.LogError("Save of {typename} failed. Status code {resultCode}, {message}.", typeof(T).Name,  result?.HttpStatusCode, result?.Result?.ToString());
            }

            var adapterResult = result.Result as TableEntityAdapter<T>;
            return adapterResult?.OriginalEntity;
        }

        /// <summary>
        /// Executes an operation that requires an entity, and does not return a result.
        /// </summary>
        private async Task ExecuteOperation(T entity, CloudTable table, TableOperation operation)
        {
            var result = await table.ExecuteAsync(operation).ConfigureAwait(false);

            if (result.HttpStatusCode < 200 || result.HttpStatusCode > 299)
            {
                logger.LogError("Save of {typename} failed, key {partition}:{row}. Status code {resultCode}, {message}.", typeof(T).Name, entity.PartitionKey, entity.RowKey, result.HttpStatusCode, result.Result.ToString());
            }
        }
    }
}
