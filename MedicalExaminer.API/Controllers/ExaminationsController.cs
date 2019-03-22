using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.MedicalTeams;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Examinations Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations")]
    [ApiController]
    [Authorize]
    public class ExaminationsController : BaseController
    {
        private readonly IAsyncQueryHandler<CreateExaminationQuery, string> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>
            _examinationsRetrievalService;

        private readonly IAsyncUpdateDocumentHandler _medicaTeamUpdateService;
        //private readonly IValidator<ExaminationItem> _examinationValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExaminationsController"/> class.
        /// </summary>
        /// <param name="examinationPersistence">The Examination Persistence.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="examinationCreationService">examinationCreationService.</param>
        /// <param name="examinationRetrievalService">examinationRetrievalService.</param>
        /// <param name="examinationsRetrievalService">examinationsRetrievalService.</param>
        /// <param name="medicalTeamUpdateService">medicalTeamUpdateService.</param>
        public ExaminationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateExaminationQuery, string> examinationCreationService,
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
        ///     Get All Examinations as a list of <see cref="ExaminationItem" />.
        /// </summary>
        /// <returns>A list of examinations.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationsResponse>> GetExaminations()
        {
            var examinations = await _examinationsRetrievalService.Handle(new ExaminationsRetrievalQuery());
            return Ok(new GetExaminationsResponse
            {
                Examinations = examinations.Select(e => Mapper.Map<Examination>(e)).ToList(),
            });
        }

        /// <summary>
        ///     Get Examination by ID
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpGet("{examinationId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationResponse>> GetExamination(string examinationId)
        {
            var result = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
            if (result == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<GetExaminationResponse>(result));
        }

        /// <summary>
        ///     Create a new case.
        /// </summary>
        /// <param name="postNewCaseRequest">The PostNewCaseRequest.</param>
        /// <returns>A PostNewCaseResponse.</returns>
        // POST api/examinations
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> CreateNewCase(
            [FromBody] PostNewCaseRequest postNewCaseRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutExaminationResponse());
            }

            try
            {
                var examination = Mapper.Map<Examination>(postNewCaseRequest);
                var result = await _examinationCreationService.Handle(new CreateExaminationQuery(examination));
                var res = new PutExaminationResponse
                {
                    ExaminationId = result
                };

                return Ok(res);
            }
            catch (DocumentClientException)
            {
                return NotFound(new PostUserResponse());
            }
            catch (ArgumentException)
            {
                return NotFound(new PostUserResponse());
            }
        }

        /// <summary>
        ///     Post Medical Team.
        /// </summary>
        /// ///
        /// <param name="examinationId">The ID of the examination that the medical team object is to be posted to.</param>
        /// <param name="postMedicalTeamRequest">The PostMedicalTeamRequest.</param>
        /// <returns>A PutExaminationResponse.</returns>
        [HttpPost("{examinationId}/medical_team/")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> PostMedicalTeam(
            string examinationId,
            [FromBody] PostMedicalTeamRequest postMedicalTeamRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutExaminationResponse());
            }

            var medicalTeamRequest = Mapper.Map<MedicalTeam>(postMedicalTeamRequest);

            if (medicalTeamRequest == null)
            {
                return BadRequest(new PutExaminationResponse());
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
                return BadRequest(new PutExaminationResponse());
            }

            var res = new PutExaminationResponse
            {
                ExaminationId = examinationId
            };

            return Ok(res);
        }

        [HttpGet("{examinationId}/medical_team/")]
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
    }
}