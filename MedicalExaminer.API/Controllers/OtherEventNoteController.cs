using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.CaseBreakdown;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MedicalExaminer.API.Controllers
{
    [Route("examinations")]
    [ApiController]
    [Authorize]
    public class OtherEventNoteController : BaseController
    {
        private IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, EventNote> _otherCaseEventRetrievalService;

        private IAsyncQueryHandler<CreateOtherEventQuery, string> _otherCaseEventCreationService;


        public OtherEventNoteController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, EventNote> otherCaseEventRetrievalService,
            IAsyncQueryHandler<CreateOtherEventQuery, string> otherCaseEventCreationService)
            : base(logger, mapper)
        {
            _otherCaseEventRetrievalService = otherCaseEventRetrievalService;
            _otherCaseEventCreationService = otherCaseEventCreationService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutOtherEventResponse>> CreateNewOtherEventNote(
            [FromBody]
            PutOtherEventResponse postNewOtherEventNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutOtherEventResponse());
            }

            var otherEventNote = Mapper.Map<EventNote>(postNewOtherEventNoteRequest);
            var result = await _otherCaseEventCreationService.Handle(new CreateOtherEventQuery(otherEventNote));
            var res = new PutOtherEventResponse
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
