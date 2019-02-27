using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace MedicalExaminer.Common
{
    public class UserPersistence : PersistenceBase, IUserPersistence
    {
        public UserPersistence(Uri endpointUri, string primaryKey, string databaseId) : base(endpointUri, primaryKey, databaseId, "Users")
        {
        }

        public async Task<MeUser> UpdateUserAsync(MeUser meUser)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionName);
            var doc = await Client.UpsertDocumentAsync(documentCollectionUri, meUser);

            if (doc == null) throw new ArgumentException("Invalid Argument");

            return (MeUser) doc;
        }

        public async Task<MeUser> CreateUserAsync(MeUser meUser)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionName);
            var document = await Client.CreateDocumentAsync(documentCollectionUri, meUser);

            if (document == null) throw new ArgumentException("Invalid Argument");

            return (MeUser) document;
        }

        public async Task<MeUser> GetUserAsync(string UserId)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(DatabaseId, CollectionName, UserId);
            var result = await Client.ReadDocumentAsync<MeUser>(documentUri);

            if (result.Document == null) throw new ArgumentException("Invalid Argument");

            return result.Document;
        }

        public async Task<IEnumerable<MeUser>> GetUsersAsync()
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, "Users");

            // build the query
            var feedOptions = new FeedOptions {MaxItemCount = -1};
            var query = Client.CreateDocumentQuery<MeUser>(documentCollectionUri, "SELECT * FROM Users", feedOptions);
            var queryAll = query.AsDocumentQuery();

            // combine the results
            var results = new List<MeUser>();
            while (queryAll.HasMoreResults) results.AddRange(await queryAll.ExecuteNextAsync<MeUser>());

            return results;
        }
    }
}