using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Seeders;

namespace Medical_Examiner_API.Persistence
{
    interface ILocationsSeederPersistence
    {

        Task<bool> SaveLocationAsync(Location location);
    }
}
