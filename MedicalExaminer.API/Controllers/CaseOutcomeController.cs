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
    [Authorize]
    public class CaseOutcomeController : AuthenticatedBaseController
    {
        private IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination> _confirmationOfScrutinyService;

        public CaseOutcomeController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination> confirmationOfScrutinyService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _confirmationOfScrutinyService = confirmationOfScrutinyService;
        }

        /// <summary>
        /// Confirmation Of Scrutiny
        /// </summary>
        /// <param name="examinationId">Examination Identifier</param>
        /// <returns>Date time of Scrutiny Confirmed On</returns>
        [HttpPut]
        [Route("confirmation_of_scrutiny")]
        public async Task<ActionResult<PutConfirmationOfScrutinyResponse>> PutConfirmationOfScrutiny(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new PutConfirmationOfScrutinyResponse());
            }

            var user = await CurrentUser();

            var result = await _confirmationOfScrutinyService.Handle(new ConfirmationOfScrutinyQuery(examinationId, user));
            return Ok(Mapper.Map<PutConfirmationOfScrutinyResponse>(result));

        }

        [HttpPut]
        [Route("coroner_referral")]
        public ActionResult PutCoronerReferral()
        {
            // TODO:  Implement
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
        public ActionResult PutCloseCase([FromBody] PutOutstandingCaseItemsRequest request)
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
