using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MedicalExaminer.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations")]
    [ApiController]
    [Authorize]
    public class CaseBreakdownController : AuthenticatedBaseController
    {
        private IAsyncQueryHandler<CreateEventQuery, string> _eventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        
        public CaseBreakdownController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateEventQuery, string> eventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _eventCreationService = eventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPut]
        [Route("{examinationId}/bereaved_discussion")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewBereavedDiscussionEvent(string examinationId,
            [FromBody]
            PutBereavedDiscussionEventRequest putNewBereavedDiscussionEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (putNewBereavedDiscussionEventNoteRequest == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var bereavedDiscussionEventNote = Mapper.Map<BereavedDiscussionEvent>(putNewBereavedDiscussionEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, bereavedDiscussionEventNote));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var res = new PutCaseBreakdownEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{examinationId}/prescrutiny")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewPreScrutinyEvent(string examinationId,
            [FromBody]
            PutPreScrutinyEventRequest putNewPreScrutinyEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (putNewPreScrutinyEventNoteRequest == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var preScrutinyEventNote = Mapper.Map<PreScrutinyEvent>(putNewPreScrutinyEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, preScrutinyEventNote));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var res = new PutCaseBreakdownEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{examinationId}/medical_history")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewMedicalHistoryEvent(string examinationId,
            [FromBody]
            PutMedicalHistoryEventRequest putMedicalHistoryEventRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (putMedicalHistoryEventRequest == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var medicalHistoryEventNote = Mapper.Map<MedicalHistoryEvent>(putMedicalHistoryEventRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, medicalHistoryEventNote));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var res = new PutCaseBreakdownEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{examinationId}/admission")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewAdmissionEvent(string examinationId,
            [FromBody]
            PutAdmissionEventRequest putNewAdmissionEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (putNewAdmissionEventNoteRequest == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var admissionEventNote = Mapper.Map<AdmissionEvent>(putNewAdmissionEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, admissionEventNote));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var res = new PutCaseBreakdownEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{examinationId}/other")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewOtherEvent(string examinationId,
            [FromBody]
            PutOtherEventRequest putNewOtherEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (putNewOtherEventNoteRequest == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var user = await CurrentUser();

            var otherEventNote = Mapper.Map<OtherEvent>(putNewOtherEventNoteRequest);
            otherEventNote.UserId = user.UserId;
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, otherEventNote));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var res = new PutCaseBreakdownEventResponse
            {
                EventId = result
            };

            return Ok(res);
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
            if(!Guid.TryParse(examinationId, out examinationGuid))
            {
                return BadRequest(new GetCaseBreakdownResponse());
            }

            var myUser = await CurrentUser();

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, myUser));
            
            if(examination == null)
            {
                return new NotFoundObjectResult(new GetCaseBreakdownResponse());
            }

            var result = Mapper.Map<CaseBreakDownItem>(examination.CaseBreakdown, opt => opt.Items["myUser"] = myUser);

            return Ok(new GetCaseBreakdownResponse
            {
                CaseBreakdown = result
            });
        }

        [HttpPut]
        [Route("{examinationId}/qap_discussion")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewQapDiscussionEvent(string examinationId,
            [FromBody]
            PutQapDiscussionEventRequest putNewQapDiscussionEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (putNewQapDiscussionEventNoteRequest == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var qapDiscussionEventNote = Mapper.Map<QapDiscussionEvent>(putNewQapDiscussionEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, qapDiscussionEventNote));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var res = new PutCaseBreakdownEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{examinationId}/meo_summary")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutCaseBreakdownEventResponse>> UpsertNewMeoSummaryEvent(string examinationId,
            [FromBody]
            PutMeoSummaryEventRequest putNewMeoSummaryEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            if (putNewMeoSummaryEventNoteRequest == null)
            {
                return BadRequest(new PutCaseBreakdownEventResponse());
            }

            var meoSummaryEvent = Mapper.Map<MeoSummaryEvent>(putNewMeoSummaryEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, meoSummaryEvent));

            if (result == null)
            {
                return NotFound(new PutCaseBreakdownEventResponse());
            }

            var res = new PutCaseBreakdownEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }
    }
}
