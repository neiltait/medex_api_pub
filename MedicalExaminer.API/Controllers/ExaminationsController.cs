using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Extensions.Models;
using MedicalExaminer.Common.Loggers;
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
    /// <summary>
    ///     Examinations Controller.
    /// </summary>
    /// <inheritdoc />
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations")]
    [ApiController]
    [Authorize]
    public class ExaminationsController : AuthorizedBaseController
    {
        /// <summary>
        /// The location filter lookup key
        /// </summary>
        public const string LocationFilterLookupKey = "LocationFilterLookup";

        /// <summary>
        /// The user filter lookup key
        /// </summary>
        public const string UserFilterLookupKey = "UserFilterLookup";

        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview> _examinationsDashboardService;
        private readonly IAsyncQueryHandler<CreateExaminationQuery, Examination> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> _examinationsRetrievalService;
        private readonly IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> _locationParentsService;

        private readonly IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> _locationRetrievalByQueryHandler;

        private readonly IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>> _locationsRetrievalByQueryHandler;

        private readonly IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> _usersRetrievalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExaminationsController"/> class.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        /// <param name="examinationCreationService">examinationCreationService.</param>
        /// <param name="examinationsRetrievalService">examinationsRetrievalService.</param>
        /// <param name="examinationsDashboardService">Examination Dashboard Service.</param>
        /// <param name="locationParentsService">Location Parents Service.</param>
        /// <param name="locationRetrievalByQueryHandler">Location Retrieval by Query Handler.</param>
        /// <param name="usersRetrievalService">User retrieval service.</param>
        public ExaminationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<CreateExaminationQuery, Examination> examinationCreationService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview> examinationsDashboardService,
            IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> locationParentsService,
            IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> locationRetrievalByQueryHandler,
            IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> usersRetrievalService,
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>> locationsRetrievalByQueryHandler)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _examinationCreationService = examinationCreationService;
            _examinationsRetrievalService = examinationsRetrievalService;
            _examinationsDashboardService = examinationsDashboardService;
            _locationParentsService = locationParentsService;
            _locationRetrievalByQueryHandler = locationRetrievalByQueryHandler;
            _usersRetrievalService = usersRetrievalService;
            _locationsRetrievalByQueryHandler = locationsRetrievalByQueryHandler;
        }

        /// <summary>
        /// Get All Examinations as a list of <see cref="ExaminationItem"/>.
        /// </summary>
        /// <param name="filter">Filter.</param>
        /// <returns>A list of examinations.</returns>
        [HttpGet]
        public async Task<ActionResult<GetExaminationsResponse>> GetExaminations([FromQuery]GetExaminationsRequest filter)
        {
            if (filter == null)
            {
                return BadRequest(new GetExaminationsResponse());
            }

            var permissedLocations = (await LocationsWithPermission(Permission.GetExaminations)).ToList();

            var examinationsQuery = new ExaminationsRetrievalQuery(
                permissedLocations,
                filter.CaseStatus,
                filter.LocationId,
                filter.OrderBy,
                filter.PageNumber,
                filter.PageSize,
                filter.UserId,
                filter.OpenCases);

            var examinations = await _examinationsRetrievalService.Handle(examinationsQuery);

            var dashboardOverview = await _examinationsDashboardService.Handle(examinationsQuery);

            var response = new GetExaminationsResponse
            {
                CountOfTotalCases = dashboardOverview.TotalCases,
                CountOfUrgentCases = dashboardOverview.CountOfUrgentCases,
                CountOfCasesHaveUnknownBasicDetails = dashboardOverview.CountOfHaveUnknownBasicDetails,
                CountOfCasesUnassigned = dashboardOverview.CountOfUnassigned,
                CountOfCasesReadyForMEScrutiny = dashboardOverview.CountOfReadyForMEScrutiny,
                CountOfCasesHaveBeenScrutinisedByME = dashboardOverview.CountOfHaveBeenScrutinisedByME,
                CountOfCasesPendingAdditionalDetails = dashboardOverview.CountOfPendingAdditionalDetails,
                CountOfCasesPendingDiscussionWithQAP = dashboardOverview.CountOfPendingDiscussionWithQAP,
                CountOfCasesPendingDiscussionWithRepresentative = dashboardOverview.CountOfPendingDiscussionWithRepresentative,
                CountOfCasesHaveFinalCaseOutstandingOutcomes = dashboardOverview.CountOfHaveFinalCaseOutstandingOutcomes,
                Examinations = examinations.Select(e => Mapper.Map<PatientCardItem>(e)).ToList(),
                CountOfFilteredCases = dashboardOverview.CountOfFilteredCases,
            };

            var locations = (await _locationRetrievalByQueryHandler.Handle(
                    new LocationsRetrievalByQuery(null, null, false, false, permissedLocations))).ToList();

            var onlyMeOffices = locations.Where(l => l.IsMeOffice).ToList();
            var allLocations = (await _locationsRetrievalByQueryHandler.Handle(new LocationsParentsQuery(onlyMeOffices.Select(x => x.LocationId)))).ToList();

            var flatternedLocations = allLocations.SelectMany(x => x.Value);
            var distinctLocationIds = flatternedLocations.Select(x => x.LocationId).Distinct();
            var distinctLocations = distinctLocationIds.Select(id => flatternedLocations.First(x => x.LocationId == id));
            var orderedDistinctLocations = distinctLocations.OrderBy(x => x.NationalLocationId)
                                .ThenBy(x => x.RegionLocationId)
                                .ThenBy(x => x.TrustLocationId)
                                .ThenBy(x => x.SiteLocationId).ToList();
            response.AddLookup(LocationFilterLookupKey, Mapper.Map<IEnumerable<Location>, IEnumerable<LocationLookup>>(orderedDistinctLocations));

            response.AddLookup(UserFilterLookupKey, await GetUserLookupForLocations(locations));

            return Ok(response);
        }

        /// <summary>
        ///     Create a new case.
        /// </summary>
        /// <param name="postExaminationRequest">The PostExaminationRequest.</param>
        /// <returns>A PostExaminationResponse.</returns>
        [HttpPost]
        public async Task<ActionResult<PutExaminationResponse>> CreateExamination(
            [FromBody] PostExaminationRequest postExaminationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestEnums(new PutExaminationResponse());
            }

            try
            {
                var examination = Mapper.Map<Examination>(postExaminationRequest);

                var locations = await
                    _locationParentsService.Handle(
                        new LocationParentsQuery(examination.MedicalExaminerOfficeResponsible));

                examination.UpdateLocationPath(locations);

                if (!CanAsync(Permission.CreateExamination, examination))
                {
                    return Forbid();
                }

                var myUser = await CurrentUser();

                examination.CreatedBy = myUser.UserId;
                examination.CreatedAt = DateTime.Now;

                var result = await _examinationCreationService.Handle(new CreateExaminationQuery(examination, myUser));
                var res = new PutExaminationResponse
                {
                    ExaminationId = result.ExaminationId
                };

                return Ok(res);
            }
            catch (DocumentClientException)
            {
                return NotFound(new PostExaminationRequest());
            }
            catch (ArgumentException)
            {
                return NotFound(new PostExaminationRequest());
            }
        }

        /// <summary>
        /// Gets the user lookup for locations.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <returns>User Lookup</returns>
        private async Task<IEnumerable<object>> GetUserLookupForLocations(IEnumerable<Location> locations)
        {
            var locationIds = locations.Select(l => l.LocationId).ToList();

            var users = (await GetUsersForLocations(locationIds)).ToList();

            return Mapper.Map<IEnumerable<MeUser>, IEnumerable<UserLookup>>(users);
        }

        /// <summary>
        /// Gets the users for locations.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <returns>List of users.</returns>
        private async Task<IEnumerable<MeUser>> GetUsersForLocations(IEnumerable<string> locations)
        {
            var users = await _usersRetrievalService.Handle(new UsersRetrievalQuery(true, null));

            var usersForLocations = users.Where(u => u.Permissions != null && u.Permissions.Any(p => locations.Contains(p.LocationId)));

            return usersForLocations;
        }
    }
}