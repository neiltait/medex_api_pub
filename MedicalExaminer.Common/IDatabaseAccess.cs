using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common
{
    public interface IDatabaseAccess
    {
        DocumentClient CreateClient(IConnectionSettings connectionSettings);

        Task<string> Create<T>(IConnectionSettings connectionSettings, T document);

        Task<T> QuerySingleAsync<T>(
            IConnectionSettings connectionSettings,
            string documentId,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null);

        Task<T> QuerySingleOrDefaultAsync<T>(
          IDbConnection cnn,
          string sql,
          object param = null,
          IDbTransaction transaction = null,
          int? commandTimeout = null,
          CommandType? commandType = null);

        Task<IEnumerable<T>> QueryAsync<T>(
          IDbConnection cnn,
          string sql,
          object param = null,
          IDbTransaction transaction = null,
          int? commandTimeout = null,
          CommandType? commandType = null);

        Task ExecuteAsync(
          IConnectionSettings connectionSettings,
          string sql,
          object param = null,
          IDbTransaction transaction = null,
          int? commandTimeout = null,
          CommandType? commandType = null);

        Task<IEnumerable<T3>> QueryAsync<T1, T2, T3>(
          IDbConnection cnn,
          string sql,
          Func<T1, T2, T3> map,
          object param = null,
          IDbTransaction transaction = null,
          bool buffered = true,
          string splitOn = "Id",
          int? commandTimeout = null,
          CommandType? commandType = null);

        Task<IEnumerable<T4>> QueryAsync<T1, T2, T3, T4>(
          IDbConnection cnn,
          string sql,
          Func<T1, T2, T3, T4> map,
          object param = null,
          IDbTransaction transaction = null,
          bool buffered = true,
          string splitOn = "Id",
          int? commandTimeout = null,
          CommandType? commandType = null);

        IEnumerable<T> Query<T>(
          IDbConnection conn,
          string sql,
          object param = null,
          IDbTransaction transaction = null,
          bool buffered = true,
          int? commandTimeout = null,
          CommandType? commandType = null);

        IEnumerable<T3> Query<T1, T2, T3>(
          IDbConnection cnn,
          string sql,
          Func<T1, T2, T3> map,
          object param = null,
          IDbTransaction transaction = null,
          bool buffered = true,
          string splitOn = "Id",
          int? commandTimeout = null,
          CommandType? commandType = null);
    }
}
