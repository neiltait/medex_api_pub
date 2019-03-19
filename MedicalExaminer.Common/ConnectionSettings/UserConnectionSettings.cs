using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.ConnectionSettings
{
    public class UserConnectionSettings : IUserConnectionSettings
    {
        public UserConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "Users";
        }

        public Uri EndPointUri { get; }
        public string PrimaryKey { get; }
        public string DatabaseId { get; }
        public string Collection { get; }
    }
}
