using System.Collections.Generic;

namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Location Retrieval By Query.
    /// </summary>
    public class LocationRetrievalByQuery : IQuery<IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LocationRetrievalByQuery"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="parentId">Parent Id.</param>
        public LocationRetrievalByQuery(string name, string parentId)
        {
            Name = name;
            ParentId = parentId;
        }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent Id.
        /// </summary>
        public string ParentId { get; }
    }
}