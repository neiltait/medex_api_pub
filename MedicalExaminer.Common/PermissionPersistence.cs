using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace MedicalExaminer.Common
{
    public class PermissionPersistence : PersistenceBase, IPermissionPersistence
    {
        /// <inheritdoc />
        public PermissionPersistence(Uri endpointUri, string primaryKey, string databaseId)
            : base(endpointUri, primaryKey, databaseId, "Permissions")
        {
        }

        /// <summary>
        ///     Update Permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Permission> UpdatePermissionAsync(Permission permission)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionName);
            var doc = await client.UpsertDocumentAsync(documentCollectionUri, permission);

            if (doc == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return (Permission)(dynamic) doc.Resource;
        }

        /// <summary>
        ///     Create Permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionName);
            var document = await client.CreateDocumentAsync(documentCollectionUri, permission);

            if (document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return (Permission)(dynamic)document.Resource;
        }

        /// <summary>
        /// </summary>
        /// <param name="meUserId"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc />
        public async Task<Permission> GetPermissionAsync(string meUserId, string permissionId)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(databaseId, collectionName, permissionId);
            var result = await client.ReadDocumentAsync<Permission>(documentUri);

            if (result.Document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return result.Document;
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="meUserId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Permission>> GetPermissionsAsync(string meUserId)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionName);

            // build the query
            var feedOptions = new FeedOptions { MaxItemCount = -1 };
            var query = client.CreateDocumentQuery<MeUser>(
                documentCollectionUri,
                $"SELECT * FROM {collectionName} WHERE Permissions.user_id = \"{meUserId}\"",
                feedOptions);
            var queryAll = query.AsDocumentQuery();

            // combine the results
            var results = new List<Permission>();
            while (queryAll.HasMoreResults)
            {
                results.AddRange(await queryAll.ExecuteNextAsync<Permission>());
            }

            return results;
        }
    }
}