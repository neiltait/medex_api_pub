using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Persistence
{
    /// <summary>
    /// Interface for persistence class used by location seeder
    /// </summary>
    public interface ILocationsSeederPersistence
    {
        /// <summary>
        /// Writes location object to database
        /// </summary>
        /// <param name="location">location object to be written to database</param>
        /// <returns>bool</returns>
        Task<bool> SaveLocationAsync(Location location);
    }
}
