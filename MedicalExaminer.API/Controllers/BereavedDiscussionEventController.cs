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
    public class BereavedDiscussionEventController : BaseController
    {
        private IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string> _bereavedDiscussionEventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public BereavedDiscussionEventController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateBereavedDiscussionEventQuery, string> bereavedDiscussionEventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService)
            : base(logger, mapper)
        {
            _bereavedDiscussionEventCreationService = bereavedDiscussionEventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPut]
        [Route("{caseId}/BereavedDiscussionevent")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutBereavedDiscussionEventResponse>> UpsertNewBereavedDiscussionEvent(string caseId,
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

            var BereavedDiscussionEventNote = Mapper.Map<BereavedDiscussionEvent>(putNewBereavedDiscussionEventNoteRequest);
            var result = await _bereavedDiscussionEventCreationService.Handle(new CreateBereavedDiscussionEventQuery(caseId, BereavedDiscussionEventNote));

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

        [HttpGet]
        [Route("{caseId}/events/{eventType}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetBereavedDiscussionEventResponse>> GetBereavedDiscussionEvent(string examinationId)
        {

            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetBereavedDiscussionEventResponse());
            }

            if (!Guid.TryParse(examinationId, out Guid examinationGuid))
            {
                return BadRequest(new GetBereavedDiscussionEventResponse());
            }

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));

            if (examination == null)
            {
                return new NotFoundObjectResult(new GetBereavedDiscussionEventResponse());
            }

            var result = Mapper.Map<GetBereavedDiscussionEventResponse>(examination);

            return Ok(result);
        }
    }
}
