using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
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
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public CaseOutcomeController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPut]
        [Route("confirmation_of_scrutiny")]
        public async Task<ActionResult<PutConfirmationOfScrutinyResponse>> PutConfirmationOfScrutiny(string examinationId)
        {
            var user = await CurrentUser();
            //var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));
            var confirmationOfScrutinyQuery = new ConfirmationOfScrutinyQuery(examinationId, user);
            
            return Ok(new PutConfirmationOfScrutinyResponse()
            {
                ScrutinyConfirmedOn = DateTime.Now
            });
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
