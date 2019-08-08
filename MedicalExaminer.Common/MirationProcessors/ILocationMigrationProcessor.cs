using MedicalExaminer.Common.Queries;

namespace MedicalExaminer.Common.MirationProcessors
{
    public interface ILocationMigrationProcessor
    {
        Models.Location Handle(Models.Location location, IMigrationQuery param);
    }
}
