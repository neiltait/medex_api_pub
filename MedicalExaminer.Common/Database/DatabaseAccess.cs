using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Reporting;
using MedicalExaminer.Common.Settings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;

namespace MedicalExaminer.Common.Database
{
    /// <summary>
    /// Database Access.
    /// </summary>
    /// <seealso cref="MedicalExaminer.Common.Database.IDatabaseAccess" />
    public class DatabaseAccess : IDatabaseAccess
    {
        private readonly IDocumentClientFactory _documentClientFactory;

        private readonly RequestChargeService _requestChargeService;

        private readonly bool _bypassSsl;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess"/> class.
        /// </summary>
        /// <param name="documentClientFactory">The document client factory.</param>
        /// <param name="requestChargeService">Request charge service.</param>
        /// <param name="cosmosOptions">Cosmos options.</param>
        public DatabaseAccess(IDocumentClientFactory documentClientFactory, RequestChargeService requestChargeService, IOptions<CosmosDbSettings> cosmosOptions)
        {
            _documentClientFactory = documentClientFactory;
            _requestChargeService = requestChargeService;
            _bypassSsl = cosmosOptions.Value.BypassSsl;
        }

        /// <summary>
        /// Ensures the collection is available.
        /// </summary>
        /// <param name="connectionSettings">Connection settings.</param>
        public void EnsureCollectionAvailable(IConnectionSettings connectionSettings)
        {
            var client = GetClient(connectionSettings);

            client.CreateDatabaseIfNotExistsAsync(
                new Microsoft.Azure.Documents.Database
                {
                    Id = connectionSettings.DatabaseId
                }).Wait();

            var databaseUri = UriFactory.CreateDatabaseUri(connectionSettings.DatabaseId);

            client.CreateDocumentCollectionIfNotExistsAsync(
                databaseUri,
                new DocumentCollection {Id = connectionSettings.Collection}).Wait();
        }

        private IDocumentClient GetClient(IClientSettings connectionSettings)
        {
            return _documentClientFactory.CreateClient(connectionSettings, _bypassSsl);
        }

        public async Task<T> CreateItemAsync<T>(IConnectionSettings connectionSettings, T item, bool disableAutomaticIdGeneration = false)
        {
            var client = GetClient(connectionSettings);
            var resourceResponse = await client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(
                    connectionSettings.DatabaseId,
                    connectionSettings.Collection),
                    item);
            AddAuditEntry(connectionSettings, item);

            _requestChargeService.RequestCharges.Add( new RequestChargeService.RequestCharge()
            {
                Request = $"CreateItemAsync<{typeof(T).Name}>()",
                Charge = resourceResponse.RequestCharge
            });

            return (T)(dynamic)resourceResponse.Resource;
        }

        private void AddAuditEntry<T>(IConnectionSettings connectionSettings, T item)
        {
            var auditConnectionSettings = connectionSettings.ToAuditSettings();
            var auditClient = _documentClientFactory.CreateClient(auditConnectionSettings, _bypassSsl);

            var auditEntry = new AuditEntry<T>(item);
            auditClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(
                    auditConnectionSettings.DatabaseId,
                    auditConnectionSettings.Collection),
                    auditEntry);
        }

        /// <inheritdoc/>
        public async Task<T> GetItemByIdAsync<T>(IConnectionSettings connectionSettings, string id)
        {
            try
            {
                var client = GetClient(connectionSettings);
                var documentUri = UriFactory.CreateDocumentUri(
                    connectionSettings.DatabaseId,
                    connectionSettings.Collection,
                    id);

                var response = await client.ReadDocumentAsync(documentUri);

                _requestChargeService.RequestCharges.Add(new RequestChargeService.RequestCharge()
                {
                    Request = $"GetItemByIdAsync<{typeof(T).Name}>(id={id})",
                    Charge = response.RequestCharge
                });

                return (T)(dynamic)response.Resource;
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

        public async Task<T> GetItemAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate)
        {
            try
            {
                var client = GetClient(connectionSettings);
                var feedOptions = new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true };
                var documentCollectionUri =
                    UriFactory.CreateDocumentCollectionUri(
                        connectionSettings.DatabaseId,
                        connectionSettings.Collection);

                var query = client.CreateDocumentQuery<T>(documentCollectionUri, feedOptions)
                    .Where(predicate)
                    .AsDocumentQuery();

                var results = new List<T>();
                while (query.HasMoreResults)
                {
                    var response = await query.ExecuteNextAsync<T>();

                    _requestChargeService.RequestCharges.Add(new RequestChargeService.RequestCharge()
                    {
                        Request = $"GetItemAsync<{typeof(T).Name}>(query={query})",
                        Charge = response.RequestCharge
                    });

                    results.AddRange(response);
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

        public async Task<IEnumerable<T>> GetItemsAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, dynamic>> select)
            where T : class
        {
            var client = GetClient(connectionSettings);

            var query = client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(
                        connectionSettings.DatabaseId,
                        connectionSettings.Collection),
                    new FeedOptions {MaxItemCount = -1})
                .Where(predicate)
                .Select(select)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ExecuteNextAsync<T>();

                // Unable to get RequestCharge during testing since we've casted the return type
                // to match our select. Appears to work fine on the real cosmos; something missing on the CosmosMocker.

                results.AddRange(response);
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate)
            where T : class
        {
            var client = GetClient(connectionSettings);

            var query = client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(
                        connectionSettings.DatabaseId,
                        connectionSettings.Collection),
                    new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            var resultPartCount = 0;
            while (query.HasMoreResults)
            {
                var response = await query.ExecuteNextAsync<T>();

                _requestChargeService.RequestCharges.Add(new RequestChargeService.RequestCharge()
                {
                    Request = $"GetItemsAsync<{typeof(T).Name}>({resultPartCount},query={query})",
                    Charge = response.RequestCharge
                });

                resultPartCount++;
                results.AddRange(response);
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T, TKey>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy)
            where T : class
        {
            var client = GetClient(connectionSettings);
            FeedOptions feedOptions = new FeedOptions
            {
                MaxItemCount = -1,
            };

            var query = client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId,
                        connectionSettings.Collection),
                    feedOptions)
                .Where(predicate)
                .OrderBy(orderBy)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ExecuteNextAsync<T>();

                _requestChargeService.RequestCharges.Add(new RequestChargeService.RequestCharge()
                {
                    Request = $"GetItemAsync<{typeof(T).Name}>(query={query})",
                    Charge = response.RequestCharge
                });

                results.AddRange(response);
            }

            return results;
        }

        public async Task<int> GetCountAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate)
        {
            var client = GetClient(connectionSettings);
            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection);

            var query = client.CreateDocumentQuery<T>(documentCollectionUri, feedOptions)
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ExecuteNextAsync<T>();

                _requestChargeService.RequestCharges.Add(new RequestChargeService.RequestCharge()
                {
                    Request = $"GetCountAsync<{typeof(T).Name}>(query={query})",
                    Charge = response.RequestCharge
                });

                results.AddRange(response);
            }

            return results.Count();
        }

        public async Task<T> UpdateItemAsync<T>(IConnectionSettings connectionSettings, T item)
        {
            var client = GetClient(connectionSettings);

            try
            {
                var updateItemAsync = await client.UpsertDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection),
                    item);

                _requestChargeService.RequestCharges.Add(new RequestChargeService.RequestCharge()
                {
                    Request = $"UpdateItemAsync<{typeof(T).Name}>()",
                    Charge = updateItemAsync.RequestCharge
                });

                AddAuditEntry(connectionSettings, item);
                return (T)(dynamic)updateItemAsync.Resource;
            }
            catch
            (DocumentClientException
            ex)
            {
                if
                    (
                    ex.StatusCode ==
                    HttpStatusCode.TooManyRequests)
                {
                    System.Threading.Thread.Sleep(ex.RetryAfter.Milliseconds);
                    var
                        retry =
                        await UpdateItemAsync(
                            connectionSettings,
                            item);
                    return
                        retry;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}