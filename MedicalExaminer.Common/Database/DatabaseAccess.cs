using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public DocumentClient CreateClient(IConnectionSettings connectionSettings)
        {
            var Client = new DocumentClient(connectionSettings.EndPointUri, connectionSettings.PrimaryKey);

            Client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database { Id = connectionSettings.DatabaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(connectionSettings.DatabaseId);

            Client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection { Id = connectionSettings.Collection });

            return Client;
        }

        public async Task<string> Create<T>(IConnectionSettings connectionSettings, T document)
        {
            var client = CreateClient(connectionSettings);
            var response = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection), document);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                return null;
            }
            
            return response.Resource.Id;
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

        public async Task<T> QuerySingleAsync<T>(IConnectionSettings connectionSettings, string documentId)
        {
            var Client = CreateClient(connectionSettings);

            DocumentResponse<T> result = null;
            var documentUri = UriFactory.CreateDocumentUri(connectionSettings.DatabaseId, connectionSettings.Collection,
                documentId);

            try
            {
                result = await Client.ReadDocumentAsync<T>(documentUri);
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return default(T);
                }

                throw;
            }
            
            return result.Document;
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(IDbConnection cnn, string sql)
        {
            throw new NotImplementedException();
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

        public async Task<T> QueryAsyncOne<T>(IConnectionSettings connectionSettings, string queryString, object param = null)
        {
            var Client = CreateClient(connectionSettings);
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection);

            var feedOptions = new FeedOptions { MaxItemCount = -1 };
            var query = Client.CreateDocumentQuery<T>(documentCollectionUri, queryString,
                feedOptions);
            var queryAll = query.AsDocumentQuery();
            var results = new List<T>();
            while (queryAll.HasMoreResults) results.AddRange(await queryAll.ExecuteNextAsync<T>());
            return results.FirstOrDefault();
        }
    }
}
