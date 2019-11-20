using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.API.Models.v1.Report;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// A controller that returns reporting data
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/report")]
    [ApiController]
    public class ReportController : AuthorizedBaseController
    {
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private readonly IAsyncQueryHandler<FinanceQuery, IEnumerable<Examination>> _financeQuery;
        private readonly IAsyncQueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Location>> _locationsRetrievalService;
        private readonly IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> _usersRetrievalService;

        /// <summary>
        /// Location Parents Service.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>> _locationsParentsService;
        
        /// <summary>
        /// Location Retrieval by Query.
        /// </summary>
        private readonly IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> _locationRetrievalByQueryHandler;

        /// <summary>
        /// The report controller constructor
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">Users retrieval by okta id service.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="permissionService">Permission service.</param>
        /// <param name="examinationRetrievalService">Examination Retrieval service.</param>
        /// <param name="financeQuery">Finance service.</param>
        /// <param name="locationsRetrievalService">Locations retrieval service.</param>
        /// <param name="usersRetrievalService">Users retrieval service</param>
        public ReportController(
            IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<FinanceQuery, IEnumerable<Examination>> financeQuery,
            IAsyncQueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Location>> locationsRetrievalService,
            IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>> usersRetrievalService,
            IAsyncQueryHandler<LocationsParentsQuery, IDictionary<string, IEnumerable<Location>>> locationsParentsService,
            IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>> locationRetrievalByQueryHandler)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _financeQuery = financeQuery;
            _locationsRetrievalService = locationsRetrievalService;
            _usersRetrievalService = usersRetrievalService;
            _locationsParentsService = locationsParentsService;
            _locationRetrievalByQueryHandler = locationRetrievalByQueryHandler;
        }

        /// <summary>
        ///     Get the dto object for making the coroner referral report <see cref="GetCoronerReferralDownloadResponse" />.
        /// </summary>
        /// <param name="examinationId">The examination id to return reporting data for.</param>
        /// <returns>A coroner referral download response</returns>
        [HttpGet]
        [Route("{examinationId}/coronal_referral_download")]
        public async Task<ActionResult<GetCoronerReferralDownloadResponse>> GetCoronerReferralDownload(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var currentUser = await CurrentUser();
            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, currentUser));

            if (examination == null)
            {
                return new NotFoundResult();
            }

            if (!CanAsync(Permission.GetCoronerReferralDownload, examination))
            {
                return Forbid();
            }

            if (!examination.ScrutinyConfirmed)
            {
                return new BadRequestObjectResult("Scrutiny should be confirmed before downloading.");
            }

            return new OkObjectResult(Mapper.Map<GetCoronerReferralDownloadResponse>(examination));
        }

        /// <summary>
        ///     Get the dto object for making the coroner referral report <see cref="GetCoronerReferralDownloadResponse" />.
        /// </summary>
        /// <param name="examinationId">The examination id to return reporting data for.</param>
        /// <returns>A coroner referral download response</returns>
        [HttpGet]
        [Route("finance_download")]
        public async Task<ActionResult<GetFinanceDownloadResponse>> GetFinanceDownload([FromQuery]GetFinanceDownloadRequest request)
        {
            var currentUser = await CurrentUser();

            if (!CanAsync(Permission.GetFinanceDownload))
            {
                return Forbid();
            }

            var permissedLocations = (await LocationsWithPermission(Permission.GetFinanceDownload)).ToList();

            var results = await _financeQuery.Handle(new FinanceQuery(request.ExaminationsCreatedFrom, request.ExaminationsCreatedTo, request.LocationId, permissedLocations));

            var sites = results.Select(x => x.SiteLocationId).ToList();
            var trusts = results.Select(x => x.TrustLocationId).ToList();
            var regions = results.Select(x => x.RegionLocationId).ToList();
            var nationals = results.Select(x => x.NationalLocationId).ToList();

            sites.AddRange(trusts);
            sites.AddRange(regions);
            sites.AddRange(nationals);

            var distinctLocations = sites.Distinct();

            var distinctLocationNames = await _locationsRetrievalService.Handle(new LocationsRetrievalByIdQuery(true, distinctLocations.ToArray()));

            var response = new GetFinanceDownloadResponse();

            response.Data = new List<ExaminationFinanceItem>();

            var users = (await _usersRetrievalService.Handle(new UsersRetrievalQuery(false, null))).ToList();

            foreach (var examination in results)
            {
                response.Data.Add(Mapper.Map<ExaminationFinanceItem>(new ExaminationLocationItem()
                {
                    Examination = examination,
                    Locations = distinctLocationNames,
                    Users = users,
                }));
            }

            return new OkObjectResult(response);
        }

        /// <summary>
        /// Get the locations for the finance download.
        /// </summary>
        /// <remarks>Needs to work on the <see cref="Permission.GetFinanceDownload"/> permission so that's why it lives here.</remarks>
        /// <returns>A list of locations.</returns>
        [HttpGet("finance_download_locations")]
        public async Task<ActionResult<GetLocationsResponse>> GetFinanceDownloadLocations()
        {
            var permissedLocationIds = await LocationsWithPermission(Permission.GetFinanceDownload);

            // All sub locations under these permissed locations
            var subLocations = (await _locationRetrievalByQueryHandler.Handle(
                new LocationsRetrievalByQuery(null, null, false, false, permissedLocationIds))).ToList();

            var onlyMeOffices = subLocations.Where(l => l.IsMeOffice).ToList();
            var allLocations = (await _locationsParentsService.Handle(new LocationsParentsQuery(onlyMeOffices.Select(x => x.LocationId)))).ToList();

            var flattenedLocations = allLocations.SelectMany(x => x.Value).ToList();
            var distinctLocationIds = flattenedLocations.Select(x => x.LocationId).Distinct();
            var distinctLocations = distinctLocationIds.Select(id => flattenedLocations.First(x => x.LocationId == id));
            var orderedDistinctLocations = distinctLocations.OrderBy(x => x.NationalLocationId)
                .ThenBy(x => x.RegionLocationId)
                .ThenBy(x => x.TrustLocationId)
                .ThenBy(x => x.SiteLocationId).ToList();

            return Ok(new GetLocationsResponse
            {
                Locations = orderedDistinctLocations.Select(e => Mapper.Map<LocationItem>(e)).ToList(),
            });
        }
    }
}
