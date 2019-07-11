namespace MedicalExaminer.Common.Queries.Permissions
{
    /// <summary>
    /// Invalid User Permission Query
    /// </summary>
    public class InvalidUserPermissionQuery : IQuery<bool>
    {
        public string InvalidId { get; }

        public InvalidUserPermissionQuery()
        {
            InvalidId = null;
        }
    }
}
