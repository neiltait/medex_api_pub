using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace MedicalExaminer.Common
{
    /// <summary>
    ///     Manages persistence of locations
    /// </summary>
    public class LocationPersistence : PersistenceBase, ILocationPersistence
    {
        public LocationPersistence(Uri endpointUri, string primaryKey, string databaseId)
            : base(endpointUri, primaryKey, databaseId, "Locations")
        {
        }

        /// <inheritdoc />
        public async Task<Location> GetLocationAsync(string locationId)
        {
            await EnsureSetupAsync();
            DocumentResponse<Location> result = null;
            var documentUri = UriFactory.CreateDocumentUri(databaseId, "Locations", locationId);
            try
            {
                result = await client.ReadDocumentAsync<Location>(documentUri);
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null; // whatever we want as the empty resultset as it were...
                }
            }

            if (result.Document == null)
            {
                throw new ArgumentException("Invalid Argument");
            }

            return result.Document;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, "Locations");
            var feedOptions = new FeedOptions { MaxItemCount = - 1 };
            var query = client.CreateDocumentQuery<Location>(
                documentCollectionUri,
                "SELECT * FROM Locations",
                feedOptions);
            var queryAll = query.AsDocumentQuery();
            var results = new List<Location>();
            while (queryAll.HasMoreResults)
            {
                results.AddRange(await queryAll.ExecuteNextAsync<Location>());
            }

            return results;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Location>> GetLocationsByNameAsync(string locationName)
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, "Locations");
            var feedOptions = new FeedOptions { MaxItemCount = - 1 };
            var query = client.CreateDocumentQuery<Location>(
                documentCollectionUri,
                "SELECT * FROM Locations",
                feedOptions);
            var queryAll = query.AsDocumentQuery();
            var allLocations = new List<Location>();
            while (queryAll.HasMoreResults)
            {
                allLocations.AddRange(await queryAll.ExecuteNextAsync<Location>());
            }

            var result = allLocations.FindAll(location => location.Name.ToUpper().Contains(locationName.ToUpper()));
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Location>> GetLocationsByParentIdAsync(string parentId)
        {
            await EnsureSetupAsync();
            var results = new List<Location>();

            var feedOptions = new FeedOptions { MaxItemCount = - 1 };
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, "Locations");
            var queryString = $"SELECT * FROM Locations WHERE Locations.parentId = \"{parentId}\"";
            var query = client.CreateDocumentQuery<Location>(documentCollectionUri, queryString, feedOptions);
            var queryAll = query.AsDocumentQuery();
            var childLocations = new List<Location>();
            while (queryAll.HasMoreResults)
            {
                childLocations.AddRange(await queryAll.ExecuteNextAsync<Location>());
            }

            results.AddRange(childLocations);

            if (childLocations.Count > 0)
            {
                await GetNextLevelOfDescendents(results, results, feedOptions, documentCollectionUri);
            }

            return results;
        }

        /// <summary>
        ///     Get next level of descendents for locations in parents list and adds these to results
        /// </summary>
        /// <param name="parents">list of locations whose immediate descendents are to be got</param>
        /// <param name="results">
        ///     list of locations that have already been identified to be returned. This method appends to this
        ///     list the immediate descendents that are found
        /// </param>
        /// <param name="feedOptions">FeedOptions instance</param>
        /// <param name="documentCollectionUri">DocumentCollection Uri</param>
        /// <returns>Task</returns>
        /// <remarks>Uses recursion to descend levels of descendents until lowest level reached</remarks>
        private async Task GetNextLevelOfDescendents(
            List<Location> parents,
            List<Location> results,
            FeedOptions feedOptions,
            Uri documentCollectionUri)
        {
            var childLocations = new List<Location>();

            // Load parents in batches to prevent exceeding length of queryString permitted by CreateDocumentQuery
            var initialPosition = 0; // Starting item in list to be processed in this batch. Initially zero
            const int batchSize = 500; // Number of items to be processed in one batch
            var finalPosition = initialPosition + batchSize; // Stop point for loading items in this batch
            var itemsLoaded = 0; // Number of items loaded in total.Initially zero

            while (itemsLoaded < parents.Count)
            {
                var queryString = new StringBuilder();
                queryString.Append("SELECT * FROM Locations WHERE Locations.parentId IN (");

                for (var count = initialPosition; count < parents.Count && count < finalPosition; count ++)
                {
                    var lastItemInThisBatch = Math.Min(parents.Count - 1, finalPosition - 1);
                    queryString.Append($"\"{parents[count].LocationId}\"");
                    if (count < lastItemInThisBatch)
                    {
                        queryString.Append(", ");
                    }
                    else
                    {
                        queryString.Append(")");
                    }
                }

                var query = client.CreateDocumentQuery<Location>(
                    documentCollectionUri,
                    queryString.ToString(),
                    feedOptions);
                var queryAll = query.AsDocumentQuery();

                while (queryAll.HasMoreResults)
                {
                    childLocations.AddRange(await queryAll.ExecuteNextAsync<Location>());
                }

                initialPosition = finalPosition;
                finalPosition += batchSize;
                itemsLoaded += batchSize;
            }

            if (childLocations.Count > 0)
            {
                results.AddRange(childLocations);
                await GetNextLevelOfDescendents(childLocations, results, feedOptions, documentCollectionUri);
            }
        }
    }
}