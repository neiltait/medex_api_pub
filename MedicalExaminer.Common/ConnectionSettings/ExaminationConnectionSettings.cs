using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    public class ExaminationConnectionSettings : IExaminationConnectionSettings
    {
        public ExaminationConnectionSettings(Uri endPointUri, string primaryKey, string databaseId)
        {
            if(endPointUri == null)
            {
                throw new ArgumentNullException(nameof(endPointUri));
            }

            if (string.IsNullOrEmpty(primaryKey))
            {
                throw new ArgumentNullException(nameof(primaryKey));
            }

            if (string.IsNullOrEmpty(databaseId))
            {
                throw new ArgumentNullException(nameof(databaseId));
            }

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