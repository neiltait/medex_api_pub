using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Medical_Examiner_API.Persistence
{
    public class UserPersistence : IUserPersistence
    {
        private DocumentClient _client;

        private readonly string _databaseId;
        private readonly Uri _endpointUri;
        private readonly string _primaryKey;

        public UserPersistence(Uri endpointUri, string primaryKey)
        {
            _databaseId = "testing123";
            _endpointUri = endpointUri;
            _primaryKey = primaryKey;
        }

        public async Task<MeUser> UpdateUserAsync(MeUser meUser)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Users");
            var doc = await _client.UpsertDocumentAsync(documentCollectionUri, meUser);

            if (doc == null) throw new ArgumentException("Invalid Argument");

            return (MeUser) doc;
        }

        public async Task<MeUser> CreateUserAsync(MeUser meUser)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Users");
            var document = await _client.CreateDocumentAsync(documentCollectionUri, meUser);

            if (document == null) throw new ArgumentException("Invalid Argument");

            return (MeUser) document;
        }

        public async Task<MeUser> GetUserAsync(string UserId)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(_databaseId, "Users", UserId);
            var result = await _client.ReadDocumentAsync<MeUser>(documentUri);

            if (result.Document == null) throw new ArgumentException("Invalid Argument");

            return result.Document;
        }

        public async Task<IEnumerable<MeUser>> GetUsersAsync()
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "Users");

            // build the query
            var feedOptions = new FeedOptions {MaxItemCount = -1};
            var query = _client.CreateDocumentQuery<MeUser>(documentCollectionUri, "SELECT * FROM Users", feedOptions);
            var queryAll = query.AsDocumentQuery();

            // combine the results
            var results = new List<MeUser>();
            while (queryAll.HasMoreResults) results.AddRange(await queryAll.ExecuteNextAsync<MeUser>());

            return results;
        }

        public async Task EnsureSetupAsync()
        {
            if (_client == null) _client = new DocumentClient(_endpointUri, _primaryKey);

            await _client.CreateDatabaseIfNotExistsAsync(new Database {Id = _databaseId});
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);

            // Samples
            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection {Id = "Users"});
        }
    }
}