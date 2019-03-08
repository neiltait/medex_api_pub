using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    public class LocationConnectionSettings : ILocationConnectionSettings
    {
        public LocationConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "Location";
        }

        public Uri EndPointUri { get; }
        public string PrimaryKey { get; }
        public string DatabaseId { get; }
        public string Collection { get; }
    }
}
