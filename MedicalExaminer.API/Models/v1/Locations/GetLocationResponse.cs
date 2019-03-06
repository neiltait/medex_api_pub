using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Models.V1.Locations
{
    /// <summary>
    /// Response object for a single location
    /// </summary>
    public class GetLocationResponse
    {
        /// <summary>
        /// Id that uniquely identifies a location in the database
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// Full name of the location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The alphanumeric code that identifies a location
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Corresponds to the LocationId of this location's parent location
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Location is in use in Medical Examiners
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Identifies location as one of site, trust, region or national
        /// </summary>
        public LocationType Type {get; set; }
    }
}
