using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Location Retrieval By Query.
    /// </summary>
    public class LocationsRetrievalByQuery : IQuery<IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LocationsRetrievalByQuery"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="parentId">Parent Id.</param>
        /// <param name="permissedLocations">Permissed locations.</param>
        public LocationsRetrievalByQuery(string name, string parentId, IEnumerable<string> permissedLocations = null)
        {
            Name = name;
            ParentId = parentId;
            PermissedLocations = permissedLocations;
        }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent Id.
        /// </summary>
        public string ParentId { get; }

        /// <summary>
        /// Permissed Locations.
        /// </summary>
        public IEnumerable<string> PermissedLocations { get; }
    }
}