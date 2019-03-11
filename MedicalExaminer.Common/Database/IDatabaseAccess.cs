using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common.Database
{
    public interface IDatabaseAccess
    {
        Task<T> CreateItemAsync<T>(IConnectionSettings connectionSettings, T item,
            bool disableAutomaticIdGeneration = false);

        Task<string> Update(IConnectionSettings connectionSettings, Document document);

        Task<T> UpdateItemAsync<T>(IConnectionSettings connectionSettings, string id, T item);
        Task<T> GetItemAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetItemsAsync<T>(IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate);

        
    }
}
