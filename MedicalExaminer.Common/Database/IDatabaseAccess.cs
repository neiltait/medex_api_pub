using System;
using System.Collections.Generic;
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

        Task<T> UpdateItemAsync<T>(IConnectionSettings connectionSettings, T item);
        Task<T> GetItemAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetItemsAsync<T>(IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetItemsAsync<T, TKey>(IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy);


        int GetCountAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);

    }
}
