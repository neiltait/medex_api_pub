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
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MedicalExaminer.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations")]
    [ApiController]
    [Authorize]
    public class OtherEventController : BaseController
    {
        private IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, OtherEvent> _otherCaseEventRetrievalService;
        private IAsyncQueryHandler<CreateOtherEventQuery, string> _otherEventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public OtherEventController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, OtherEvent> otherCaseEventRetrievalService,
            IAsyncQueryHandler<CreateOtherEventQuery, string> otherEventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService)
            : base(logger, mapper)
        {
            _otherCaseEventRetrievalService = otherCaseEventRetrievalService;
            _otherEventCreationService = otherEventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPost]
        [Route("{caseId}/otherevent")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PostOtherEventResponse>> CreateNewOtherEventNote(string caseId,
            [FromBody]
            PostOtherEventRequest postNewOtherEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PostOtherEventResponse());
            }

            var otherEventNote = Mapper.Map<OtherEvent>(postNewOtherEventNoteRequest);
            var result = await _otherEventCreationService.Handle(new CreateOtherEventQuery(caseId, otherEventNote));
            var res = new PostOtherEventResponse
            {
                EventId = result
            };

            return Ok(res);
        }

        [HttpGet]
        [Route("{caseId}/events/{eventType}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetOtherEventResponse>> GetOtherEvent(string examinationId, EventType? eventType)
        {
            
            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetOtherEventResponse());
            }

            Guid examinationGuid;
            if(!Guid.TryParse(examinationId, out examinationGuid))
            {
                return BadRequest(new GetOtherEventResponse());
            }

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));

            if(examination == null)
            {
                return new NotFoundObjectResult(new GetOtherEventResponse());
            }

            var result = Mapper.Map<GetOtherEventResponse>(examination);

            return Ok(result);
        }
    }
}
