using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
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
    public class OtherEventController : BaseController
    {
        private IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, OtherEvent> _otherCaseEventRetrievalService;

        private IAsyncQueryHandler<CreateOtherEventQuery, string> _otherEventCreationService;


        public OtherEventController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, OtherEvent> otherCaseEventRetrievalService,
            IAsyncQueryHandler<CreateOtherEventQuery, string> otherEventCreationService)
            : base(logger, mapper)
        {
            _otherCaseEventRetrievalService = otherCaseEventRetrievalService;
            _otherEventCreationService = otherEventCreationService;
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
        [Route("{caseId}/otherevent")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetOtherEventResponse>> GetOtherEvent(string caseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetOtherEventResponse());
            }

            if (await _otherCaseEventRetrievalService.Handle(new OtherCaseEventByCaseIdQuery(caseId)) == null)
            {
                return NotFound(new GetOtherEventResponse());
            }

            var result = await _otherCaseEventRetrievalService.Handle(new OtherCaseEventByCaseIdQuery(caseId));


            return Ok(Mapper.Map<GetOtherEventResponse>(result));
        }
    }
}
