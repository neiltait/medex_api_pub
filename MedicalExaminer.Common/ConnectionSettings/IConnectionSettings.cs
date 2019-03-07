using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    public interface IConnectionSettings
    {
        Uri EndPointUri { get; }
        string PrimaryKey { get; }
        string DatabaseId { get; }
        string Collection { get; }
    }
}
