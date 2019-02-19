using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medical_Examiner_API.Persistence
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

        public async Task<Models.User> SaveUserAsync(Models.User user)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Users");
            var doc = await _client.UpsertDocumentAsync(documentCollectionUri, user);

            if (doc == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return (dynamic)doc;
        }

        public async Task<Models.User> CreateUserAsync(Models.User user)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Users");
            var document = await _client.CreateDocumentAsync(documentCollectionUri, user);

            if (document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return (dynamic)document;
        }

        public async Task<Models.User> GetUserAsync(string UserId)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(_databaseId, "Users", UserId);
            var result = await _client.ReadDocumentAsync<Models.User>(documentUri);

            if (result.Document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return (dynamic)result.Document;
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
