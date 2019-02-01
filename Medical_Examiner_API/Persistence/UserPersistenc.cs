using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medical_Examiners_API.Persistence
{
    public class UserPersistence : IUserPersistence
    {

        private string _databaseId;
        private Uri _endpointUri;
        private string _primaryKey;
        private DocumentClient _client;

        public UserPersistence(Uri endpointUri, string primaryKey)
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
            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = "Users" });
        }

        public async Task SaveUserAsync(Models.User user)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Users");
            await _client.UpsertDocumentAsync(documentCollectionUri, user);
        }

        public async Task<Models.User> GetUserAsync(string Id)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(_databaseId, "Users", Id);
            var result = await _client.ReadDocumentAsync<Models.User>(documentUri);
            return result.Document;
        }

        public async Task<IEnumerable<Models.User>> GetUsersAsync()
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Users");

            // build the query
            var feedOptions = new FeedOptions() { MaxItemCount = -1 };
            var query = _client.CreateDocumentQuery<Models.User>(documentCollectionUri, "SELECT * FROM Users", feedOptions);
            var queryAll = query.AsDocumentQuery();

            // combine the results
            var results = new List<Models.User>();
            while (queryAll.HasMoreResults)
            {
                results.AddRange(await queryAll.ExecuteNextAsync<Models.User>());
            }

            return results;
        }
    }
}
