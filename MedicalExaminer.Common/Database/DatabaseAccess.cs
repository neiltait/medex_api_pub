using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Cosmonaut.Extensions;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;

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
                var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection);
                var queryable = _client.CreateDocumentQuery<T>(documentCollectionUri, feedOptions);
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

        public async Task<(IEnumerable<T> result, string ContinuationToken)> GetItemsAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate,
            int pageSize = 25, int pageNumber = 1, string pagingToken = "")
            where T : class
        {
            var tempToken = pagingToken;
            var _client = _documentClientFactory.CreateClient(connectionSettings);


            var store = _documentClientFactory.CreateCosmosStore<T>(connectionSettings);

            var result = store.Query().Where(predicate).WithPagination(pageNumber, pageSize);

            //var query = _client.CreateDocumentQuery<T>(
            //        UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId,
            //        connectionSettings.Collection),
            //        new FeedOptions
            //        {
            //            MaxItemCount = pageSize,
            //            RequestContinuation = pagingToken ?? string.Empty
            //        })
            //    .Where(predicate)
            //.AsDocumentQuery();


            

            //FeedResponse<T> response = await query.ExecuteNextAsync<T>();

            //var results = new List<T>();
            
            //foreach(var t in response)
            //{
            //    results.Add(t);
            //}
            //var continuationToken = response.ResponseContinuation;
            return (result, null);
        }


        public async Task<(IEnumerable<T> result, string ContinuationToken)> GetItemsAsync<T, TKey>(IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy,
            int pageSize = 25, int pageNumber = 1, string pagingToken = null)
            where T : class
        {

            //var _client = _documentClientFactory.CreateClient(connectionSettings);
            //var query = _client.CreateDocumentQuery<T>(
            //        UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection),
            //        new FeedOptions {
            //            MaxItemCount = pageSize,
            //            RequestContinuation = pagingToken ?? string.Empty
            //        })
            //    .Where(predicate)
            //    .OrderBy(orderBy)
            //    .AsDocumentQuery();

            //FeedResponse<T> response = await query.ExecuteNextAsync<T>();

            //var results = new List<T>();

            //foreach (var t in response)
            //{
            //    results.Add(t);
            //}
            //var continuationToken = response.ResponseContinuation;
            var store = _documentClientFactory.CreateCosmosStore<T>(connectionSettings);

            var result = await store.Query().Where(predicate).WithPagination(pageNumber, pageSize).ToListAsync();
            return (result, null);
        }

        public async Task<int> GetCountAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate)
        {
            var _client = _documentClientFactory.CreateClient(connectionSettings);
            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection);
            var queryable = _client.CreateDocumentQuery<T>(documentCollectionUri, feedOptions);
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
    }
}