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

        /// <summary>
        /// Updates the list of items.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="items">The items.</param>
        /// <returns>The list of updated items.</returns>
        Task<IEnumerable<T>> UpdateItemsAsync<T>(IConnectionSettings connectionSettings, IEnumerable<T> items);

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