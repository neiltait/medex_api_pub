using AutoMapper;
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
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService) 
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _examinationRetrievalService = examinationRetrievalService;
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
    }
}
