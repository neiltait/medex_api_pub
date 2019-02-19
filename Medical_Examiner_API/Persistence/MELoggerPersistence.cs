using System;
using System.Threading.Tasks;
using Medical_Examiner_API.Loggers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Medical_Examiner_API.Persistence
{
    public class MELoggerPersistence : IMELoggerPersistence
    {
        private DocumentClient _client;
        private readonly string _databaseId;
        private readonly Uri _endpointUri;
        private readonly string _primaryKey;

        public MELoggerPersistence(Uri endpointUri, string primaryKey)
        {
            _databaseId = "testing123";
            _endpointUri = endpointUri;
            _primaryKey = primaryKey;
        }


        public async Task<bool> SaveLogEntryAsync(LogMessageActionDefault logEntry)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "MELogger");
            await _client.UpsertDocumentAsync(documentCollectionUri, logEntry);

            return true;
        }

        public async Task EnsureSetupAsync()
        {
            if (_client == null) _client = new DocumentClient(_endpointUri, _primaryKey);

            await _client.CreateDatabaseIfNotExistsAsync(new Database {Id = _databaseId});
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);

            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri,
                new DocumentCollection {Id = "MELogger"});
        }
    }
}