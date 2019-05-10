using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace MedicalExaminer.Common.Database
{
    /// <summary>
    /// Database Access Interface.
    /// </summary>
    public interface IDatabaseAccess
    {
        Task<T> CreateItemAsync<T>(
            IConnectionSettings connectionSettings,
            T item,
            bool disableAutomaticIdGeneration = false);

        Task<T> UpdateItemAsync<T>(IConnectionSettings connectionSettings, T item);

        Task<T> GetItemAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetItemsAsync<T>(
            IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate)
            where T : class;

        Task<IEnumerable<T>> GetItemsAsync<T, TKey>(IConnectionSettings connectionSettings,
            Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy)
            where T : class;

        Task<int> GetCountAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);

    }
}