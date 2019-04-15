using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Persistence
{
    public class ExaminationPersistence : IExaminationPersistence
    {

        private string _databaseId;
        private Uri _endpointUri;
        private string _primaryKey;
        private DocumentClient _client;

        public ExaminationPersistence(Uri endpointUri, string primaryKey)
        {
            _databaseId = "testing123";
            _endpointUri = endpointUri;
            _primaryKey = primaryKey;
        }

        public async Task EnsureSetupAsync()
        {
            if (_client == null)
            {
                _client = new DocumentClient(_endpointUri, _primaryKey);
            }

            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);

            // Samples
            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = "Examinations" });
        }

        public async Task<bool> SaveExaminationAsync(Examination examination)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Examinations");
            await _client.UpsertDocumentAsync(documentCollectionUri, examination);

            return true;
        }

        public async Task<Examination> GetExaminationAsync(string Id)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(_databaseId, "Examinations", Id);
            var result = await _client.ReadDocumentAsync<Examination>(documentUri);

            if (result.Document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return result.Document;
        }

        public async Task<IEnumerable<Examination>> GetExaminationsAsync()
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Examinations");

            // build the query
            var feedOptions = new FeedOptions() { MaxItemCount = -1 };
            var query = _client.CreateDocumentQuery<Examination>(documentCollectionUri, "SELECT * FROM Examinations", feedOptions);
            var queryAll = query.AsDocumentQuery();

            // combine the results
            var results = new List<Examination>();
            while (queryAll.HasMoreResults)
            {
                results.AddRange(await queryAll.ExecuteNextAsync<Examination>());
            }

            return results;
        }
    }
}
