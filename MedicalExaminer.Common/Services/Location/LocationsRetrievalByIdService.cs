using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Queries;
using MedicalExaminer.Common.Queries.Location;

namespace MedicalExaminer.Common.Services.Location
{
    /// <summary>
    /// Location Retrieval By Id Service.
    /// </summary>
    /// <inheritdoc/>
    public class LocationsRetrievalByIdService : QueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Models.Location>>
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LocationsRetrievalByIdService"/>.
        /// </summary>
        /// <param name="databaseAccess">Database Access.</param>
        /// <param name="connectionSettings">Connection Settings.</param>
        public LocationsRetrievalByIdService(IDatabaseAccess databaseAccess, ILocationConnectionSettings connectionSettings)
            : base(databaseAccess, connectionSettings)
        {
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Models.Location>> Handle(LocationsRetrievalByIdQuery param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            Expression<Func<Models.Location, bool>> predicate = l =>
                param.LocationIds.Contains(l.LocationId);

            IEnumerable<Models.Location> result;

            if (param.ForLookup)
            {
                result = await GetItemsAsync<Models.Location>(predicate, location => new
                {
                    name = location.Name,
                    id = location.LocationId
                });
            }
            else
            {
                result = await GetItemsAsync(predicate);
            }

            return result;
        }

        /// <summary>
        /// Get Predicate.
        /// </summary>
        /// <param name="param">Params.</param>
        /// <returns>The Predicate.</returns>
        private Expression<Func<Models.Location, bool>> GetPredicate(LocationsRetrievalByQuery param)
        {
            Expression<Func<Models.Location, bool>> predicate;

            if (param.Name == null && param.ParentId == null)
            {
                predicate = location => true;
            }
            else if (param.Name == null)
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

            if (param.OnlyMeOffices)
            {
                predicate = predicate.And(x => x.IsMeOffice);
            }

            return predicate;
        }
    }
}