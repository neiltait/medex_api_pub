using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Examinations Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    [Authorize]
    public class MedicalTeamController : BaseController
    {
        private readonly IAsyncQueryHandler<CreateExaminationQuery, Examination> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>
            _examinationsRetrievalService;
        private readonly IAsyncUpdateDocumentHandler _medicaTeamUpdateService;
        
        public MedicalTeamController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateExaminationQuery, Examination> examinationCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService,
            IAsyncUpdateDocumentHandler medicaTeamUpdateService)
            : base(logger, mapper)
        {
            _examinationCreationService = examinationCreationService;
            _examinationRetrievalService = examinationRetrievalService;
            _examinationsRetrievalService = examinationsRetrievalService;
            _medicaTeamUpdateService = medicaTeamUpdateService;
        }
        
        /// <summary>
        ///     Get Medical Team
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

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
            if (examination == null || examination.MedicalTeam == null)
            {
                return NotFound(new GetMedicalTeamResponse());
            }

            var getMedicalTeamResponse = Mapper.Map<GetMedicalTeamResponse>(examination.MedicalTeam);

            return Ok(getMedicalTeamResponse);
        }
        
        /// <summary>
        ///     Put Medical Team
        /// </summary>
        /// ///
        /// <param name="examinationId">The ID of the examination on which the medical team is being updated.</param>
        /// <param name="putMedicalTeamRequest">The PutMedicalTeamRequest.</param>
        /// <returns>A PutExaminationResponse.</returns>
        [HttpPut("medical_team/")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutMedicalTeamResponse>> PutMedicalTeam(string examinationId,
            [FromBody]
            PutMedicalTeamRequest putMedicalTeamRequest)
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

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
            if (examination == null)
            {
                return NotFound();
            }

            examination.MedicalTeam = medicalTeamRequest;

            var returnedExaminationId = await _medicaTeamUpdateService.Handle(examination);

            if (returnedExaminationId == null)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var res = new PutMedicalTeamResponse()
            {
                ExaminationId = examinationId
            };

            return Ok(res);
        }        
    }
}