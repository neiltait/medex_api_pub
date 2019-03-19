using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    public class ExaminationConnectionSettings : IExaminationConnectionSettings
    {
        public ExaminationConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = "Examinations";
        }

        public Uri EndPointUri { get; }
        public string PrimaryKey { get; }
        public string DatabaseId { get; }
        public string Collection { get; } 
    }
}
