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
    [Route("examinations/{caseId}")]
    [ApiController]
    [Authorize]
    public class PatientDetailsController : BaseController
    {
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>
            _examinationRetrievalService;

        private readonly IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination>
            _patientDetailsByCaseIdService;

        private readonly IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>
            _patientDetailsUpdateService;

        public PatientDetailsController(IMELogger logger, IMapper mapper,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination> patientDetailsUpdateService,
            IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination> patientDetailsByCaseIdService)
            : base(logger, mapper)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _patientDetailsUpdateService = patientDetailsUpdateService;
            _patientDetailsByCaseIdService = patientDetailsByCaseIdService;
        }

        [HttpGet]
        [Route("patientdetails")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPatientDetailsResponse>> GetPatientDetails(string caseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetPatientDetailsResponse());
            }

            if (await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(caseId)) == null)
            {
                return NotFound(new GetPatientDetailsResponse());
            }

            var result = await _patientDetailsByCaseIdService.Handle(new PatientDetailsByCaseIdQuery(caseId));


            return Ok(Mapper.Map<GetPatientDetailsResponse>(result));
        }


        [HttpPut]
        [Route("patientdetails")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPatientDetailsResponse>> UpdatePatientDetails(string caseId,
            [FromBody]
            PutPatientDetailsRequest putPatientDetailsRequest)
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


            return Ok(new PutPatientDetailsResponse
            {
                ExaminationId = result.Result.ExaminationId
            });
        }
    }
}