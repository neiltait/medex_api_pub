using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;

namespace MedicalExaminer.Common.Services
{
    /// <summary>
    /// Query Handler.
    /// </summary>
    /// <typeparam name="TQuery">Query Reqyest</typeparam>
    /// <typeparam name="TResult">Query Response</typeparam>
    public abstract class QueryHandler<TQuery, TResult>
        : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Initialise a new instance of the Query Handler.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        protected QueryHandler(IDatabaseAccess databaseAccess, IConnectionSettings connectionSettings)
        {
            DatabaseAccess = databaseAccess;
            ConnectionSettings = connectionSettings;
        }

        /// <summary>
        /// Connection Settings.
        /// </summary>
        protected IConnectionSettings ConnectionSettings { get; }

        /// <summary>
        /// Database Access.
        /// </summary>
        protected IDatabaseAccess DatabaseAccess { get; }

        /// <summary>
        /// Handle the Query.
        /// </summary>
        /// <param name="param">Query Request</param>
        /// <returns>Task of <see cref="TResult"/>.</returns>
        public abstract Task<TResult> Handle(TQuery param);

        /// <summary>
        /// Create Item Async.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="disableAutomaticIdGeneration"></param>
        /// <returns></returns>
        protected Task<TResult> CreateItemAsync(
            TResult item,
            bool disableAutomaticIdGeneration = false)
        {
            return DatabaseAccess.CreateItemAsync(ConnectionSettings, item, disableAutomaticIdGeneration);
        }

        /// <summary>
        /// Update Item Async.
        /// </summary>
        /// <param name="item">Item to update.</param>
        /// <returns>The <see cref="TResult"/></returns>
        protected Task<TResult> UpdateItemAsync(TResult item)
        {
            return DatabaseAccess.UpdateItemAsync(ConnectionSettings, item);
        }

        /// <summary>
        /// Get Item Async.
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <returns>A <see cref="TResult"/>.</returns>
        protected Task<TResult> GetItemAsync(Expression<Func<TResult, bool>> predicate)
        {
            return DatabaseAccess.GetItemAsync(ConnectionSettings, predicate);
        }

        /// <summary>
        /// Get Items Async.
        /// </summary>
        /// <typeparam name="T">Query Reqyest.</typeparam>
        /// <param name="predicate">Predicate.</param>
        /// <returns>A list of <see cref="TResult"/>.</returns>
        protected Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate)
        {
            return DatabaseAccess.GetItemsAsync(ConnectionSettings, predicate);
        }
    }
}
