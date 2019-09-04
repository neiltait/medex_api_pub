using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Location Retrieval By Id Query.
    /// </summary>
    public class LocationsRetrievalByIdQuery : IQuery<IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LocationsRetrievalByQuery"/>.
        /// </summary>
        /// <param name="forLookup">Limit fields returned to ID and Name.</param>
        /// <param name="locationIds">List of locations to query.</param>
        public LocationsRetrievalByIdQuery(bool forLookup, IEnumerable<string> locationIds = null)
        {
            ForLookup = forLookup;
            LocationIds = locationIds;
        }

        /// <summary>
        /// For Lookup
        /// </summary>
        /// <remarks>Set to true to restrict the fields that get returned.</remarks>
        public bool ForLookup { get; }

        /// <summary>
        /// Location Ids
        /// </summary>
        public IEnumerable<string> LocationIds { get; }
    }
}