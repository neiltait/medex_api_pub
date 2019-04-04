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
    public class PreScrutinyEventController : BaseController
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
    }
}