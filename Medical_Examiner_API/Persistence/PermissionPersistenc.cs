using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Medical_Examiner_API.Persistence
{
    public class PermissionPersistence : PersistenceBase, IPermissionPersistence
    {
        public PermissionPersistence(Uri endpointUri, string primaryKey, string databaseId)
            : base(endpointUri, primaryKey, databaseId, "Permissions")
        {
        }

        /// <summary>
        /// Update Permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Permission> UpdatePermissionAsync(Permission permission)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionName);
            var doc = await Client.UpsertDocumentAsync(documentCollectionUri, permission);

            if (doc == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return (Permission) doc;
        }

        /// <summary>
        /// Create Permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionName);
            var document = await Client.CreateDocumentAsync(documentCollectionUri, permission);

            if (document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return (Permission) document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Permission> GetPermissionAsync(string meUserId, string permissionId)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(DatabaseId, CollectionName, permissionId);
            var result = await Client.ReadDocumentAsync<Permission>(documentUri);

            if (result.Document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return result.Document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Permission>> GetPermissionsAsync(string meUserId)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionName);

            // build the query
            var feedOptions = new FeedOptions { MaxItemCount = -1};
            var query = Client.CreateDocumentQuery<MeUser>(documentCollectionUri, $"SELECT * FROM {CollectionName} WHERE user_id = {meUserId}", feedOptions);
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