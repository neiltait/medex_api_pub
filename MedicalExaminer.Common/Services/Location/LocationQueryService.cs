using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    /// <summary>
    /// Location Query Service.
    /// </summary>
    /// <inheritdoc/>
    public class LocationQueryService : QueryHandler<LocationRetrievalByQuery, IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of the Location Id Service.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public LocationQueryService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<Models.Location>> Handle(LocationRetrievalByQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var predicate = GetPredicate(param);

            return GetItemsAsync(predicate);
        }

        /// <summary>
        /// Get Predicate.
        /// </summary>
        /// <param name="param">Params.</param>
        /// <returns>The Predicate.</returns>
        private Expression<Func<Models.Location, bool>> GetPredicate(LocationRetrievalByQuery param)
        {
            Expression<Func<Models.Location, bool>> predicate;

            if (param.Name == null)
            {
                predicate = location => location.ParentId == param.ParentId;
            }
            else if (param.ParentId == null)
            {
                predicate = location => location.Name == param.Name;
            }
            else
            {
                predicate = location => location.Name == param.Name
                                        && location.ParentId == param.ParentId;
            }

            return predicate;
        }
    }
}