using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    public class CaseOutcomeController : AuthenticatedBaseController
    {
        IAsyncQueryHandler<CoronerReferralQuery, string> _coronerReferralService;

        public CaseOutcomeController(
            IMELogger logger,
            IMapper mapper,
            IAuthorizationService authorizationService,
            IAsyncQueryHandler<CoronerReferralQuery, string> coronerReferralService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _coronerReferralService = coronerReferralService;
        }


        [HttpPut]
        [Route("confirmation_of_scrutiny")]
        public ActionResult<PutConfirmationOfScrutinyResponse> PutConfirmationOfScrutiny()
        {
            // TODO:  Implement
            return Ok(new PutConfirmationOfScrutinyResponse()
            {
                ScrutinyConfirmedOn = DateTime.Now
            });
        }


        [HttpPut]
        [Route("coroner_referral")]
        public async Task<ActionResult> PutCoronerReferral(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var user = await CurrentUser();

            var result = await _coronerReferralService.Handle(new CoronerReferralQuery(examinationId, user));

            return Ok();
        }

        [HttpPut]
        [Route("outstanding_case_items")]
        public ActionResult PutOutstandingCaseItems([FromBody] PutOutstandingCaseItemsRequest request)
        {
            // TODO:  Implement
            return Ok();
        }

        [HttpPut]
        [Route("close_case")]
        public ActionResult PutCloseCase()
        {
            // TODO:  Implement
            return Ok();
        }

        [HttpGet]
        [Route("case_outcome")]
        public ActionResult<GetCaseOutcomeResponse> GetCaseOutcome()
        {
            // TODO:  Implement
            return Ok(new GetCaseOutcomeResponse());
        }
    }
}
