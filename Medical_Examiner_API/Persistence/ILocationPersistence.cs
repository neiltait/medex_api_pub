using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;

namespace Medical_Examiner_API.Persistence
{
    interface ILocationPersistence
    {
        Task<Location> GetLocationAsync(string locationId);
        Task<IEnumerable<Location>> GetLocationsAsync();
    }
}
