using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models
{
    /// <summary>
    /// Cosmos Database Settings.
    /// </summary>
    public class CosmosDbSettings
    {
        /// <summary>
        /// URL.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Primary Key.
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Database Id.
        /// </summary>
        public string DatabaseId { get; set; }
    }
}
