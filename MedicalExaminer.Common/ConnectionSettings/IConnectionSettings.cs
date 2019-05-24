using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    ///     Connection Settings Interface
    /// </summary>
    public interface IConnectionSettings
    {
        Uri EndPointUri { get; }

        string PrimaryKey { get; }

        string DatabaseId { get; }

        string Collection { get; }
    }
}