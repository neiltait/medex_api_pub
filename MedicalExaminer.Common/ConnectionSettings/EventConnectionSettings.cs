using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    public class EventConnectionSettings : IExaminationConnectionSettings
    {
        public EventConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "Events";
        }

        public Uri EndPointUri { get; }

        public string PrimaryKey { get; }

        public string DatabaseId { get; }

        public string Collection { get; }
    }
}