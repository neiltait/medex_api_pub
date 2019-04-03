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
        private IAsyncQueryHandler<CreateEventQuery, string> _otherEventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public CaseBreakdownController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateEventQuery, string> otherEventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService)
            : base(logger, mapper)
        {
            _otherEventCreationService = otherEventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPut]
        [Route("{caseId}/otherevent")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutOtherEventResponse>> UpsertNewOtherEvent(string caseId,
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
            var result = await _otherEventCreationService.Handle(new CreateEventQuery(caseId, otherEventNote));

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
        [Route("{caseId}")]
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
    }
}
