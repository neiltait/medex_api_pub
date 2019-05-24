namespace MedicalExaminer.Common.ConnectionSettings
{
    public static class ConnectionSettingsExtensionMethods
    {
        public static IConnectionSettings ToAuditSettings(this IConnectionSettings connectionSetting)
        {
            return new AuditConnectionSetting(connectionSetting.EndPointUri,
                connectionSetting.PrimaryKey,
                connectionSetting.DatabaseId,
                connectionSetting.Collection);
        }
    }
}
