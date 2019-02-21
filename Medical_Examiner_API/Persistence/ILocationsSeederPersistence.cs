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
        /// Write list of locations to database
        /// </summary>
        /// <param name="locations">IList of location objects</param>
        /// <returns>bool</returns>
        Task<bool> SaveAllLocationsAsync(IList<Location> locations);
    }
}
