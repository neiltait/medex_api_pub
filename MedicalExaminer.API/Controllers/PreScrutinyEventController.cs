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
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    public class PreScrutinyEventController : Controller
    {
        private IAsyncQueryHandler<CreatePreScrutinyEventQuery, string> _preScrutinyEventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public PreScrutinyEventController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreatePreScrutinyEventQuery, string> preScrutinyEventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService)
            : base(logger, mapper)
        {
            _preScrutinyEventCreationService = preScrutinyEventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPut]
        [Route("{caseId}/PreScrutinyevent")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPreScrutinyEventResponse>> UpsertNewPreScrutinyEvent(string caseId,
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

            var PreScrutinyEventNote = Mapper.Map<PreScrutinyEvent>(putNewPreScrutinyEventNoteRequest);
            var result = await _preScrutinyEventCreationService.Handle(new CreatePreScrutinyEventQuery(caseId, PreScrutinyEventNote));

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

        [HttpGet]
        [Route("{caseId}/events/{eventType}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPreScrutinyEventResponse>> GetPreScrutinyEvent(string examinationId)
        {

            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetPreScrutinyEventResponse());
            }

            if (!Guid.TryParse(examinationId, out Guid examinationGuid))
            {
                return BadRequest(new GetPreScrutinyEventResponse());
            }

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));

            if (examination == null)
            {
                return new NotFoundObjectResult(new GetPreScrutinyEventResponse());
            }

            var result = Mapper.Map<GetPreScrutinyEventResponse>(examination);

            return Ok(result);
        }
    }
}