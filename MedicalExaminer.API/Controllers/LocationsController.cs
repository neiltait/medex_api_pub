using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Extensions.Models;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.MELogger;
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
        /// Location Parents Service.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> _locationParentsService;

        /// <summary>
        /// Examination Retrieval Service.
        /// </summary>
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> _examinationsRetrievalService;

        /// <summary>
        /// Update Location Is Me Office Service.
        /// </summary>
        private readonly IAsyncQueryHandler<UpdateLocationIsMeOfficeQuery, Location> _updateLocationIsMeOfficeService;

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
        /// <param name="locationParentsService">Location Parents service.</param>
        /// <param name="examinationsRetrievalService">Examinations retrieval service.</param>
        /// <param name="updateLocationIsMeOfficeService">Update location is me office service.</param>
        public LocationsController(
            IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> locationRetrievalByIdQueryHandler,
            IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> locationRetrievalByQueryHandler,
            IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> locationParentsService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService,
            IAsyncQueryHandler<UpdateLocationIsMeOfficeQuery, Location> updateLocationIsMeOfficeService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _locationRetrievalByIdQueryHandler = locationRetrievalByIdQueryHandler;
            _locationRetrievalByQueryHandler = locationRetrievalByQueryHandler;
            _locationParentsService = locationParentsService;
            _examinationsRetrievalService = examinationsRetrievalService;
            _updateLocationIsMeOfficeService = updateLocationIsMeOfficeService;
            _examinationsRetrievalService = examinationsRetrievalService;
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

        /// <summary>
        /// Set whether location is ME office.
        /// </summary>
        /// <param name="locationId">Location to change.</param>
        /// <param name="isMEOffice">Set or unset is me office.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{locationId}/is_me_office")]
        public async Task<ActionResult> PutIsMeOffice(string locationId, [FromBody] bool isMeOffice)
        {
            if (string.IsNullOrEmpty(locationId))
            {
                return new BadRequestObjectResult(nameof(locationId));
            }

            if (!Guid.TryParse(locationId, out _))
            {
                return new BadRequestObjectResult(nameof(locationId));
            }

            var user = await CurrentUser();

            var location =
                await _locationRetrievalByIdQueryHandler.Handle(new LocationRetrievalByIdQuery(locationId));

            if (location == null)
            {
                return new NotFoundResult();
            }

            var locationDocument = (await
                    _locationParentsService.Handle(
                        new LocationParentsQuery(location.LocationId)))
                .ToLocationPath();

            if (!CanAsync(Permission.UpdateLocation, locationDocument))
            {
                return Forbid();
            }

            // When clearing; if ANY cases have this location assigned return bad request. These could even be cases the current user might not have permission to.
            if (isMeOffice == false)
            {
                var permissedLocations = new[] { locationId };

                var examinations = await _examinationsRetrievalService.Handle(new ExaminationsRetrievalQuery(
                    permissedLocations,
                    null,
                    locationId,
                    null,
                    1,
                    1,
                    null,
                    MedicalExaminer.Models.Enums.OpenClosedCases.Open));

                if (examinations.Any())
                {
                    return new BadRequestObjectResult("The location is currently in use.");
                }
            }

            await _updateLocationIsMeOfficeService.Handle(new UpdateLocationIsMeOfficeQuery()
            {
                LocationId = locationId,
                IsMeOffice = isMeOffice,
            });

            return Ok();
        }
    }
}
