using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalExaminer.Models;

namespace MedicalExaminer.Common
{
    public interface ILocationPersistence
    {
        Task<Location> GetLocationAsync(string locationId);
        Task<IEnumerable<Location>> GetLocationsAsync();
    }
}
