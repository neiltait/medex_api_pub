using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.API.Services;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission = MedicalExaminer.Common.Authorization.Permission;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    ///     Medical Team Controller.
    /// </summary>
    /// <inheritdoc />
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    [Authorize]
    public class MedicalTeamController : AuthorizedBaseController
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
        private readonly IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> _UsersRetrievalByOktaIdService;

        private readonly IAsyncQueryHandler<UsersRetrievalByRoleLocationQuery, IEnumerable<MeUser>>
            _usersRetrievalByRoleLocationQueryService;

        /// <summary>
        /// Initialise a new instance of the <see cref="MedicalTeamController"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="usersRetrievalByOktaIdService">User Retrieval By Okta Id Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        /// <param name="examinationRetrievalService">Examination Retrieval Service.</param>
        /// <param name="medicalTeamUpdateService">Medical Team Update Service.</param>
        /// <param name="usersRetrievalByRoleLocationQueryService">Users Retrieval by Role Location Query Service.</param>
        public MedicalTeamController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByOktaIdQuery, MeUser> usersRetrievalByOktaIdService,
            IAuthorizationService authorizationService,
            IPermissionService permissionService,
            IAsyncQueryHandler<ExaminationRetrievalQuery,Examination> examinationRetrievalService,
            IAsyncUpdateDocumentHandler medicalTeamUpdateService,
            IAsyncQueryHandler<UsersRetrievalByRoleLocationQuery, IEnumerable<MeUser>> usersRetrievalByRoleLocationQueryService)
            : base(logger, mapper, usersRetrievalByOktaIdService, authorizationService, permissionService)
        {
            _examinationRetrievalService = examinationRetrievalService;
            _medicalTeamUpdateService = medicalTeamUpdateService;
            _usersRetrievalByRoleLocationQueryService = usersRetrievalByRoleLocationQueryService;
            _UsersRetrievalByOktaIdService = usersRetrievalByOktaIdService;
        }

        /// <summary>
        ///     Get Medical Team.
        /// </summary>
        /// ///
        /// <param name="examinationId">The ID of the examination.</param>
        /// <returns>A GetMedicalTeamResponse.</returns>
        [HttpGet("medical_team/")]
        public async Task<ActionResult<GetMedicalTeamResponse>> GetMedicalTeam(string examinationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetMedicalTeamResponse());
            }

            var examination = _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null)).Result;

            if (examination == null)
            {
                return NotFound(new GetMedicalTeamResponse());
            }

            if (!CanAsync(Permission.GetExamination, examination))
            {
                return Forbid();
            }

            var getMedicalTeamResponse = examination.MedicalTeam != null
                ? Mapper.Map<GetMedicalTeamResponse>(examination)
                : new GetMedicalTeamResponse
                {
                    ConsultantsOther = new ClinicalProfessionalItem[] { },
                    NursingTeamInformation = string.Empty,
                };

            getMedicalTeamResponse
                .AddLookup(
                    MedicalExaminersLookupKey,
                    await GetLookupForExamination(examination, UserRoles.MedicalExaminer))
                .AddLookup(
                    MedicalExaminerOfficersLookupKey,
                    await GetLookupForExamination(examination, UserRoles.MedicalExaminerOfficer));

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
        public async Task<ActionResult<PutMedicalTeamResponse>> PutMedicalTeam(
            string examinationId,
            [FromBody] [ModelBinder(Name = "examinationId")] PutMedicalTeamRequest putMedicalTeamRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var medicalTeamRequest = Mapper.Map<MedicalTeam>(putMedicalTeamRequest);

            var myUser = await CurrentUser();
            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, null));

            if (examination == null)
            {
                return NotFound();
            }

            if (!CanAsync(Permission.UpdateExamination, examination))
            {
                return Forbid();
            }

            examination.MedicalTeam = medicalTeamRequest;

            var returnedExamination = await _medicalTeamUpdateService.Handle(examination, myUser.UserId);

            if (returnedExamination == null)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var response = Mapper.Map<PutMedicalTeamResponse>(returnedExamination);

            return Ok(response);
        }

        /// <summary>
        /// Get Lookup for Examination.
        /// </summary>
        /// <param name="examination">Examination.</param>
        /// <param name="role">Role.</param>
        /// <returns>A Lookup.</returns>
        private async Task<IEnumerable<object>> GetLookupForExamination(Examination examination, UserRoles role)
        {
            var users = await GetUsersForExamination(examination, role);

            return Mapper.Map<IEnumerable<MeUser>, IEnumerable<UserLookup>>(users);
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

            var users = await _usersRetrievalByRoleLocationQueryService.Handle(new UsersRetrievalByRoleLocationQuery(locations, new[] { role }));

            return users;
        }
    }
}