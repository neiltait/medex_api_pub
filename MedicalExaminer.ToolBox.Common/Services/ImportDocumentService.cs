using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using MedicalExaminer.ToolBox.Common.Dtos.Generate;
using Newtonsoft.Json;

namespace MedicalExaminer.ToolBox.Common.Services
{
    public class ImportDocumentService
    {
        private readonly ICosmosStore<Location> _locationStore;
        private readonly ICosmosStore<MeUser> _userStore;
        private readonly ICosmosStore<Examination> _examinationStore;

        public ImportDocumentService(
            ICosmosStore<Location> locationStore,
            ICosmosStore<MeUser> userStore,
            ICosmosStore<Examination> examinationStore)
        {
            _locationStore = locationStore;
            _userStore = userStore;
            _examinationStore = examinationStore;
        }

        public async Task<Location> ImportLocation(string locationJson)
        {
            var location = JsonConvert.DeserializeObject<Location>(locationJson);

            var result = await _locationStore.UpsertAsync(location);
            
            return result.Entity;
        }

        public async Task<MeUser> ImportUser(string userJson)
        {
            var user = JsonConvert.DeserializeObject<MeUser>(userJson);

            var result = await _userStore.UpsertAsync(user);

            return result.Entity;
        }
    }
}
