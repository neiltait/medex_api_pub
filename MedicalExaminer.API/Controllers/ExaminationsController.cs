using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
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
    /// Examinations Controller
    /// </summary>
    [Route("examinations")]
    [ApiController]
    [Authorize]
    public class ExaminationsController : BaseController
    {
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview> _examinationsDashboardService;
        private readonly IAsyncQueryHandler<CreateExaminationQuery, Examination> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> _examinationsRetrievalService;
		private readonly IAsyncUpdateDocumentHandler _medicaTeamUpdateService;
        

        /// <summary>
        /// Initialise a new instance of the Examiantions Controller.
        /// </summary>
        /// <param name="examinationPersistence">The Examination Persistance.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="examinationCreationService"></param>
        /// <param name="examinationRetrievalService"></param>
        /// <param name="examinationsRetrievalService"></param>
        /// <param name="medicalTeamUpdateService"></param>
        public ExaminationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateExaminationQuery, Examination> examinationCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService,
            IAsyncUpdateDocumentHandler medicaTeamUpdateService,
			IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview> examinationsDashboardService)
            : base(logger, mapper)
        {
            _examinationCreationService = examinationCreationService;
            _examinationRetrievalService = examinationRetrievalService;
            _examinationsRetrievalService = examinationsRetrievalService;
			_medicaTeamUpdateService = medicaTeamUpdateService;
            _examinationsDashboardService = examinationsDashboardService;
        }

    /// <summary>
    /// Get All Examinations as a list of <see cref="ExaminationItem"/>.
    /// </summary>
    /// <returns>A list of examinations.</returns>
    [HttpPost]
    [ServiceFilter(typeof(ControllerActionFilter))]
    public async Task<ActionResult<GetExaminationsResponse>> GetExaminations([FromBody]GetExaminationsRequest filter)
    {
        var examinationsQuery = new ExaminationsRetrievalQuery(filter.CaseStatus, filter.LocationId,
        filter.OrderBy, filter.PageNumber, filter.PageSize, filter.UserId, filter.OpenCases);
        var examinations = _examinationsRetrievalService.Handle(examinationsQuery);

        var dashboardOverview = _examinationsDashboardService.Handle(examinationsQuery);

        return Ok(new GetExaminationsResponse
        {
            CountOfTotalCases = dashboardOverview.Result.TotalCases,
            CountOfUrgentCases = dashboardOverview.Result.UrgentCases,
            CountOfCasesAdmissionNotesHaveBeenAdded = dashboardOverview.Result.AdmissionNotesHaveBeenAdded,
            CountOfCasesUnassigned = dashboardOverview.Result.Unassigned,
            CountOfCasesHaveBeenScrutinisedByME = dashboardOverview.Result.HaveBeenScrutinisedByME,
            CountOfCasesHaveFinalCaseOutstandingOutcomes = dashboardOverview.Result.HaveFinalCaseOutstandingOutcomes,
            CountOfCasesPendingAdmissionNotes = dashboardOverview.Result.PendingAdmissionNotes,
            CountOfCasesPendingDiscussionWithQAP = dashboardOverview.Result.PendingDiscussionWithQAP,
            CountOfCasesPendingDiscussionWithRepresentative = dashboardOverview.Result.PendingDiscussionWithRepresentative,
            CountOfCasesReadyForMEScrutiny = dashboardOverview.Result.ReadyForMEScrutiny,
            Examinations = examinations.Result.Select(e => Mapper.Map<PatientCardItem>(e)).ToList()
        });
    }

        /// <summary>
        /// Get Examination by ID
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpGet("{examinationId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationResponse>> GetExamination(string examinationId)
        {
                Examination result = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<GetExaminationResponse>(result));
        }

        /// <summary>
        /// Create a new case.
        /// </summary>
        /// <param name="postNewCaseRequest">The PostNewCaseRequest.</param>
        /// <returns>A PostNewCaseResponse.</returns>
        [HttpPost]
        [Route("new")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> CreateNewCase([FromBody]PostNewCaseRequest postNewCaseRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutExaminationResponse());
            }

            var examination = Mapper.Map<Examination>(postNewCaseRequest);
            var result =await _examinationCreationService.Handle(new CreateExaminationQuery(examination));
            var res = new PutExaminationResponse()
            {
                ExaminationId = result.id
            };

            return Ok(res);
        }

        /// <summary>
        /// Post Medical Team
        /// </summary>
        /// /// <param name="examinationId">The ID of the examination that the medical team object is to be posted to.</param>
        /// <param name="postMedicalTeamRequest">The PostMedicalTeamRequest.</param>
        /// <returns>A PutExaminationResponse.</returns>
        [HttpPost("/MedicalTeam/{examinationId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> PostMedicalTeam(string examinationId, [FromBody]PostMedicalTeamRequest postMedicalTeamRequest)
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

            Examination examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
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

            var res = new PutExaminationResponse()
            {
                ExaminationId = examinationId
            };

            return Ok(res);
        }

        [HttpGet("/MedicalTeam/{examinationId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetMedicalTeamResponse>> GetMedicalTeam(string examinationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetMedicalTeamResponse());
            }

            Examination examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
            if (examination == null || examination.MedicalTeam == null)
            {
                return NotFound(new GetMedicalTeamResponse());
            }

            var getMedicalTeamResponse = Mapper.Map<GetMedicalTeamResponse>(examination.MedicalTeam);

            return Ok(getMedicalTeamResponse);
        }
    }
}