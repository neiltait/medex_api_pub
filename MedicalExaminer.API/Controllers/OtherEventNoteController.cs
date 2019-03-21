using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.CaseBreakdownOther;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalExaminer.API.Controllers
{
    [Route("examinations")]
    [ApiController]
    [Authorize]
    public class OtherEventNoteController : BaseController
    {
        private IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, CaseEvent> _otherCaseEventRetrievalService;

        private IAsyncQueryHandler<CreateOtherCaseEventQuery, CaseEvent> _otherCaseEventCreationService;


        public OtherEventNoteController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<OtherCaseEventByCaseIdQuery, CaseEvent> otherCaseEventRetrievalService,
            IAsyncQueryHandler<CreateOtherCaseEventQuery, CaseEvent> otherCaseEventCreationService)
            : base(logger, mapper)
        {
            _otherCaseEventRetrievalService = otherCaseEventRetrievalService;
            _otherCaseEventCreationService = otherCaseEventCreationService;
        }

        [HttpGet]
        [Route("{caseId}/casebreakdownother")]
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
