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
    /// <summary>
    /// Patient Details Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}/patient_details")]
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

        /// <summary>
        /// Initialise a new instance of <see cref="PatientDetailsController"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="examinationRetrievalService"></param>
        /// <param name="patientDetailsUpdateService"></param>
        /// <param name="patientDetailsByCaseIdService"></param>
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

        /// <summary>
        /// Get Patient Details.
        /// </summary>
        /// <param name="examinationId">Examination Id.</param>
        /// <returns>Get Patient Details Response.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetPatientDetailsResponse>> GetPatientDetails(string examinationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetPatientDetailsResponse());
            }

            if (await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null)) == null)
            {
                return NotFound(new GetPatientDetailsResponse());
            }

            var result = await _patientDetailsByCaseIdService.Handle(new PatientDetailsByCaseIdQuery(examinationId));


            return Ok(Mapper.Map<GetPatientDetailsResponse>(result));
        }

        /// <summary>
        /// Update Patient Details.
        /// </summary>
        /// <param name="examinationId">Examination Id.</param>
        /// <param name="putPatientDetailsRequest">Put Patient Details Request.</param>
        /// <returns></returns>
        [HttpPut]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public ActionResult<PutPatientDetailsResponse> UpdatePatientDetails(string examinationId, [FromBody]PutPatientDetailsRequest putPatientDetailsRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutPatientDetailsResponse());
            }

            if (_examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null)) == null)
            {
                return NotFound("Case was not found");
            }

            var patientDetails = Mapper.Map<PatientDetails>(putPatientDetailsRequest);

            var result = _patientDetailsUpdateService.Handle(new PatientDetailsUpdateQuery(examinationId, patientDetails));

            return Ok(new PutPatientDetailsResponse
            {
                ExaminationId = result.Result.ExaminationId
            });
        }
    }
}