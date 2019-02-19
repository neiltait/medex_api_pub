using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Seeders;

namespace Medical_Examiner_API.Persistence
{
    public class LocationsSeederPersistence : ILocationsSeederPersistence
    {
        private string _databaseId;
        private Uri _endpointUri;
        private string _primaryKey;
        private DocumentClient _client;
        private readonly string _Id = "Locations";


        public LocationsSeederPersistence(Uri endpointUri, string primaryKey)
        {
            _databaseId = "testing123";
            _endpointUri = endpointUri;
            _primaryKey = primaryKey;
        }

        public async Task EnsureSetupAsync()
        {
            if (_client == null)
            {
                _client = new DocumentClient(_endpointUri, _primaryKey);
            }

            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);

    
            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = _Id});
        }

        public async Task<bool> SaveLocationAsync(Location location)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _Id);
            await _client.UpsertDocumentAsync(documentCollectionUri, location);

            return true;
        }
    }
}
