using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// User Session Connection Settings
    /// </summary>
    public class UserSessionConnectionSettings : IUserSessionConnectionSettings
    {
        /// <summary>
        /// Initialise a new instance of <see cref="UserSessionConnectionSettings"/>.
        /// </summary>
        /// <param name="endPointUri">End point URI</param>
        /// <param name="primaryKey">Primary Key</param>
        /// <param name="databaseId">Database ID.</param>
        public UserSessionConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "UserSessions";
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
