using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// ME Logger Connection Settings
    /// </summary>
    public class MELoggerConnectionSettings : IMELoggerConnectionSettings
    {
        /// <summary>
        /// Initialise a new instance of <see cref="MELoggerConnectionSettings"/>.
        /// </summary>
        /// <param name="endPointUri">End point URI</param>
        /// <param name="primaryKey">Primary Key</param>
        /// <param name="databaseId">Database ID.</param>
        public MELoggerConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "MELogger";
        }

        /// <inheritdoc/>
        public Uri EndPointUri { get; }

        /// <inheritdoc/>
        public string PrimaryKey { get; }

        /// <inheritdoc/>
        public string DatabaseId { get; }

        /// <inheritdoc/>
        public string Collection { get; }
    }
}
