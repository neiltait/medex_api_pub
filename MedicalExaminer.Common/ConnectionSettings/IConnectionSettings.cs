namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// Connection Settings Interface
    /// </summary>
    /// <see cref="IClientSettings"/>
    public interface IConnectionSettings : IClientSettings
    {
        /// <summary>
        /// Database name.
        /// </summary>
        string DatabaseId { get; }

        /// <summary>
        /// Collection name within database.
        /// </summary>
        string Collection { get; }
    }
}