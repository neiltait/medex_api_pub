using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace MedicalExaminer.Common
{
    /// <summary>
    ///     Manages persistence of users
    /// </summary>
    public class UserPersistence : PersistenceBase, IUserPersistence
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserPersistence" /> class.
        /// </summary>
        /// <param name="endpointUri">COSMOS DB endpoint URI</param>
        /// <param name="primaryKey">Primary key for COSMOS DB instance</param>
        /// <param name="databaseId">DatabaseId</param>
        public UserPersistence(Uri endpointUri, string primaryKey, string databaseId)
            : base(endpointUri, primaryKey, databaseId, "Users")
        {
        }

        /// <inheritdoc />
        public async Task<MeUser> UpdateUserAsync(MeUser meUser)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionName);
            var doc = await client.UpsertDocumentAsync(documentCollectionUri, meUser);

            if (doc == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return (MeUser)(dynamic)doc;
        }

        /// <inheritdoc />
        public async Task<MeUser> CreateUserAsync(MeUser meUser)
        {
            await EnsureSetupAsync();

            meUser.UserId = Guid.NewGuid().ToString();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionName);
            var document = await client.CreateDocumentAsync(documentCollectionUri, meUser);

            if (document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            var user = (MeUser)(dynamic)document;
            return user;
        }

        /// <inheritdoc />
        public async Task<MeUser> GetUserAsync(string userId)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(databaseId, collectionName, userId);
            var result = await client.ReadDocumentAsync<MeUser>(documentUri);

            if (result.Document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return result.Document;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MeUser>> GetUsersAsync()
        {
            var results = await GetUsersByRole(null);
            return results;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MeUser>> GetMedicalExaminersAsync()
        {
            var filter = new UserRolesContainer
            {
                Role = UserRoles.MedicalExaminer
            };
            var results = await GetUsersByRole(filter);
            return results;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MeUser>> GetMedicalExaminerOfficerAsync()
        {
            var filter = new UserRolesContainer
            {
                Role = UserRoles.MedicalExaminerOfficer
            };
            var results = await GetUsersByRole(filter);
            return results;
        }

        /// <summary>
        ///     Gets all users and filters bu UserRole as defined by userRoleContainer
        /// </summary>
        /// <param name="userRoleContainer">Used to filter results</param>
        /// <returns>List of users</returns>
        /// <remarks>If userRoleContainer is null then no filtering takes place</remarks>
        private async Task<IEnumerable<MeUser>> GetUsersByRole(UserRolesContainer userRoleContainer)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, "Users");

            // build the query
            var feedOptions = new FeedOptions { MaxItemCount = -1 };
            var query = client.CreateDocumentQuery<MeUser>(
                documentCollectionUri,
                "SELECT * FROM Users ORDER BY Users.last_name",
                feedOptions);
            var queryAll = query.AsDocumentQuery();

            // combine the results
            var results = new List<MeUser>();
            while (queryAll.HasMoreResults)
            {
                results.AddRange(await queryAll.ExecuteNextAsync<MeUser>());
            }

            // Filter results to required type if required
            // if no filtering required then return all results
            if (userRoleContainer == null)
            {
                return results;
            }

            var resultsFiltered = results.FindAll(r => r.UserRole == userRoleContainer.Role);
            return resultsFiltered;
        }
    }
}