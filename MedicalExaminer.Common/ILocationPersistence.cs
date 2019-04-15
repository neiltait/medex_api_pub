using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common
{
    /// <summary>
    ///     Interface for classes that manage persistence of location objects
    /// </summary>
    public interface ILocationPersistence
    {
        /// <summary>
        ///     Get location object that has the given Id
        /// </summary>
        /// <param name="locationId">The Id of location to be returned</param>
        /// <returns>A single location</returns>
        Task<Location> GetLocationAsync(string locationId);

        /// <summary>
        ///     Get all locations
        /// </summary>
        /// <returns>A collection of locations</returns>
        Task<IEnumerable<Location>> GetLocationsAsync();

        /// <summary>
        ///     Get all locations whose name value contains locationName
        /// </summary>
        /// <param name="locationName">The string to be used to select locations whose names contain this value </param>
        /// <returns>A collection of locations whose names contain locationName</returns>
        Task<IEnumerable<Location>> GetLocationsByNameAsync(string locationName);

        /// <summary>
        ///     Get all locations who are under the location that has its own locationId that matches the parentId passed as
        ///     paramter
        /// </summary>
        /// <param name="parentId">The id that identifies the parent location</param>
        /// <returns>A collection of locations who are under the location that has a locationId that matches the parentId</returns>
        Task<IEnumerable<Location>> GetLocationsByParentIdAsync(string parentId);
    }
}