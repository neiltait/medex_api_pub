namespace MedicalExaminer.API.Models.v1.Locations
{
    /// <summary>
    /// Location Lookup
    /// </summary>
    /// <remarks>Location fields when displayed in lookup lists.</remarks>
    public class LocationLookup
    {
        /// <summary>
        /// The Location identifier.
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string Name { get; set; }
    }
}
