using System;
using System.Threading.Tasks;
using MedicalExaminer.Common.Loggers;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common
{
 /// <summary>
    /// Class responsible for logging actions to database
    /// </summary>
    public class MeLoggerPersistence : PersistenceBase, IMeLoggerPersistence
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpointUri">Cosmos DB URI</param>
        /// <param name="primaryKey">Key required for connection</param>
        /// <param name="databaseId">Id of database</param>
        public MeLoggerPersistence(Uri endpointUri, string primaryKey, string databaseId)
            : base(endpointUri, primaryKey, databaseId, "MELogger")
        {
        }

        /// <summary>
        /// Write one log entry
        /// </summary>
        /// <param name="logEntry">object to be logged</param>
        /// <returns>bool</returns>
        public async Task<bool> SaveLogEntryAsync(LogMessageActionDefault logEntry)
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionName);
            await Client.UpsertDocumentAsync(documentCollectionUri, logEntry);
            return true;
        }
    }
}
