using System;
using System.Collections.Generic;
using System.Linq;
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
    public class QapDiscussionEventController : BaseController
    {
        private IAsyncQueryHandler<CreateQapDiscussionEventQuery, string> _qapDiscussionEventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public QapDiscussionEventController(
            IMELogger logger, 
            IMapper mapper,
            IAsyncQueryHandler<CreateQapDiscussionEventQuery, string> qapDiscussionEventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService) 
            : base(logger, mapper)
        {
            _qapDiscussionEventCreationService = qapDiscussionEventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPut]
        [Route("{caseId}/QapDiscussionevent")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutQapDiscussionEventResponse>> UpsertNewQapDiscussionEvent(string caseId,
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
            var result = await _qapDiscussionEventCreationService.Handle(new CreateQapDiscussionEventQuery(caseId, qapDiscussionEventNote));

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

        [HttpGet]
        [Route("{caseId}/events/{eventType}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetQapDiscussionEventResponse>> GetQapDiscussionEvent(string examinationId)
        {

            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetQapDiscussionEventResponse());
            }

            if (!Guid.TryParse(examinationId, out Guid examinationGuid))
            {
                return BadRequest(new GetQapDiscussionEventResponse());
            }

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));

            if (examination == null)
            {
                return new NotFoundObjectResult(new GetQapDiscussionEventResponse());
            }

            var result = Mapper.Map<GetQapDiscussionEventResponse>(examination);

            return Ok(result);
        }
    }
}