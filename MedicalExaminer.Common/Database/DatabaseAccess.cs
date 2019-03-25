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
        private readonly IDocumentClientFactory _documentClientFactory;
        public DatabaseAccess(IDocumentClientFactory documentClientFactory)
        {
            _documentClientFactory = documentClientFactory;
        }

        public async Task<T> CreateItemAsync<T>(IConnectionSettings connectionSettings, T item, bool disableAutomaticIdGeneration = false)
        {
            var _client = _documentClientFactory.CreateClient(connectionSettings);
            var resourceResponse = await _client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, 
                    connectionSettings.Collection), item);
            return (T)(dynamic)resourceResponse.Resource;
        }

        public async Task<T> GetItemAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate)
        {
            try
            {
                var _client = _documentClientFactory.CreateClient(connectionSettings);
                var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
                var queryable = _client.CreateDocumentQuery<T>(connectionSettings.EndPointUri, feedOptions);
                IQueryable<T> filter = queryable.Where(predicate);
                IDocumentQuery<T> query = filter.AsDocumentQuery();
                
                var results = new List<T>();
                results.AddRange(await query.ExecuteNextAsync<T>());
                
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

        public async Task<IEnumerable<T>> GetItemsAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate)
        {
            var _client = _documentClientFactory.CreateClient(connectionSettings);
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
            var _client = _documentClientFactory.CreateClient(connectionSettings);
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

        public async Task<int> GetCountAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate)
        {
            var _client = _documentClientFactory.CreateClient(connectionSettings);
            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            var queryable = _client.CreateDocumentQuery<T>(connectionSettings.EndPointUri, feedOptions);
            IQueryable<T> filter = queryable.Where(predicate);
            IDocumentQuery<T> query = filter.AsDocumentQuery();
            var results = new List<T>();
            
                results.AddRange(await query.ExecuteNextAsync<T>());
            
            return results.Count;
        }


        public async Task<T> UpdateItemAsync<T>(IConnectionSettings connectionSettings, T item)
        {
            var client = _documentClientFactory.CreateClient(connectionSettings);
            var updateItemAsync = await client.UpsertDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection),
                item);

            return (T)(dynamic)updateItemAsync.Resource;
        }

        private DocumentClient CreateClient(IConnectionSettings connectionSettings)
        {
            var client = new DocumentClient(connectionSettings.EndPointUri, connectionSettings.PrimaryKey);

            client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database
                { Id = connectionSettings.DatabaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(connectionSettings.DatabaseId);

            client.CreateDocumentCollectionIfNotExistsAsync(
                databaseUri,
                new DocumentCollection { Id = connectionSettings.Collection });

            return client;
        }
    }
}