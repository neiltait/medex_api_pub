using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Locations Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/locations")]
    [ApiController]
    public class LocationsController : BaseController
    {
        /// <summary>
        /// Location Retrieval by Id Query.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> _locationRetrievalByIdQueryHandler;

        /// <summary>
        /// Location Retrieval by Query.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> _locationRetrievalByQueryHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsController"/> class.
        /// </summary>
        /// <param name="locationRetrievalByIdQueryHandler">Location Retrieval By Id Query Handler.</param>
        /// <param name="locationRetrievalByQueryHandler">Location Retrieval By Query Handler.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        public LocationsController(
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> locationRetrievalByIdQueryHandler,
            IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> locationRetrievalByQueryHandler,
            IMELogger logger,
            IMapper mapper)
            : base(logger, mapper)
        {
            _locationRetrievalByIdQueryHandler = locationRetrievalByIdQueryHandler;
            _locationRetrievalByQueryHandler = locationRetrievalByQueryHandler;
        }

        /// <summary>
        ///     Get all Locations as a list of <see cref="LocationItem" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A list of locations.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetLocationsResponse>> GetLocations([FromBody] GetLocationsRequest request)
        {
            var locations =
                await _locationRetrievalByQueryHandler.Handle(
                    new LocationsRetrievalByQuery(request.Name, request.ParentId));

            // TODO: Filter on whether you have access or not
            if (request.AccessOnly)
            {
                // get current user
                // does user have access to this location directly or by having access
                // to its parent
                // if so include it in the list;
                // otherwise ignore it.
            }

            return Ok(new GetLocationsResponse
            {
                Locations = locations.Select(e => Mapper.Map<LocationItem>(e)).ToList(),
            });
        }

        /// <summary>
        ///     Get Location by ID.
        /// </summary>
        /// <param name="locationId">The Location Id.</param>
        /// <returns>A GetLocationsResponse.</returns>
        [HttpGet("{locationId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetLocationResponse>> GetLocation(string locationId)
        {
            try
            {
                var location =
                    await _locationRetrievalByIdQueryHandler.Handle(new LocationRetrievalByIdQuery(locationId));

                var response = Mapper.Map<GetLocationResponse>(location);
                return Ok(response);
            }
            catch (DocumentClientException)
            {
                return NotFound(new GetLocationResponse());
            }
        }
    }
}
