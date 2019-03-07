using System;
using System.Collections.Generic;
using System.Text;
using MedicalExaminer.Common.ConnectionSettings;

namespace MedicalExaminer.Common
{
    public class LocationConnectionSetting : ILocationConnectionSettings
    {
        public LocationConnectionSetting(Uri endPointUri, string primaryKey, string databaseId)
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
