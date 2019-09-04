using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.MELogger;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Case Breakdown Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations")]
    [ApiController]
    [Authorize]
    public class CaseBreakdownController : AuthorizedBaseController
    {
        private readonly IAsyncQueryHandler<CreateEventQuery, EventCreationResult> _eventCreationService;
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        /// <summary>
        /// Initialise a new instance of <see cref="CaseBreakdownController"/>.
        /// </summary>
        public CaseBreakdownController(
            IAsyncQueryHandler<CreateMELoggerQuery, LogMessageActionDefault> logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<CreateEventQuery, EventCreationResult> eventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _eventCreationService = eventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        /// <summary>
        /// Returns a <see cref="GetCaseBreakdownResponse"/> object for the given examination.
        /// </summary>
        /// <param name="examinationId">The examination id.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{examinationId}/casebreakdown")]
        public async Task<ActionResult<GetCaseBreakdownResponse>> GetCaseBreakdown(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetCaseBreakdownResponse());
            }

            if (!Guid.TryParse(examinationId, out _))
            {
                return BadRequest(new GetCaseBreakdownResponse());
            }

            var user = await CurrentUser();

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return new NotFoundObjectResult(new GetCaseBreakdownResponse());
            }

            if (!CanAsync(Permission.GetExamination, examination))
            {
                return Forbid();
            }

            var patientCard = Mapper.Map<PatientCardItem>(examination);
            var result = Mapper.Map<CaseBreakDownItem>(examination, opt => opt.Items["user"] = user);

            return Ok(new GetCaseBreakdownResponse
            {
                Header = patientCard,
                CaseBreakdown = result
            });
        }

        /// <summary>
        /// Upserts the new bereaved discussion event.
        /// </summary>
        /// <param name="examinationId">The examination identifier.</param>
        /// <param name="putNewBereavedDiscussionEventNoteRequest">The put new bereaved discussion event note request.</param>
        /// <returns>Response.</returns>
        [HttpPut]
        [Route("{examinationId}/bereaved_discussion")]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewBereavedDiscussionEvent(
            string examinationId,
            [FromBody] PutBereavedDiscussionEventRequest putNewBereavedDiscussionEventNoteRequest)
        {
            return await UpsertEvent<BereavedDiscussionEvent, PutBereavedDiscussionEventRequest>(
                examinationId,
                Permission.BereavedDiscussionEvent,
                putNewBereavedDiscussionEventNoteRequest);
        }

        /// <summary>
        /// Upserts the new pre scrutiny event.
        /// </summary>
        /// <param name="examinationId">The examination identifier.</param>
        /// <param name="putNewPreScrutinyEventNoteRequest">The put new pre scrutiny event note request.</param>
        /// <returns>Response.</returns>
        [HttpPut]
        [Route("{examinationId}/prescrutiny")]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewPreScrutinyEvent(
            string examinationId,
            [FromBody]
            PutPreScrutinyEventRequest putNewPreScrutinyEventNoteRequest)
        {
            return await UpsertEvent<PreScrutinyEvent, PutPreScrutinyEventRequest>(
                examinationId,
                Permission.PreScrutinyEvent,
                putNewPreScrutinyEventNoteRequest);
        }

        /// <summary>
        /// Upserts the new medical history event.
        /// </summary>
        /// <param name="examinationId">The examination identifier.</param>
        /// <param name="putMedicalHistoryEventRequest">The put medical history event request.</param>
        /// <returns>Response.</returns>
        [HttpPut]
        [Route("{examinationId}/medical_history")]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewMedicalHistoryEvent(
            string examinationId,
            [FromBody] PutMedicalHistoryEventRequest putMedicalHistoryEventRequest)
        {
            return await UpsertEvent<MedicalHistoryEvent, PutMedicalHistoryEventRequest>(
                examinationId,
                Permission.MedicalHistoryEvent,
                putMedicalHistoryEventRequest);
        }

        /// <summary>
        /// Upserts the new admission event.
        /// </summary>
        /// <param name="examinationId">The examination identifier.</param>
        /// <param name="putNewAdmissionEventNoteRequest">The put new admission event note request.</param>
        /// <returns>Response.</returns>
        [HttpPut]
        [Route("{examinationId}/admission")]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewAdmissionEvent(
            string examinationId,
            [FromBody] PutAdmissionEventRequest putNewAdmissionEventNoteRequest)
        {
            return await UpsertEvent<AdmissionEvent, PutAdmissionEventRequest>(
                examinationId,
                Permission.AdmissionEvent,
                putNewAdmissionEventNoteRequest);
        }

        /// <summary>
        /// Upserts the new other event.
        /// </summary>
        /// <param name="examinationId">The examination identifier.</param>
        /// <param name="putNewOtherEventNoteRequest">The put new other event note request.</param>
        /// <returns>Response.</returns>
        [HttpPut]
        [Route("{examinationId}/other")]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewOtherEvent(
            string examinationId,
            [FromBody] PutOtherEventRequest putNewOtherEventNoteRequest)
        {
            return await UpsertEvent<OtherEvent, PutOtherEventRequest>(
                examinationId, 
                Permission.OtherEvent,
                putNewOtherEventNoteRequest);
        }

        /// <summary>
        /// Upserts the new qap discussion event.
        /// </summary>
        /// <param name="examinationId">The examination identifier.</param>
        /// <param name="putNewQapDiscussionEventNoteRequest">The put new qap discussion event note request.</param>
        /// <returns>Response.</returns>
        [HttpPut]
        [Route("{examinationId}/qap_discussion")]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewQapDiscussionEvent(
            string examinationId,
            [FromBody] PutQapDiscussionEventRequest putNewQapDiscussionEventNoteRequest)
        {
            return await UpsertEvent<QapDiscussionEvent, PutQapDiscussionEventRequest>(
                examinationId,
                Permission.QapDiscussionEvent,
                putNewQapDiscussionEventNoteRequest);
        }

        /// <summary>
        /// Upserts the new meo summary event.
        /// </summary>
        /// <param name="examinationId">The examination identifier.</param>
        /// <param name="putNewMeoSummaryEventNoteRequest">The put new meo summary event note request.</param>
        /// <returns>Response.</returns>
        [HttpPut]
        [Route("{examinationId}/meo_summary")]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewMeoSummaryEvent(
            string examinationId,
            [FromBody] PutMeoSummaryEventRequest putNewMeoSummaryEventNoteRequest)
        {
            return await UpsertEvent<MeoSummaryEvent, PutMeoSummaryEventRequest>(
                examinationId,
                Permission.MeoSummaryEvent,
                putNewMeoSummaryEventNoteRequest);
        }

        private T SetEventUserStatuses<T>(T theEvent, MeUser user)
            where T : IEvent
        {
            theEvent.UserId = user.UserId;
            theEvent.UserFullName = user.FullName();
            theEvent.UsersRole = user.UsersRoleIn(new[] { UserRoles.MedicalExaminer, UserRoles.MedicalExaminerOfficer }).ToString();
            return theEvent;
        }

        private async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertEvent<TEvent, TRequest>(
            string examinationId,
            Permission permission,
            [FromBody] TRequest caseBreakdownEvent)
            where TEvent : IEvent
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (caseBreakdownEvent == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var user = await CurrentUser();
            var theEvent = Mapper.Map<TEvent>(caseBreakdownEvent);
            theEvent = SetEventUserStatuses(theEvent, user);

            var examination =
                await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            if (!CanAsync(permission, examination))
            {
                return Forbid();
            }

            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, theEvent));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var patientCard = Mapper.Map<PatientCardItem>(result.Examination);

            var res = new PutCaseBreakdownEventResponse
            {
                Header = patientCard,
                EventId = result.EventId
            };

            return Ok(res);
        }
    }
}
