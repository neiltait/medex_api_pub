using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common
{
    public class PersistenceBase
    {
        protected DocumentClient Client;
        protected  readonly string DatabaseId;
        protected  readonly Uri EndpointUri;
        protected  readonly string PrimaryKey;
        protected  readonly string CollectionName;

        public PersistenceBase(Uri endpointUri, string primaryKey, string databaseId, string collectionName)
        {
            DatabaseId = databaseId;
            EndpointUri = endpointUri;
            PrimaryKey = primaryKey;
            CollectionName = collectionName;
        }
        
        public async Task EnsureSetupAsync()
        {
            if (Client == null) Client = new DocumentClient(EndpointUri, PrimaryKey);

            await Client.CreateDatabaseIfNotExistsAsync(new Database {Id = DatabaseId});
            var databaseUri = UriFactory.CreateDatabaseUri(DatabaseId);

            // Samples
            await Client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection {Id = CollectionName});
        }
    }
}