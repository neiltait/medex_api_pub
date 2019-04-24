using Cosmonaut;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common.Database
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        public IDocumentClient CreateClient(IConnectionSettings connectionSettings)
        {
            var Client = new DocumentClient(connectionSettings.EndPointUri, connectionSettings.PrimaryKey);

            Client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = connectionSettings.DatabaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(connectionSettings.DatabaseId);

            Client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection { Id = connectionSettings.Collection });

            return Client;
        }

        public ICosmosStore<TEntity> CreateCosmosStore<TEntity>(IConnectionSettings connectionSettings)
            where TEntity : class
        {
            var cosmosSettings = new CosmosStoreSettings(connectionSettings.DatabaseId, connectionSettings.EndPointUri
                , connectionSettings.PrimaryKey);

            ICosmosStore<TEntity> cosmosStore = new CosmosStore<TEntity>(cosmosSettings);
            
            return cosmosStore;
        }
    }
}
