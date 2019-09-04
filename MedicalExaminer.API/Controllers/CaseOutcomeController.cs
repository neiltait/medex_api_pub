using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Case Outcome Controller.
    /// </summary>
    /// <seealso cref="MedicalExaminer.API.Controllers.AuthorizedBaseController" />
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    public class CaseOutcomeController : AuthorizedBaseController
    {
        private IAsyncQueryHandler<CoronerReferralQuery, string> _coronerReferralService;
        private IAsyncQueryHandler<CloseCaseQuery, string> _closeCaseService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string> _saveOutstandingCaseItemsService;
        private IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination> _confirmationOfScrutinyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseOutcomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="coronerReferralService">The coroner referral service.</param>
        /// <param name="closeCaseService">The close case service.</param>
        /// <param name="examinationRetrievalService">The examination retrieval service.</param>
        /// <param name="saveOutstandingCaseItemsService">The save outstanding case items service.</param>
        /// <param name="confirmationOfScrutinyService">The confirmation of scrutiny service.</param>
        /// <param name="usersRetrievalByEmailService">The users retrieval by email service.</param>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="permissionService">The permission service.</param>
        public CaseOutcomeController(
            IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> logger,
            IMapper mapper,
            IAsyncQueryHandler<CoronerReferralQuery, string> coronerReferralService,
            IAsyncQueryHandler<CloseCaseQuery, string> closeCaseService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string> saveOutstandingCaseItemsService,
            IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination> confirmationOfScrutinyService,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _coronerReferralService = coronerReferralService;
            _closeCaseService = closeCaseService;
            _examinationRetrievalService = examinationRetrievalService;
            _saveOutstandingCaseItemsService = saveOutstandingCaseItemsService;
            _confirmationOfScrutinyService = confirmationOfScrutinyService;
        }

        /// <summary>
        /// Confirmation of Scrutiny
        /// </summary>
        /// <param name="examinationId"> Examination ID</param>
        /// <returns>PutConfirmationOfScrutinyResponse type</returns>
        [HttpPut]
        [Route("confirmation_of_scrutiny")]
        public async Task<ActionResult<PutConfirmationOfScrutinyResponse>> PutConfirmationOfScrutiny(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new PutConfirmationOfScrutinyResponse());
            }

            var user = await CurrentUser();

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (!CanAsync(Permission.UpdateExamination, examination))
            {
                return Forbid();
            }

            if (user.UserId != examination.MedicalTeam.MedicalExaminerUserId)
            {
                return BadRequest();
            }

            if (!examination.CalculateCanCompleteScrutiny())
            {
                return new BadRequestObjectResult("Scrutiny needs to be performed before confirming scrutiny is complete.");
            }

            var result = await _confirmationOfScrutinyService.Handle(new ConfirmationOfScrutinyQuery(examinationId, user));
            return Ok(Mapper.Map<PutConfirmationOfScrutinyResponse>(result));
        }

        /// <summary>
        /// Send Coroner Referral
        /// </summary>
        /// <param name="examinationId">Examination Id</param>
        /// <returns>Response</returns>
        [HttpPut]
        [Route("coroner_referral")]
        public async Task<ActionResult> PutCoronerReferral(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            if (!Guid.TryParse(examinationId, out _))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var user = await CurrentUser();
            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return new NotFoundResult();
            }

            if (!CanAsync(Permission.UpdateExamination, examination))
            {
                return Forbid();
            }

            if (!examination.CalculateRequiresCoronerReferral())
            {
                return BadRequest();
            }

            await _coronerReferralService.Handle(new CoronerReferralQuery(examinationId, user));

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

            if (!Guid.TryParse(examinationId, out _))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var user = await CurrentUser();
            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return new NotFoundResult();
            }

            if (!CanAsync(Permission.UpdateExamination, examination))
            {
                return Forbid();
            }

            if (!examination.ScrutinyConfirmed)
            {
                return new BadRequestObjectResult("Scrutiny has not yet been confirmed.");
            }

            var caseOutcome = Mapper.Map<CaseOutcome>(putOutstandingCaseItemsRequest);
            await _saveOutstandingCaseItemsService.Handle(new SaveOutstandingCaseItemsQuery(examinationId, caseOutcome, user));
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

            if (!Guid.TryParse(examinationId, out _))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var user = await CurrentUser();
            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return new NotFoundResult();
            }

            if (examination.CaseCompleted)
            {
                return BadRequest();
            }

            if (!CanAsync(Permission.UpdateExamination, examination))
            {
                return Forbid();
            }

            if (examination.CalculateOutstandingCaseOutcomesCompleted())
            {
                await _closeCaseService.Handle(new CloseCaseQuery(examinationId, user));
                return Ok();
            }

            if (!examination.OutstandingCaseItemsCompleted)
            {
                return new BadRequestObjectResult("examination still has outstanding case items that need to be completed.");
            }

            return BadRequest();
        }

        /// <summary>
        /// Get Case Outcome details
        /// </summary>
        /// <returns>Case Outcome Details</returns>
        [HttpGet]
        public async Task<ActionResult<GetCaseOutcomeResponse>> GetCaseOutcome(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetCaseOutcomeResponse());
            }

            if (!Guid.TryParse(examinationId, out _))
            {
                return BadRequest(new GetCaseOutcomeResponse());
            }

            var user = await CurrentUser();

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return NotFound(new GetCaseOutcomeResponse());
            }

            if (!CanAsync(Permission.GetExamination, examination))
            {
                return Forbid();
            }

            var result = Mapper.Map<GetCaseOutcomeResponse>(examination);

            return Ok(result);
        }
    }
}
