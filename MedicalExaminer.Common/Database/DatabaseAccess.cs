using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace MedicalExaminer.Common.Database
{
    public class DatabaseAccess : IDatabaseAccess
    {
        private DocumentClient CreateClient(IConnectionSettings connectionSettings)
        {
            var Client = new DocumentClient(connectionSettings.EndPointUri, connectionSettings.PrimaryKey);

            Client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = connectionSettings.DatabaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(connectionSettings.DatabaseId);

            Client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection { Id = connectionSettings.Collection });

            return Client;
        }

        public async Task<T> CreateItemAsync<T>(IConnectionSettings connectionSettings, T item, bool disableAutomaticIdGeneration = false)
        {
            var _client = CreateClient(connectionSettings);
            var resourceResponse = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection), item, disableAutomaticIdGeneration: disableAutomaticIdGeneration);
            return (T)(dynamic)resourceResponse.Resource;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(IConnectionSettings connectionSettings, string queryString)
        {
            var Client = CreateClient(connectionSettings);
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection);

            var feedOptions = new FeedOptions { MaxItemCount = -1 };
            var query = Client.CreateDocumentQuery<T>(documentCollectionUri, queryString,
                feedOptions);
            var queryAll = query.AsDocumentQuery();
            var results = new List<T>();
            while (queryAll.HasMoreResults) results.AddRange(await queryAll.ExecuteNextAsync<T>());
            return results;
        }

        public async Task<T> GetItemAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate)
        {
            var _client = CreateClient(connectionSettings);
            var query = _client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection),
                    new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate)
        {
            var _client = CreateClient(connectionSettings);
            var query = _client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection),
                    new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<string> Update(IConnectionSettings connectionSettings, Document document)
        {
            var client = CreateClient(connectionSettings);
            var response = await client.ReplaceDocumentAsync(document);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            return response.Resource.Id;
        }


        public async Task<T> UpdateItemAsync<T>(IConnectionSettings connectionSettings, string id, T item)
        {
            var _client = CreateClient(connectionSettings);
            var updateItemAsync = _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(connectionSettings.DatabaseId, connectionSettings.Collection, id), item);
            return (T)(dynamic)updateItemAsync.Result.Resource;
        }
    }
}
