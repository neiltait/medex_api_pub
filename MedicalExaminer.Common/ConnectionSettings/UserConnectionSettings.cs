using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// User Connection Settings
    /// </summary>
    public class UserConnectionSettings : IUserConnectionSettings
    {
        public UserConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "User";
        }

        public Uri EndPointUri { get; }

        public string PrimaryKey { get; }

        public string DatabaseId { get; }

        public string Collection { get; }
    }
}
