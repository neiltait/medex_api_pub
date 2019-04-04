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
    public class MeoSummaryEventController : BaseController
    {
        private IAsyncQueryHandler<CreateMeoSummaryEventQuery, string> _meoSummaryEventCreationService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public MeoSummaryEventController(
            IMELogger logger, 
            IMapper mapper,
            IAsyncQueryHandler<CreateMeoSummaryEventQuery, string> meoSummaryEventCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService)
            : base(logger, mapper)
        {
            _meoSummaryEventCreationService = meoSummaryEventCreationService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        [HttpPut]
        [Route("{caseId}/MeoSummaryEvent")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutMeoSummaryEventResponse>> UpsertNewMeoSummaryEvent(string caseId,
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

            var meoSummaryEventNote = Mapper.Map<MeoSummaryEvent>(putNewMeoSummaryEventNoteRequest);
            var result = await _meoSummaryEventCreationService.Handle(new CreateMeoSummaryEventQuery(caseId, meoSummaryEventNote));

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