using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace MedicalExaminer.ReferenceDataLoader.Loaders
{
    /// <summary>
    /// Create collection in CosmosDB and load with locations extracted from input file
    /// </summary>
    public class LocationsLoader
    {
        private static Uri _endpointUri;
        private static string _primaryKey;
        private static string _databaseId;
        private static string _importFile;
        private static string _containerID;
        private static DocumentClient _client;
        private static Uri _documentCollectionUri;
        private static List<Location> _locations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="args">0: endpoint; 1: primary key; 3: databaseId; 4: import file; 5: container id</param>
        /// <remarks>Set up details of Cosmos DB and connection to write to</remarks>
        public LocationsLoader(string[] args)
        {
            Console.WriteLine("Setting up parameters for LocationsLoader...");

            _endpointUri = new Uri(args[0]);
            _primaryKey = args[1];
            _databaseId = args[2];
            _importFile = args[3];
            _containerID = args[4];

            Console.WriteLine("Parameters read OK for LocationsLoader...");
        }

        /// <summary>
        /// Create collection on database and load locations into it
        /// </summary>
        /// <returns>Task</returns>
        public  async Task Load()
        {
            await CreateCollection();
            LoadImportFile();
            ValidateLocations();
            await LoadLocations();
        }

        /// <summary>
        /// Create collection in database
        /// </summary>
        /// <returns>Task</returns>
        private async Task CreateCollection()
        {
            _client = new DocumentClient(_endpointUri, _primaryKey);
            var documentCollection = new DocumentCollection() { Id = _containerID };
            var databaseResult = await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _containerID);
            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, documentCollection);
        }

        /// <summary>
        /// Load contents of json import file into list of location objects
        /// </summary>
        private void LoadImportFile()
        {
            var json = File.ReadAllText(_importFile);
            _locations = JsonConvert.DeserializeObject<List<Location>>(json);

            Console.WriteLine("Locations passed validation ...");
        }

        /// <summary>
        /// Validate that all locations are consistenet and correct
        /// </summary>
        /// <remarks>Expect this to throw an excpetion if locations are not valid. This exception will be handled by calling code</remarks>
        private void ValidateLocations()
        {
            var locationsChecker = new LocationsChecker(_locations);
            locationsChecker.RunAllChecks();
        }

        /// <summary>
        /// Load each location in import file into container on Cosmos DB
        /// </summary>
        /// <returns></returns>
        /// <remarks>Report progress to Console evertytime 1000 locations loaded</remarks>
        private static async Task LoadLocations()
        {
            Console.WriteLine("Loading locations ...");

            var startTime = DateTime.Now;
            

            var loadCount = 0;
            foreach (var location in _locations)
            {
                await _client.UpsertDocumentAsync(_documentCollectionUri, location);

                loadCount++;
                if (loadCount % 1000 == 0)
                {
                    Console.WriteLine($"loaded {loadCount} locations out of {_locations.Count}...");
                }
            }

            var endTime = DateTime.Now;
            var processingTime = endTime - startTime;

            Console.WriteLine($"{_locations.Count} locations loaded");
            Console.WriteLine($"Processing time: {processingTime.Hours} Hours;  {processingTime.Minutes} Minutes; {processingTime.Seconds} Seconds");
            Console.ReadKey();

        }
    }
}
