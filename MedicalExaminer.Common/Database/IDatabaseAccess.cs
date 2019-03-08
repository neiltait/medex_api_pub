using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common.Database
{
    public interface IDatabaseAccess
    {
        DocumentClient CreateClient(IConnectionSettings connectionSettings);

        Task<string> Create<T>(IConnectionSettings connectionSettings, T document);

        Task<T> QuerySingleAsync<T>(
            IConnectionSettings connectionSettings,
            string documentId);

        Task<T> QuerySingleOrDefaultAsync<T>(
          IDbConnection cnn,
          string sql);

        Task<IEnumerable<T>> QueryAsync<T>(
            IConnectionSettings connectionSettings,
            string queryString);

        Task<T> QueryAsyncOne<T>(
            IConnectionSettings connectionSettings, 
            string queryString, 
            object param = null);


    }
}
