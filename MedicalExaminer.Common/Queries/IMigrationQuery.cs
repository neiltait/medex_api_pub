namespace MedicalExaminer.Common.Queries
{
    public interface IMigrationQuery : IQuery<bool>
    {
        int VersionNumber { get; }
    }
}
