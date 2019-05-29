using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Extensions.Models;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Patient Details Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}/patient_details")]
    [ApiController]
    [Authorize]
    public class PatientDetailsController : AuthorizedBaseController
    {
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>
            _examinationRetrievalService;

        private readonly IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>
            _patientDetailsUpdateService;

        private readonly IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> _locationParentsService;

        /// <summary>
        /// Initialise a new instance of <see cref="PatientDetailsController"/>.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByEmailService">Users Retrieval By Email Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        /// <param name="examinationRetrievalService">Examination Retrieval Service.</param>
        /// <param name="patientDetailsUpdateService">Patient Details Update Service.</param>
        /// <param name="locationParentsService">Location Parents Service.</param>
        public PatientDetailsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService,
            IAuthorizationService authorizationService, 
            IPermissionService permissionService, 
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination> patientDetailsUpdateService,
            IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> locationParentsService)
            : base(logger, mapper, usersRetrievalByEmailService, authorizationService, permissionService)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _patientDetailsUpdateService = patientDetailsUpdateService;
            _locationParentsService = locationParentsService;
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

            var examination =
                await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null));

            if (examination == null)
            {
                return NotFound(new GetPatientDetailsResponse());
            }

            if (!CanAsync(Permission.GetExamination, examination))
            {
                return Forbid();
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

            var examination =
                await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null));

            if (examination == null)
            {
                return NotFound(new PutPatientDetailsResponse());
            }

            if (!CanAsync(Permission.UpdateExamination, examination))
            {
                return Forbid();
            }

            var patientDetails = Mapper.Map<PatientDetails>(putPatientDetailsRequest);

            // TODO: Removed for now until we've decided what we want to happen if they change this and users are assigned.
            //if (patientDetails.MedicalExaminerOfficeResponsible != examination.MedicalExaminerOfficeResponsible)
            //{
            //    if (examination.MedicalTeam.MedicalExaminerUserId != null ||
            //        examination.MedicalTeam.MedicalExaminerOfficerUserId != null)
            //    {
            //        ModelState.AddModelError(nameof(PutPatientDetailsRequest.MedicalExaminerOfficeResponsible), "The medical examiner office is being changed while users are assigned. First un-assign users before changing the office.");
            //        return BadRequest(new PutPatientDetailsResponse());
            //    }
            //}

            var locations = (await _locationParentsService.Handle(
                    new LocationParentsQuery(patientDetails.MedicalExaminerOfficeResponsible))).ToList();

            var locationPath = new LocationPath();

            locationPath.UpdateLocationPath(locations);

            if (!CanAsync(Permission.UpdateExamination, locationPath))
            {
                return Forbid();
            }

            var myUser = await CurrentUser();

            var result = await _patientDetailsUpdateService.Handle(new PatientDetailsUpdateQuery(
                examinationId,
                patientDetails,
                myUser,
                locations));

            var patientCard = Mapper.Map<PatientCardItem>(result);

            return Ok(new PutPatientDetailsResponse
            {
                Header = patientCard
            });
        }
    }
}