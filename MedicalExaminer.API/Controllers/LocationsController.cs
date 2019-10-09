using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Locations Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/locations")]
    [ApiController]
    public class LocationsController : AuthorizedBaseController
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
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        /// <param name="locationRetrievalByIdQueryHandler">Location Retrieval By Id Query Handler.</param>
        /// <param name="locationRetrievalByQueryHandler">Location Retrieval By Query Handler.</param>
        public LocationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> locationRetrievalByIdQueryHandler,
            IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> locationRetrievalByQueryHandler)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
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
        public async Task<ActionResult<GetLocationsResponse>> GetLocations([FromQuery] GetLocationsRequest request)
        {
            IEnumerable<string> permissedLocations = null;

            if (request == null)
            {
                return BadRequest(new GetLocationsResponse());
            }

            if (request.CreateExaminationOnly)
            {
                permissedLocations = await LocationsWithPermission(Permission.CreateExamination);
            }
            else if (request.AccessOnly)
            {
                permissedLocations = await LocationsWithPermission(Permission.GetLocation);
            }

            var locations =
                await _locationRetrievalByQueryHandler.Handle(
                    new LocationsRetrievalByQuery(request.Name, request.ParentId, false, request.OnlyMEOffices, permissedLocations));

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
