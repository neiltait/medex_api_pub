using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations")]
    [ApiController]
    [Authorize]
    public class CaseBreakdownController : AuthorizedBaseController
    {
        private IAsyncQueryHandler<CreateEventQuery, string> _eventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public CaseBreakdownController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<CreateEventQuery, string> eventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService)
            : base(logger, mapper, usersRetrievalByEmailService, authorizationService, permissionService)
        {
            _eventCreationService = eventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        /// <summary>
        /// returns a casebreakdown object for the given examination
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{examinationId}/casebreakdown")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetCaseBreakdownResponse>> GetCaseBreakdown(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetCaseBreakdownResponse());
            }

            Guid examinationGuid;
            if (!Guid.TryParse(examinationId, out examinationGuid))
            {
                return BadRequest(new GetCaseBreakdownResponse());
            }

            var user = await CurrentUser();

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));
            var patientCard = Mapper.Map<PatientCardItem>(examination);

            if (examination == null)
            {
                return new NotFoundObjectResult(new GetCaseBreakdownResponse());
            }

            var result = Mapper.Map<CaseBreakDownItem>(examination.CaseBreakdown, opt => opt.Items["user"] = user);

            return Ok(new GetCaseBreakdownResponse
            {
                Header = patientCard,
                CaseBreakdown = result
            });
        }

        [HttpPut]
        [Route("{examinationId}/bereaved_discussion")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewBereavedDiscussionEvent(
            string examinationId,
            [FromBody] PutBereavedDiscussionEventRequest putNewBereavedDiscussionEventNoteRequest)
        {
            return await UpsertEvent<BereavedDiscussionEvent, PutBereavedDiscussionEventRequest>(examinationId,
                putNewBereavedDiscussionEventNoteRequest);
        }

        [HttpPut]
        [Route("{examinationId}/prescrutiny")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewPreScrutinyEvent(
            string examinationId,
            [FromBody] PutPreScrutinyEventRequest putNewPreScrutinyEventNoteRequest)
        {
            return await UpsertEvent<PreScrutinyEvent, PutPreScrutinyEventRequest>(examinationId,
                putNewPreScrutinyEventNoteRequest);
        }

        [HttpPut]
        [Route("{examinationId}/medical_history")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewMedicalHistoryEvent(
            string examinationId,
            [FromBody] PutMedicalHistoryEventRequest putMedicalHistoryEventRequest)
        {
            return await UpsertEvent<MedicalHistoryEvent, PutMedicalHistoryEventRequest>(examinationId,
                putMedicalHistoryEventRequest);
        }

        [HttpPut]
        [Route("{examinationId}/admission")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewAdmissionEvent(
            string examinationId,
            [FromBody] PutAdmissionEventRequest putNewAdmissionEventNoteRequest)
        {
            return await UpsertEvent<AdmissionEvent, PutAdmissionEventRequest>(examinationId,
                putNewAdmissionEventNoteRequest);
        }

        [HttpPut]
        [Route("{examinationId}/other")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewOtherEvent(
            string examinationId,
            [FromBody] PutOtherEventRequest putNewOtherEventNoteRequest)
        {
            return await UpsertEvent<OtherEvent, PutOtherEventRequest>(examinationId, putNewOtherEventNoteRequest);
        }

        [HttpPut]
        [Route("{examinationId}/qap_discussion")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewQapDiscussionEvent(
            string examinationId,
            [FromBody] PutQapDiscussionEventRequest putNewQapDiscussionEventNoteRequest)
        {
            return await UpsertEvent<QapDiscussionEvent, PutQapDiscussionEventRequest>(examinationId,
                putNewQapDiscussionEventNoteRequest);
        }

        [HttpPut]
        [Route("{examinationId}/meo_summary")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewMeoSummaryEvent(
            string examinationId,
            [FromBody] PutMeoSummaryEventRequest putNewMeoSummaryEventNoteRequest)
        {
            return await UpsertEvent<MeoSummaryEvent, PutMeoSummaryEventRequest>(examinationId,
                putNewMeoSummaryEventNoteRequest);
        }

        private T SetEventUserStatuses<T>(T theEvent, MeUser user)
            where T : IEvent
        {
            theEvent.UserId = user.UserId;
            theEvent.UserFullName = user.FirstName + user.LastName;
            return theEvent;
        }

        private async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertEvent<TEvent, TRequest>(
            string examinationId,
            [FromBody] TRequest putNewMeoSummaryEventNoteRequest)
            where TEvent : IEvent
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (putNewMeoSummaryEventNoteRequest == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var user = await CurrentUser();
            var meoSummaryEvent = Mapper.Map<TEvent>(putNewMeoSummaryEventNoteRequest);
            meoSummaryEvent = SetEventUserStatuses(meoSummaryEvent, user);

            var examination =
                await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (!CanAsync(Permission.UpdateExamination, examination))
            {
                return Forbid();
            }

            var patientCard = Mapper.Map<PatientCardItem>(examination);

            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, meoSummaryEvent));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var res = new PutCaseBreakdownEventResponse
            {
                Header = patientCard,
                EventId = result
            };

            return Ok(res);
        }
    }
}
