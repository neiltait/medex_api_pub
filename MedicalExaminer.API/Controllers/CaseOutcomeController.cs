using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    public class CaseOutcomeController : AuthenticatedBaseController
    {
        private IAsyncQueryHandler<CoronerReferralQuery, string> _coronerReferralService;
        private IAsyncQueryHandler<CloseCaseQuery, string> _closeCaseService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string> _saveOutstandingCaseItemsService;

        public CaseOutcomeController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CoronerReferralQuery, string> coronerReferralService,
            IAsyncQueryHandler<CloseCaseQuery, string> closeCaseService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string> saveOutstandingCaseItemsService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _coronerReferralService = coronerReferralService;
            _closeCaseService = closeCaseService;
            _examinationRetrievalService = examinationRetrievalService;
            _saveOutstandingCaseItemsService = saveOutstandingCaseItemsService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Save Coroner Referral
        /// </summary>
        /// <param name="examinationId">Examination ID</param>
        /// <returns>None</returns>
        [HttpPut]
        [Route("coroner_referral")]
        public async Task<ActionResult> PutCoronerReferral(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            Guid examinationGuid;
            if (!Guid.TryParse(examinationId, out examinationGuid))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var user = await CurrentUser();
            var result = await _coronerReferralService.Handle(new CoronerReferralQuery(examinationId, user));

            return Ok();
        }

        /// <summary>
        /// Save Outstanding Case Items
        /// </summary>
        /// <param name="examinationId">Examination ID</param>
        /// <param name="putOutstandingCaseItemsRequest">Request</param>
        /// <returns>None</returns>
        [HttpPut]
        [Route("outstanding_case_items")]
        public async Task<ActionResult> PutOutstandingCaseItems(
            string examinationId,
            [FromBody]
            PutOutstandingCaseItemsRequest putOutstandingCaseItemsRequest)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            Guid examinationGuid;
            if (!Guid.TryParse(examinationId, out examinationGuid))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            // get examination to update
            var user = await CurrentUser();
            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return NotFound();
            }

            // map the putoutstandingcaseitemsrequest onto the examinationtoUpdate
            var outstandingCaseItems = Mapper.Map<CaseOutcome>(putOutstandingCaseItemsRequest);

            // save the examination
            var result = await _saveOutstandingCaseItemsService.Handle(new SaveOutstandingCaseItemsQuery(examinationId, outstandingCaseItems, user));
            return Ok();
        }

        /// <summary>
        /// Closing a case
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("close_case")]
        public async Task<ActionResult> PutCloseCase(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            Guid examinationGuid;
            if (!Guid.TryParse(examinationId, out examinationGuid))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var user = await CurrentUser();
            var result = await _closeCaseService.Handle(new CloseCaseQuery(examinationId, user));

            return Ok();
        }

        /// <summary>
        /// Get Case Outcome details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetCaseOutcomeResponse>> GetCaseOutcome(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetCaseOutcomeResponse());
            }

            Guid examinationGuid;
            if (!Guid.TryParse(examinationId, out examinationGuid))
            {
                return BadRequest(new GetCaseOutcomeResponse());
            }

            var user = await CurrentUser();

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return NotFound(new GetCaseOutcomeResponse());
            }

            var result = Mapper.Map<GetCaseOutcomeResponse>(examination);

            return Ok(result);
        }
    }
}
