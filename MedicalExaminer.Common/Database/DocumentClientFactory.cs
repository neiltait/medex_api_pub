using Cosmonaut;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common.Database
{
    /// <summary>
    /// Document Client Factory.
    /// </summary>
    public class DocumentClientFactory : IDocumentClientFactory
    {
        /// <summary>
        /// Create Client.
        /// </summary>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <returns><see cref="IDocumentClient"/>.</returns>
        public IDocumentClient CreateClient(IClientSettings connectionSettings)
        {
            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };

            var client = new DocumentClient(
                connectionSettings.EndPointUri,
                connectionSettings.PrimaryKey,
                connectionPolicy);

            return client;
        }

        /// <summary>
        /// Create Cosmos Store.
        /// </summary>
        /// <typeparam name="TEntity">Type of Collection.</typeparam>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <returns><see cref="ICosmosStore{TEntity}"/>.</returns>
        public ICosmosStore<TEntity> CreateCosmosStore<TEntity>(IConnectionSettings connectionSettings)
            where TEntity : class
        {
            var cosmosSettings = new CosmosStoreSettings(
                connectionSettings.DatabaseId,
                connectionSettings.EndPointUri,
                connectionSettings.PrimaryKey);

            ICosmosStore<TEntity> cosmosStore = new CosmosStore<TEntity>(cosmosSettings);

            return cosmosStore;
        }
    }
}
