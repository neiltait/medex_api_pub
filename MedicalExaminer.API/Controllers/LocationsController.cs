using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Extensions.Models;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
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
        /// Location Parents Service.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> _locationParentsService;

        /// <summary>
        /// Examinations retrieval service.
        /// </summary>
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> _examinationsRetrievalService;

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
        public LocationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<LocationRetrievalByIdQuery, Location> locationRetrievalByIdQueryHandler,
            IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> locationRetrievalByQueryHandler,
            IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> locationParentsService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _locationRetrievalByIdQueryHandler = locationRetrievalByIdQueryHandler;
            _locationRetrievalByQueryHandler = locationRetrievalByQueryHandler;
            _locationParentsService = locationParentsService;
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

            if (request.AccessOnly)
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
        [Route("is_me_office")]
        public async Task<ActionResult> PutIsMEOffice(string locationId, bool isMEOffice)
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

            // When clearing; if any cases have this location assigned return bad request.
            if(isMEOffice == false)
            {
                var examinations = query();

                if( examinations.Count > 0 )
                {
                    return new BadRequestObjectResult("The location is currently being used.");
                }
            }

            location.IsMeOffice = isMEOffice;

            if (examination.CalculateOutstandingCaseOutcomesCompleted())
            {
                await _closeCaseService.Handle(new CloseCaseQuery(examinationId, user));
                return Ok();
            }


            return BadRequest();
        }


    }
}
