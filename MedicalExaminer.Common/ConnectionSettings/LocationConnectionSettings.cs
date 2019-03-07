using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.ConnectionSettings
{
    public class LocationConnectionSettings : ILocationConnectionSettings
    {
        public LocationConnectionSettings(Uri endPointUri, string primaryKey, string databaseId, string collection)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = collection;
        }

        public Uri EndPointUri { get; }
        public string PrimaryKey { get; }
        public string DatabaseId { get; }
        public string Collection { get; }
    }
}
