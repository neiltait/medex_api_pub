using System;

namespace MedicalExaminer.Common.ConnectionSettings
{
    public class AuditConnectionSetting : IConnectionSettings
    {
        public AuditConnectionSetting(Uri endPointUri, string primaryKey,
            string databaseId, string collection)
        {
            EndPointUri = endPointUri;
            PrimaryKey = primaryKey;
            DatabaseId = databaseId;
            Collection = collection + Postfix;
        }

        private const string Postfix = "-Audit";

        public Uri EndPointUri { get; private set; }

        public string PrimaryKey { get; private set; }

        public string DatabaseId { get; private set; }

        public string Collection { get; private set; }
    }
}
