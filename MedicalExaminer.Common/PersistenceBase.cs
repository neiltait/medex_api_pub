using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common
{
    public class PersistenceBase
    {
        protected readonly string collectionName;
        protected readonly string databaseId;
        protected readonly Uri endpointUri;
        protected readonly string primaryKey;
        protected DocumentClient client;

        public PersistenceBase(Uri endpointUri, string primaryKey, string databaseId, string collectionName)
        {
            this.databaseId = databaseId;
            this.endpointUri = endpointUri;
            this.primaryKey = primaryKey;
            this.collectionName = collectionName;
        }

        public async Task EnsureSetupAsync()
        {
            if (client == null)
            {
                client = new DocumentClient(endpointUri, primaryKey);
            }

            await client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(databaseId);

            // Samples
            await client.CreateDocumentCollectionIfNotExistsAsync(
                databaseUri,
                new DocumentCollection { Id = collectionName });
        }
    }
}