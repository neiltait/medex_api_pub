using AutoMapper;
using MedicalExaminer.Common.Queries;

namespace MedicalExaminer.Common.MirationProcessors.Location
{
    public class LocationMigrationProcessorV1 : ILocationMigrationProcessor
    {
        public Models.Location Handle(Models.Location location, IMigrationQuery param)
        {
            var migratedLocation = Mapper.Map<Models.Location>(location);
            migratedLocation.Version = param.VersionNumber;
            return migratedLocation;
        }
    }
}
