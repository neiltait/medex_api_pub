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
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/report")]
    [ApiController]
    public class ReportController : AuthorizedBaseController
    {

        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public ReportController(IMELogger logger,
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
        /// <param name="request">The request.</param>
        /// <returns>A coroner referral download response</returns>
        [HttpGet]
        [Route("{examinationId}/coronal_referral_download")]
        public async Task<ActionResult<GetCoronerReferralDownloadResponse>> GetCoronerReferralDownload(string examinationId)
        {
            var currentUser = CurrentUser().Result;
            var examination = _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, currentUser)).Result;

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
