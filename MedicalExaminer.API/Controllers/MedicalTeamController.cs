using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1;
using MedicalExaminer.API.Models.v1.MedicalTeams;
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
        /// <summary>
        /// Medical Examiners Lookup Key.
        /// </summary>
        public const string MedicalExaminersLookupKey = "medicalExaminers";

        /// <summary>
        /// Medical Examiner Officers Lookup Key.
        /// </summary>
        public const string MedicalExaminerOfficersLookupKey = "medicalExaminerOfficers";

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
        /// <param name="usersRetrievalByRoleLocationQueryService">Users Retrieval by Role Location Query Service.</param>
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
                    ConsultantsOther = new ClinicalProfessionalItem[] { },
                    NursingTeamInformation = string.Empty,
                };

            getMedicalTeamResponse
                .AddLookup(MedicalExaminersLookupKey, await GetLookupForExamination(examination, UserRoles.MedicalExaminer))
                .AddLookup(MedicalExaminerOfficersLookupKey, await GetLookupForExamination(examination, UserRoles.MedicalExaminerOfficer));

            return Ok(getMedicalTeamResponse);
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

        /// <summary>
        /// Get Lookup for Examination.
        /// </summary>
        /// <param name="examination">Examination.</param>
        /// <param name="role">Role.</param>
        /// <returns>A Lookup.</returns>
        private async Task<IDictionary<string, string>> GetLookupForExamination(Examination examination, UserRoles role)
        {
            if (examination == null)
            {
                return null;
            }

            var users = await GetUsersForExamination(examination, role);

            return users.ToDictionary(
                u => u.UserId,
                u => u.FullName());
        }

        /// <summary>
        /// Get Users For Examination
        /// </summary>
        /// <param name="examination">Examination.</param>
        /// <param name="role">Role.</param>
        /// <returns>List of Users.</returns>
        private async Task<IEnumerable<MeUser>> GetUsersForExamination(Examination examination, UserRoles role)
        {
            var locations = examination.LocationIds();

            // TODO: Should we care about max results?
            var users = await _usersRetrievalByRoleLocationQueryService.Handle(new UsersRetrievalByRoleLocationQuery(locations, role));

            return users;
        }

    }
}