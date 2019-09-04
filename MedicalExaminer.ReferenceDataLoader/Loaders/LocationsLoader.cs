using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedicalExaminer.Common.Settings;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MedicalExaminer.ReferenceDataLoader.Loaders
{
    /// <summary>
    ///     Create collection in CosmosDB and load with locations extracted from input file
    /// </summary>
    public class LocationsLoader : IHostedService, IDisposable
    {
        private readonly Uri _endpointUri;
        private readonly string _primaryKey;
        private readonly string _databaseId;
        private readonly string _importFile;
        private readonly string _containerId;
        private DocumentClient _client;
        private Uri _documentCollectionUri;
        private List<Location> _locations;
        private char[] _loadingChars = new[] { '|', '/', '-', '\\' };

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsLoader"/> class.
        /// </summary>
        /// <param name="environment">Environment.</param>
        /// <param name="cosmosDbSettings">Cosmos DB Settings.</param>
        /// <remarks>Set up details of Cosmos DB and connection to write to</remarks>
        public LocationsLoader(IHostingEnvironment environment, IOptions<CosmosDbSettings> cosmosDbSettings)
        {
            Console.WriteLine("Setting up parameters for LocationsLoader...");

            _endpointUri = new Uri(cosmosDbSettings.Value.URL);
            _primaryKey = cosmosDbSettings.Value.PrimaryKey;
            _databaseId = cosmosDbSettings.Value.DatabaseId;
            _importFile = Path.Combine(environment.ContentRootPath, "locations_full_out.json");
            _containerId = "Locations";

            Console.WriteLine("Parameters read OK for LocationsLoader...");
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Load();
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
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
            UpdateLocationPaths();
            await LoadLocations();
        }

        /// <summary>
        /// Load each location in import file into container on Cosmos DB
        /// </summary>
        /// <returns>Task.</returns>
        /// <remarks>Report progress to Console every time 1000 locations loaded</remarks>
        private async Task LoadLocations()
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

                if (loadCount % 5 == 0)
                {
                    int bar = 20;
                    int per = (int)(((loadCount % 1000) / 1000.0f) * bar);
                    Console.Write($"{_loadingChars[(loadCount / 5) % 4]} [{string.Empty.PadRight(per, '#').PadRight(bar, ' ')}]\r");
                }
            }

            var endTime = DateTime.Now;
            var processingTime = endTime - startTime;

            Console.WriteLine($"{_locations.Count} locations loaded");
            Console.WriteLine(
                $"Processing time: {processingTime.Hours} Hours;  {processingTime.Minutes} Minutes; {processingTime.Seconds} Seconds");
            Console.ReadKey();
        }

        /// <summary>
        ///     Create collection in database
        /// </summary>
        /// <returns>Task</returns>
        private async Task CreateCollection()
        {
            _client = new DocumentClient(_endpointUri, _primaryKey);
            var documentCollection = new DocumentCollection { Id = _containerId };
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _containerId);
            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, documentCollection);
        }

        /// <summary>
        ///     Load contents of json import file into list of location objects
        /// </summary>
        private void LoadImportFile()
        {
            var json = File.ReadAllText(_importFile);
            _locations = JsonConvert.DeserializeObject<List<Location>>(json);

            Console.WriteLine("Locations passed validation ...");
        }

        /// <summary>
        ///     Validate that all locations are consistent and correct
        /// </summary>
        /// <remarks>Expect this to throw an exception if locations are not valid. This exception will be handled by calling code</remarks>
        private void ValidateLocations()
        {
            var locationsChecker = new LocationsChecker(_locations);
            locationsChecker.RunAllChecks();
        }

        private void UpdateLocationPaths()
        {
            var parents = new Dictionary<string, Location>();

            foreach (var location in _locations)
            {
                var loop = location;
                while (loop != null)
                {
                    if (loop.Type == LocationType.National)
                    {
                        location.NationalLocationId = loop.LocationId;
                    }

                    if (loop.Type == LocationType.Region)
                    {
                        location.RegionLocationId = loop.LocationId;
                    }

                    if (loop.Type == LocationType.Trust)
                    {
                        location.TrustLocationId = loop.LocationId;
                    }

                    if (loop.Type == LocationType.Site)
                    {
                        location.SiteLocationId = loop.LocationId;
                    }

                    var key = loop.ParentId;
                    if (key == null)
                    {
                        break;
                    }

                    if (parents.ContainsKey(key))
                    {
                        loop = parents[key];
                    }
                    else
                    {
                        loop = _locations.FirstOrDefault(l => l.LocationId == key);

                        parents.Add(key, loop);
                    }
                }
            }
        }
    }
}