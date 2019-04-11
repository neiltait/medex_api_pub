using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Extensions.MeUser;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Medical Team Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    [Authorize]
    public class MedicalTeamController : BaseController
    {
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private readonly IAsyncUpdateDocumentHandler _medicalTeamUpdateService;

        private readonly IAsyncQueryHandler<UsersRetrievalByRoleLocationQuery, IEnumerable<MeUser>>
            _usersRetrievalByRoleLocationQueryService;

        /// <summary>
        /// Initialise a new instance of the <see cref="MedicalTeamController"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="examinationRetrievalService">Examination Retrieval Service.</param>
        /// <param name="medicalTeamUpdateService">Medical Team Update Service.</param>
        public MedicalTeamController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncUpdateDocumentHandler medicalTeamUpdateService,
            IAsyncQueryHandler<UsersRetrievalByRoleLocationQuery, IEnumerable<MeUser>>
                usersRetrievalByRoleLocationQueryService)
            : base(logger, mapper)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _medicalTeamUpdateService = medicalTeamUpdateService;
            _usersRetrievalByRoleLocationQueryService = usersRetrievalByRoleLocationQueryService;
        }

        /// <summary>
        ///     Get Medical Team.
        /// </summary>
        /// ///
        /// <param name="examinationId">The ID of the examination.</param>
        /// <returns>A GetMedicalTeamResponse.</returns>
        [HttpGet("medical_team/")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetMedicalTeamResponse>> GetMedicalTeam(string examinationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetMedicalTeamResponse());
            }

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null));

            var getMedicalTeamResponse = examination?.MedicalTeam != null
                ? Mapper.Map<GetMedicalTeamResponse>(examination.MedicalTeam)
                : new GetMedicalTeamResponse
                {
                    ConsultantsOther = new Models.v1.MedicalTeams.ClinicalProfessionalItem[] { },
                    NursingTeamInformation = string.Empty,
                };

            getMedicalTeamResponse.AddLookup("medicalExaminers", await GetLookupFor(examination, UserRoles.MedicalExaminer));
            getMedicalTeamResponse.AddLookup("medicalExaminerOfficers", await GetLookupFor(examination, UserRoles.MedicalExaminerOfficer));

            return Ok(getMedicalTeamResponse);
        }

        private async Task<IDictionary<string, string>> GetLookupFor(Examination examination, UserRoles role)
        {
            var users = await GetUsersFor(examination, role);

            return users.ToDictionary(u => u.UserId, u => u.FullName());
        }

        private async Task<IEnumerable<MeUser>> GetUsersFor(Examination examination, UserRoles role)
        {
            // get all users that have that role on this examination

            // get all the locations from the examination
            var locations = examination.LocationIds();

            // get all users that have a permission in those locations where the role matches
            var users = await _usersRetrievalByRoleLocationQueryService.Handle(new UsersRetrievalByRoleLocationQuery(locations, role));

            return users;
        }

        /// <summary>
        ///     Put Medical Team.
        /// </summary>
        /// ///
        /// <param name="examinationId">The ID of the examination on which the medical team is being updated.</param>
        /// <param name="putMedicalTeamRequest">The PutMedicalTeamRequest.</param>
        /// <returns>A PutExaminationResponse.</returns>
        [HttpPut("medical_team/")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutMedicalTeamResponse>> PutMedicalTeam(string examinationId, [FromBody] PutMedicalTeamRequest putMedicalTeamRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var medicalTeamRequest = Mapper.Map<MedicalTeam>(putMedicalTeamRequest);

            if (medicalTeamRequest == null)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null));
            if (examination == null)
            {
                return NotFound();
            }

            examination.MedicalTeam = medicalTeamRequest;

            var returnedExamination = await _medicalTeamUpdateService.Handle(examination);

            if (returnedExamination == null)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var response = Mapper.Map<PutMedicalTeamResponse>(returnedExamination.MedicalTeam);

            return Ok(response);
        }
    }
}