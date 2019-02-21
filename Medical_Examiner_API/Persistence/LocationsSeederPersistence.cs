using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Medical_Examiner_API.Persistence
{
    /// <summary>
    /// Persistence class used by location seeder
    /// </summary>
    public class LocationsSeederPersistence : ILocationsSeederPersistence
    {
        private readonly string _id = "Locations";
        private string _databaseId;
        private Uri _endpointUri;
        private string _primaryKey;
        private DocumentClient _client;
        private DocumentCollection _documentCollection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpointUri">URI of Cosmos DB</param>
        /// <param name="primaryKey">key required for DB connection</param>
        public LocationsSeederPersistence(Uri endpointUri, string primaryKey)
        {
            _databaseId = "testing123";
            _endpointUri = endpointUri;
            _primaryKey = primaryKey;
        }

        /// <summary>
        /// Sets up to Cosmsos DB
        /// </summary>
        /// <returns>bool</returns>
        public async Task EnsureSetupAsync()
        {
            if (_client == null)
            {
                _client = new DocumentClient(_endpointUri, _primaryKey);
            }

            if (_documentCollection == null)
            {
                _documentCollection = new DocumentCollection() { Id = _id };
            }

            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);

            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, _documentCollection);
        }

        

        /// <summary>
        /// Writes location object to database
        /// </summary>
        /// <param name="location">location object to be written to database</param>
        /// <returns>bool</returns>
        public async Task<bool> SaveLocationAsync(Location location)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _id);
            await _client.UpsertDocumentAsync(documentCollectionUri, location);

            return true;
        }

        /// <summary>
        /// Write list of location objects to database
        /// </summary>
        /// <param name="locations">list of location objects</param>
        /// <returns>bool</returns>
        public async Task<bool> SaveAllLocationsAsync(IList<Location> locations)
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _id);

            foreach (var l in locations)
            {
                await _client.UpsertDocumentAsync(documentCollectionUri, l);
            }

            return true;
        }
    }
}
