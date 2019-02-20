using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Seeders
{
    public class LocationsSeeder
    {
        public List<Location> Locations { get; private set; }

        public void LoadFromFile(string jsonFileName)
        {
            var json = File.ReadAllText(jsonFileName);
            Locations = JsonConvert.DeserializeObject<List<Location>>(json);
        }
    }
}
