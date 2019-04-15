namespace MedicalExaminer.ReferenceDataLoader.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Models;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Newtonsoft.Json;

    /// <summary>
    ///     Create collection in CosmosDB and load with locations extracted from input file
    /// </summary>
    public class LocationsLoader
    {
        private static Uri endpointUri;
        private static string primaryKey;
        private static string databaseId;
        private static string importFile;
        private static string _containerId;
        private static DocumentClient client;
        private static Uri documentCollectionUri;
        private static List<Location> locations;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="args">0: endpoint; 1: primary key; 3: databaseId; 4: import file; 5: container id</param>
        /// <remarks>Set up details of Cosmos DB and connection to write to</remarks>
        public LocationsLoader(string[] args)
        {
            Console.WriteLine("Setting up parameters for LocationsLoader...");

            endpointUri = new Uri(args[0]);
            primaryKey = args[1];
            databaseId = args[2];
            importFile = args[3];
            _containerId = args[4];

            Console.WriteLine("Parameters read OK for LocationsLoader...");
        }

        /// <summary>
        ///     Create collection on database and load locations into it
        /// </summary>
        /// <returns>Task</returns>
        public async Task Load()
        {
            await CreateCollection();
            LoadImportFile();
            ValidateLocations();
            await LoadLocations();
        }

        /// <summary>
        ///     Create collection in database
        /// </summary>
        /// <returns>Task</returns>
        private async Task CreateCollection()
        {
            client = new DocumentClient(endpointUri, primaryKey);
            var documentCollection = new DocumentCollection { Id = _containerId };
            var databaseResult = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(databaseId);
            documentCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, _containerId);
            await client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, documentCollection);
        }

        /// <summary>
        ///     Load contents of json import file into list of location objects
        /// </summary>
        private void LoadImportFile()
        {
            var json = File.ReadAllText(importFile);
            locations = JsonConvert.DeserializeObject<List<Location>>(json);

            Console.WriteLine("Locations passed validation ...");
        }

        /// <summary>
        ///     Validate that all locations are consistenet and correct
        /// </summary>
        /// <remarks>Expect this to throw an excpetion if locations are not valid. This exception will be handled by calling code</remarks>
        private void ValidateLocations()
        {
            var locationsChecker = new LocationsChecker(locations);
            locationsChecker.RunAllChecks();
        }

        /// <summary>
        ///     Load each location in import file into container on Cosmos DB
        /// </summary>
        /// <returns></returns>
        /// <remarks>Report progress to Console evertytime 1000 locations loaded</remarks>
        private static async Task LoadLocations()
        {
            Console.WriteLine("Loading locations ...");

            var startTime = DateTime.Now;

            var loadCount = 0;
            foreach (var location in locations)
            {
                await client.UpsertDocumentAsync(documentCollectionUri, location);

                loadCount++;
                if (loadCount % 1000 == 0)
                {
                    Console.WriteLine($"loaded {loadCount} locations out of {locations.Count}...");
                }
            }

            var endTime = DateTime.Now;
            var processingTime = endTime - startTime;

            Console.WriteLine($"{locations.Count} locations loaded");
            Console.WriteLine(
                $"Processing time: {processingTime.Hours} Hours;  {processingTime.Minutes} Minutes; {processingTime.Seconds} Seconds");
            Console.ReadKey();
        }
    }
}