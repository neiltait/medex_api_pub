using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.ToolBox.SeedDatabase
{
    /// <summary>
    /// Seed Database Settings.
    /// </summary>
    class SeedDatabaseSettings
    {
        /// <summary>
        /// Path to look for JSON files for Users.
        /// </summary>
        public string ImportUsersFrom { get; set; }

        /// <summary>
        /// Path to look for JSON files for Locations.
        /// </summary>
        public string ImportLocationsFrom { get; set; }
    }
}
