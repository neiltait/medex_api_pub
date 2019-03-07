using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace MedicalExaminer.Common
{
    public class DatabaseAccess : IDatabaseAccess
    {
        public DocumentClient CreateClient(IConnectionSettings connectionSettings)
        {
            var Client = new DocumentClient(connectionSettings.EndPointUri, connectionSettings.PrimaryKey);

            Client.CreateDatabaseIfNotExistsAsync(new Database { Id = connectionSettings.DatabaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(connectionSettings.DatabaseId);

            Client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection { Id = connectionSettings.Collection });

            return Client;
        }

        public async Task<string> Create<T>(IConnectionSettings connectionSettings, T document)
        {
            var clientX = CreateClient(connectionSettings);
            var response = await clientX.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(connectionSettings.DatabaseId, connectionSettings.Collection), document);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                return null;
            }
            
            return response.Resource.Id;
        }

        public async Task<T> QuerySingleAsync<T>(IConnectionSettings connectionSettings, string documentId,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
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

        public Task<T> QuerySingleOrDefaultAsync<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> QueryAsync<T>(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(IConnectionSettings connectionSettings, string sql, object param = null,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }


        public Task ExecuteAsync(IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T3>> QueryAsync<T1, T2, T3>(IDbConnection cnn, string sql, Func<T1, T2, T3> map, object param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T4>> QueryAsync<T1, T2, T3, T4>(IDbConnection cnn, string sql, Func<T1, T2, T3, T4> map, object param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Query<T>(IDbConnection conn, string sql, object param = null, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T3> Query<T1, T2, T3>(IDbConnection cnn, string sql, Func<T1, T2, T3> map, object param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
            CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }
    }
}
