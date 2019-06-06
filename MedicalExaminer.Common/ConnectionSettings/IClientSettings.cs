using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// Client Settings Interface.
    /// </summary>
    public interface IClientSettings
    {
        /// <summary>
        /// End point URI.
        /// </summary>
        Uri EndPointUri { get; }

        /// <summary>
        /// Primary key.
        /// </summary>
        string PrimaryKey { get; }
    }
}
