using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    [Route("patientdetails")]
    [ApiController]
    [Authorize]
    public class PatientDetailsController : BaseController
    {
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>
            _examinationRetrievalService;

        private IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>
            _patientDetailsUpdateService;

        public PatientDetailsController(IMELogger logger, IMapper mapper, IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService, IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination> patientDetailsUpdateService)
            : base(logger, mapper)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _patientDetailsUpdateService = patientDetailsUpdateService;
        }

        [HttpPut]
        [Route("/{caseId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPatientDetailsResponse>> CreateNewCase(string caseId, [FromBody]PutPatientDetailsRequest putPatientDetailsRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutPatientDetailsResponse());
            }

            if (_examinationRetrievalService.Handle(new ExaminationRetrievalQuery(caseId)) == null)
            {
                return NotFound("Case was not found");
            }

            var patientDetails = Mapper.Map<PatientDetails>(putPatientDetailsRequest);

            var result = _patientDetailsUpdateService.Handle(new PatientDetailsUpdateQuery(caseId, patientDetails));
            

            return Ok(new PutPatientDetailsResponse()
            {
                ExaminationId = result.Result.Id
            });
        }

    }
}