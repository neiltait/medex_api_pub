using System;
using System.Collections.Generic;
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
            var resourceResponse = await _client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, 
                    connectionSettings.Collection), item);
            return (T)(dynamic)resourceResponse.Resource;
        }

        public async Task<T> GetItemAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate)
        {
            try
            {
                var _client = CreateClient(connectionSettings);
                var query = _client.CreateDocumentQuery<T>(
                        UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId,
                            connectionSettings.Collection),
                        new FeedOptions {MaxItemCount = -1})
                    .Where(predicate)
                    .AsDocumentQuery();

                var results = new List<T>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<T>());
                }

                return results.FirstOrDefault();
            }
            catch (DocumentClientException documentClientException)
            {
                if (documentClientException.StatusCode == HttpStatusCode.NotFound)
                {
                    return default(T);
                }

                throw;
            }
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


        public async Task<IEnumerable<T>> GetItemsAsync<T, TKey>(IConnectionSettings connectionSettings, 
            Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy)
        {
            var _client = CreateClient(connectionSettings);
            var query = _client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection),
                    new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .OrderBy(orderBy)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public int GetCountAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate)
        {
            var _client = CreateClient(connectionSettings);
            var query = _client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId,
                        connectionSettings.Collection),
                    new FeedOptions {MaxItemCount = -1})
                .Count(predicate);
            return query;
        }


        public async Task<T> UpdateItemAsync<T>(IConnectionSettings connectionSettings, T item)
        {
            var client = CreateClient(connectionSettings);
            var updateItemAsync = await client.UpsertDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection),
                item);

            return (T)(dynamic)updateItemAsync.Resource;
        }
    }
}
