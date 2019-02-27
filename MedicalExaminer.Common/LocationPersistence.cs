using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace MedicalExaminer.Common
{
    /// <summary>
    ///Manages persistence of locations
    /// </summary>
    public class LocationPersistence : PersistenceBase, ILocationPersistence
    {
        public LocationPersistence(Uri endpointUri, string primaryKey, string databaseId) : base(endpointUri,
            primaryKey, databaseId, "Locations")
        {
        }

        /// <inheritdoc />
        public async Task<Location> GetLocationAsync(string locationId)
        {
            await EnsureSetupAsync();
            var documentUri = UriFactory.CreateDocumentUri(DatabaseId, "Locations", locationId);
            var result = await Client.ReadDocumentAsync<Location>(documentUri);
            if (result.Document == null) throw new ArgumentException("Invalid Argument");
            return result.Document;

        }

        /// <inheritdoc />
        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, "Locations");
            var feedOptions = new FeedOptions { MaxItemCount = -1 };
            var query = Client.CreateDocumentQuery<Location>(documentCollectionUri, "SELECT * FROM Locations",
                feedOptions);
            var queryAll = query.AsDocumentQuery();
            var results = new List<Location>();
            while (queryAll.HasMoreResults) results.AddRange(await queryAll.ExecuteNextAsync<Location>());
            return results;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Location>> GetLocationsByNameAsync(string locationName)
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, "Locations");
            var feedOptions = new FeedOptions { MaxItemCount = -1 };
            var query = Client.CreateDocumentQuery<Location>(documentCollectionUri, "SELECT * FROM Locations",
                feedOptions);
            var queryAll = query.AsDocumentQuery();
            var allLocations = new List<Location>();
                while (queryAll.HasMoreResults) allLocations.AddRange(await queryAll.ExecuteNextAsync<Location>());
            var result = allLocations.FindAll( location => location.Name.ToUpper().Contains(locationName.ToUpper()));

            return result;
        }
    }
}
