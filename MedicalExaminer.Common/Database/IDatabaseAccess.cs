using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;

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
        /// Get Item By Id.
        /// </summary>
        /// <remarks>Faster and cheaper to call than a SQL query.</remarks>
        /// <typeparam name="T">Type of document.</typeparam>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <param name="id">The ID of the document.</param>
        /// <returns>Document.</returns>
        Task<T> GetItemByIdAsync<T>(IConnectionSettings connectionSettings, string id);

        Task<T> GetItemAsync<T>(IConnectionSettings connectionSettings, Expression<Func<T, bool>> predicate);

        void EnsureCollectionAvailable(IConnectionSettings connectionSettings);

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