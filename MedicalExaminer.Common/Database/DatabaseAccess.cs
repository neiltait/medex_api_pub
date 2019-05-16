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
    /// <summary>
    /// Database Access.
    /// </summary>
    /// <seealso cref="MedicalExaminer.Common.Database.IDatabaseAccess" />
    public class DatabaseAccess : IDatabaseAccess
    {
        private readonly IDocumentClientFactory _documentClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess"/> class.
        /// </summary>
        /// <param name="documentClientFactory">The document client factory.</param>
        public DatabaseAccess(IDocumentClientFactory documentClientFactory)
        {
            _documentClientFactory = documentClientFactory;
        }

        public async Task<T> CreateItemAsync<T>(IConnectionSettings connectionSettings, T item, bool disableAutomaticIdGeneration = false)
        {
            var client = _documentClientFactory.CreateClient(connectionSettings);
            var resourceResponse = await client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(
                    connectionSettings.DatabaseId,
                    connectionSettings.Collection),
                    item);
            AddAuditEntry(connectionSettings, item);
            return (T)(dynamic)resourceResponse.Resource;
        }

        private void AddAuditEntry<T>(IConnectionSettings connectionSettings, T item)
        {
            var auditConnectionSettings = connectionSettings.ToAuditSettings();
            var auditClient = _documentClientFactory.CreateClient(auditConnectionSettings);

            var auditEntry = new AuditEntry<T>(item);
            auditClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(
                    auditConnectionSettings.DatabaseId,
                    auditConnectionSettings.Collection),
                    auditEntry);
        }

        public async Task<T> GetItemAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate)
        {
            try
            {
                var client = _documentClientFactory.CreateClient(connectionSettings);
                var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
                var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection);
                var queryable = client.CreateDocumentQuery<T>(documentCollectionUri, feedOptions);
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
            where T : class
        {
            var client = _documentClientFactory.CreateClient(connectionSettings);

            var query = client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(
                        connectionSettings.DatabaseId,
                        connectionSettings.Collection),
                        new FeedOptions { MaxItemCount = -1 })
                    .Where(predicate)
                    .AsDocumentQuery();

            FeedResponse<T> response = await query.ExecuteNextAsync<T>();

            var results = new List<T>();

            foreach (var t in response)
            {
                results.Add(t);
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T, TKey>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy)
            where T : class
        {
            var client = _documentClientFactory.CreateClient(connectionSettings);
            FeedOptions feedOptions = new FeedOptions
            {
                MaxItemCount = -1,
            };
            var query = client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection),
                    feedOptions)
                    .Where(predicate)
                    .OrderBy(orderBy)
                    .AsDocumentQuery();

            FeedResponse<T> response = await query.ExecuteNextAsync<T>();

            var results = new List<T>();

            foreach (var t in response)
            {
                results.Add(t);
            }

            return results;
        }

        public async Task<int> GetCountAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate)
        {
            var client = _documentClientFactory.CreateClient(connectionSettings);
            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection);
            var queryable = client.CreateDocumentQuery<T>(documentCollectionUri, feedOptions);
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
            AddAuditEntry(connectionSettings, item);
            return (T)(dynamic)updateItemAsync.Resource;
        }
    }
}