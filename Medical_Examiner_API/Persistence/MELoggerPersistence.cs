using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Loggers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Medical_Examiner_API.Persistence
{
    /// <summary>
    /// Class responsible for logging actions to database
    /// </summary>
    public class MELoggerPersistence : IMELoggerPersistence
    {
        private string _databaseId;
        private Uri _endpointUri;
        private string _primaryKey;
        private DocumentClient _client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpointUri">Cosmos DB URI</param>
        /// <param name="primaryKey">Key required for connection</param>
        public MELoggerPersistence(Uri endpointUri, string primaryKey)
        {
            _databaseId = "testing123";
            _endpointUri = endpointUri;
            _primaryKey = primaryKey;
        }

        /// <summary>
        /// Set up Cosmos DB connection
        /// </summary>
        /// <returns>Task</returns>
        public async Task EnsureSetupAsync()
        {
            if (_client == null)
            {
                _client = new DocumentClient(_endpointUri, _primaryKey);
            }

            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);

            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = "MELogger" });
        }

        /// <summary>
        /// Write one log entry
        /// </summary>
        /// <param name="logEntry">object to be logged</param>
        /// <returns>bool</returns>
        public async Task<bool> SaveLogEntryAsync(LogMessageActionDefault logEntry)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "MELogger");
            await _client.UpsertDocumentAsync(documentCollectionUri, logEntry);

            return true;
        }
    }
}
