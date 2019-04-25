using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Queries.User;
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
    public class PatientDetailsController : AuthenticatedBaseController
    {
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>
            _examinationRetrievalService;

        private readonly IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination>
            _patientDetailsByCaseIdService;

        private readonly IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> _usersRetrievalByEmailService;
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
        /// <param name="usersRetrievalByEmailService"></param>
        public PatientDetailsController(IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination> patientDetailsUpdateService,
            IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination> patientDetailsByCaseIdService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _patientDetailsUpdateService = patientDetailsUpdateService;
            _patientDetailsByCaseIdService = patientDetailsByCaseIdService;
            _usersRetrievalByEmailService = usersRetrievalByEmailService;
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

            var myUser = await CurrentUser();

            var examination = _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, myUser)).Result;

            if (examination == null)
            {
                return NotFound(new GetPatientDetailsResponse());
            }

            var result = Mapper.Map<GetPatientDetailsResponse>(examination);
            return Ok(result);

        }

        /// <summary>
        /// Update Patient Details.
        /// </summary>
        /// <param name="examinationId">Examination Id.</param>
        /// <param name="putPatientDetailsRequest">Put Patient Details Request.</param>
        /// <returns>PutPatientDetailsResponse</returns>
        [HttpPut]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPatientDetailsResponse>> UpdatePatientDetails(string examinationId, [FromBody]PutPatientDetailsRequest putPatientDetailsRequest)
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

            var myUser = await CurrentUser();

            var result =await _patientDetailsUpdateService.Handle(new PatientDetailsUpdateQuery(examinationId, patientDetails, myUser.UserId));

            var patientCard = Mapper.Map<PatientCardItem>(result);

            return Ok(new PutPatientDetailsResponse
            {
                Header = patientCard
            });
        }
    }
}