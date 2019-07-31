using System.Collections.Generic;

namespace MedicalExaminer.API.Models.v1.Locations
{
    /// <summary>
    ///     Request object for a list of locations.
    /// </summary>
    /// <inheritdoc />
    public class GetLocationsRequest : RequestBase
    {
        /// <summary>
        /// Filter by Access Only.
        /// </summary>
        public bool AccessOnly { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parent Id.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// retrieve only those locations that are defined as Medical Examiner Offices
        /// </summary>
        public bool OnlyMEOffices { get; set; } = false;
    }
}
