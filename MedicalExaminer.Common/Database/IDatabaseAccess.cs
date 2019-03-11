using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common.Database
{
    public interface IDatabaseAccess
    {
        Task<T> CreateItemAsync<T>(IConnectionSettings connectionSettings, T item,
            bool disableAutomaticIdGeneration = false);

        Task<T> GetItemAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetItemsAsync<T>(IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate);

        //Task<IEnumerable<U>> GetItemsAsync<T, U>(IConnectionSettings connectionSettings,
        //    Func<Expression<Func<T, bool>>, U> predicate);

        //Task<IEnumerable<T>> QueryAsync<T>(
        //    IConnectionSettings connectionSettings,
        //    string queryString);

        //Task<IEnumerable<U>> GetItemPartAsync<T, U>(IConnectionSettings connectionSettings,
        //    Expression<Func<T, Func<U, bool>>> predicate);
    }
}
