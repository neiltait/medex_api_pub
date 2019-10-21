using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.Report;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// <summary>
        /// The report controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="usersRetrievalByOktaIdService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="permissionService"></param>
        /// <param name="examinationRetrievalService"></param>
        public ReportController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<FinanceQuery, IEnumerable<Examination>> financeQuery,
            IAsyncQueryHandler<LocationsRetrievalByIdQuery, IEnumerable<Location>> locationsRetrievalService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _financeQuery = financeQuery;
            _locationsRetrievalService = locationsRetrievalService;
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

            var results = await _financeQuery.Handle(new FinanceQuery(request.ExaminationsCreatedFrom, request.ExaminationsCreatedTo, request.LocationId));

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

            foreach (var examination in results)
            {
                response.Data.Add(Mapper.Map<ExaminationFinanceItem>(new ExaminationLocationItem()
                {
                    Examination = examination,
                    Locations = distinctLocationNames
                }));
            }

            return new OkObjectResult(response);
        }
    }
}
