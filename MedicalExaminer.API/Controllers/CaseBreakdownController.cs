using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
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
    public class CaseBreakdownController : BaseController
    {
        private IAsyncQueryHandler<CreateEventQuery, string> _eventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public CaseBreakdownController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateEventQuery, string> eventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService)
            : base(logger, mapper)
        {
            _eventCreationService = eventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPut]
        [Route("{examinationId}/bereaved_discussion")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutBereavedDiscussionEventResponse>> UpsertNewBereavedDiscussionEvent(string examinationId,
            [FromBody]
            PutBereavedDiscussionEventRequest putNewBereavedDiscussionEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutBereavedDiscussionEventResponse());
            }

            if (putNewBereavedDiscussionEventNoteRequest == null)
            {
                return BadRequest(new PutBereavedDiscussionEventResponse());
            }

            var bereavedDiscussionEventNote = Mapper.Map<BereavedDiscussionEvent>(putNewBereavedDiscussionEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, bereavedDiscussionEventNote));

            if (result == null)
            {
                return NotFound(new PutBereavedDiscussionEventResponse());
            }

            var res = new PutBereavedDiscussionEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{examinationId}/prescrutiny")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPreScrutinyEventResponse>> UpsertNewPreScrutinyEvent(string examinationId,
            [FromBody]
            PutPreScrutinyEventRequest putNewPreScrutinyEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutPreScrutinyEventResponse());
            }

            if (putNewPreScrutinyEventNoteRequest == null)
            {
                return BadRequest(new PutPreScrutinyEventResponse());
            }

            var preScrutinyEventNote = Mapper.Map<PreScrutinyEvent>(putNewPreScrutinyEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, preScrutinyEventNote));

            if (result == null)
            {
                return NotFound(new PutPreScrutinyEventResponse());
            }

            var res = new PutPreScrutinyEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{caseId}/medical_history")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutMedicalHistoryEventResponse>> UpsertNewMedicalHistoryEvent(string caseId,
            [FromBody]
            PutMedicalHistoryEventRequest putMedicalHistoryEventRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutMedicalHistoryEventResponse());
            }

            if (putMedicalHistoryEventRequest == null)
            {
                return BadRequest(new PutMedicalHistoryEventResponse());
            }

            var preScrutinyEventNote = Mapper.Map<MedicalHistoryEvent>(putMedicalHistoryEventRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(caseId, preScrutinyEventNote));

            if (result == null)
            {
                return NotFound(new PutMedicalHistoryEventResponse());
            }

            var res = new PutMedicalHistoryEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{examinationId}/other")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutOtherEventResponse>> UpsertNewOtherEvent(string examinationId,
            [FromBody]
            PutOtherEventRequest putNewOtherEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutOtherEventResponse());
            }

            if (putNewOtherEventNoteRequest == null)
            {
                return BadRequest(new PutOtherEventResponse());
            }

            var otherEventNote = Mapper.Map<OtherEvent>(putNewOtherEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, otherEventNote));

            if(result == null)
            {
                return NotFound(new PutOtherEventResponse());
            }

            var res = new PutOtherEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

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

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null));
            
            if(examination == null)
            {
                return new NotFoundObjectResult(new GetCaseBreakdownResponse());
            }

            var result = Mapper.Map<CaseBreakDownItem>(examination);

            return Ok(new GetCaseBreakdownResponse
            {
                CaseBreakdown = result
            });
        }

        [HttpPut]
        [Route("{examinationId}/qap_discussion")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutQapDiscussionEventResponse>> UpsertNewQapDiscussionEvent(string examinationId,
            [FromBody]
            PutQapDiscussionEventRequest putNewQapDiscussionEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutQapDiscussionEventResponse());
            }

            if (putNewQapDiscussionEventNoteRequest == null)
            {
                return BadRequest(new PutQapDiscussionEventResponse());
            }

            var qapDiscussionEventNote = Mapper.Map<QapDiscussionEvent>(putNewQapDiscussionEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, qapDiscussionEventNote));

            if (result == null)
            {
                return NotFound(new PutQapDiscussionEventResponse());
            }

            var res = new PutQapDiscussionEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpPut]
        [Route("{examinationId}/meo_summary")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutMeoSummaryEventResponse>> UpsertNewMeoSummaryEvent(string examinationId,
            [FromBody]
            PutMeoSummaryEventRequest putNewMeoSummaryEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutMeoSummaryEventResponse());
            }

            if (putNewMeoSummaryEventNoteRequest == null)
            {
                return BadRequest(new PutMeoSummaryEventResponse());
            }

            var meoSummaryEvent = Mapper.Map<MeoSummaryEvent>(putNewMeoSummaryEventNoteRequest);
            var result = await _eventCreationService.Handle(new CreateEventQuery(examinationId, meoSummaryEvent));

            if (result == null)
            {
                return NotFound(new PutMeoSummaryEventResponse());
            }

            var res = new PutMeoSummaryEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }
    }
}
