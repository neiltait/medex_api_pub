using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Seeders
{
    /// <summary>
    /// Adds location objects to database from source file
    /// </summary>
    public class LocationsSeeder
    {
        /// <summary>
        /// List of location objects that this class will create
        /// </summary>
        public List<Location> Locations { get; private set; }

        /// <summary>
        /// Create Locations from file
        /// </summary>
        /// <param name="jsonFileName">full name of source path</param>
        public void LoadFromFile(string jsonFileName)
        {
            var json = File.ReadAllText(jsonFileName);
            Locations = JsonConvert.DeserializeObject<List<Location>>(json);
        }
    }
}
