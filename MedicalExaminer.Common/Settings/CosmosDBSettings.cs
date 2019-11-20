namespace MedicalExaminer.Common.Settings
{
    /// <summary>
    /// Cosmos Database Settings.
    /// </summary>
    public class CosmosDbSettings
    {
        /// <summary>
        /// URL.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Primary Key.
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Database Id.
        /// </summary>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Bypass SSL certificate check
        /// </summary>
        /// <remarks>Used on the emulator which has a self signed cert.</remarks>
        public bool BypassSsl { get; set; }
    }
}
