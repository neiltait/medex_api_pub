using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.Report;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            IAsyncQueryHandler<FinanceQuery, IEnumerable<Examination>> financeQuery) 
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _financeQuery = financeQuery;
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
            var results = await _financeQuery.Handle(new FinanceQuery(request.ExaminationsCreatedFrom, request.ExaminationsCreatedTo, request.LocationId));

            //var rowOne = new ExaminationFinanceItem()
            //{
            //    CaseClosed = new System.DateTime(2019, 10, 10),
            //    CaseCreated = new System.DateTime(2019, 9, 10),
            //    WaiverFee = true,
            //    HasNhsNumber = true,
            //    ExaminationId = "whocares",
            //    MedicalExaminerId =  "medicalEaminer1",
            //    NationalName = "National",
            //    RegionName = "Region",
            //    TrustName = "Trust",
            //    SiteName = "Site",
            //    ModeOfDisposal = MedicalExaminer.Models.Enums.ModeOfDisposal.Cremation
            //};

            //var rowTwo = new ExaminationFinanceItem()
            //{
            //    CaseClosed = null,
            //    CaseCreated = new System.DateTime(2019, 9, 10),
            //    WaiverFee = false,
            //    HasNhsNumber = false,
            //    ExaminationId = "whocares",
            //    MedicalExaminerId = "medicalEaminer2",
            //    NationalName = "National",
            //    RegionName = "Region",
            //    TrustName = "Trust",
            //    SiteName = "Site",
            //    ModeOfDisposal = MedicalExaminer.Models.Enums.ModeOfDisposal.Cremation
            //};

            //var rowThree = new ExaminationFinanceItem()
            //{
            //    CaseClosed = null,
            //    CaseCreated = new System.DateTime(2019, 9, 10),
            //    WaiverFee = null,
            //    HasNhsNumber = false,
            //    ExaminationId = "whocares",
            //    MedicalExaminerId = "medicalEaminer2",
            //    NationalName = "National",
            //    RegionName = "Region",
            //    TrustName = "Trust",
            //    SiteName = "Site",
            //    ModeOfDisposal = MedicalExaminer.Models.Enums.ModeOfDisposal.BuriedAtSea
            //};

            //var response = new GetFinanceDownloadResponse();
            //response.Data = new System.Collections.Generic.List<ExaminationFinanceItem>();
            //response.Data.Add(rowOne);
            //response.Data.Add(rowTwo);
            //response.Data.Add(rowThree);

            var response = new GetFinanceDownloadResponse();
            response.Data = new List<ExaminationFinanceItem>();

            foreach (var examination in results)
            {
                response.Data.Add(Mapper.Map<ExaminationFinanceItem>(examination));
            }

            return new OkObjectResult(response);
        }
    }
}
