using System;
using System.Threading.Tasks;
using Medical_Examiner_API.Loggers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Medical_Examiner_API.Persistence
{
    public class MeLoggerPersistence : PersistenceBase, IMeLoggerPersistence
    {
        public MeLoggerPersistence(Uri endpointUri, string primaryKey, string databaseId) : base(endpointUri, primaryKey, databaseId, "MELogger")
        {
        }

        public async Task<bool> SaveLogEntryAsync(LogMessageActionDefault logEntry)
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionName);
            await Client.UpsertDocumentAsync(documentCollectionUri, logEntry);
            return true;
        }
    }
}
