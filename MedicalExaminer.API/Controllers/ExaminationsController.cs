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
    /// <summary>
    ///     Examinations Controller.
    /// </summary>
    /// <inheritdoc />
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations")]
    [ApiController]
    [Authorize]
    public class ExaminationsController : BaseController
    {
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview> _examinationsDashboardService;
        private readonly IAsyncQueryHandler<CreateExaminationQuery, Examination> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> _examinationsRetrievalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExaminationsController"/> class.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="examinationCreationService">examinationCreationService.</param>
        /// <param name="examinationRetrievalService">examinationRetrievalService.</param>
        /// <param name="examinationsRetrievalService">examinationsRetrievalService.</param>
        /// <param name="examinationsDashboardService">Examination Dashboard Service.</param>
        public ExaminationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateExaminationQuery, Examination> examinationCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview> examinationsDashboardService)
            : base(logger, mapper)
        {
            _examinationCreationService = examinationCreationService;
            _examinationRetrievalService = examinationRetrievalService;
            _examinationsRetrievalService = examinationsRetrievalService;
            _examinationsDashboardService = examinationsDashboardService;
        }

        /// <summary>
        /// Get All Examinations as a list of <see cref="ExaminationItem"/>.
        /// </summary>
        /// <param name="filter">Filter.</param>
        /// <returns>A list of examinations.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationsResponse>> GetExaminations([FromBody] GetExaminationsRequest filter)
        {
            if (filter == null)
            {
                return BadRequest(new GetExaminationsResponse());
            }

            var examinationsQuery = new ExaminationsRetrievalQuery(
                filter.CaseStatus,
                filter.LocationId,
                filter.OrderBy,
                filter.PageNumber,
                filter.PageSize,
                filter.UserId,
                filter.OpenCases);
            var examinations = _examinationsRetrievalService.Handle(examinationsQuery);

            var dashboardOverview = await _examinationsDashboardService.Handle(examinationsQuery);

            return Ok(new GetExaminationsResponse
            {
                CountOfTotalCases = dashboardOverview.TotalCases,
                CountOfUrgentCases = dashboardOverview.CountOfUrgentCases,
                CountOfCasesAdmissionNotesHaveBeenAdded = dashboardOverview.CountOfAdmissionNotesHaveBeenAdded,
                CountOfCasesUnassigned = dashboardOverview.CountOfUnassigned,
                CountOfCasesHaveBeenScrutinisedByME = dashboardOverview.CountOfHaveBeenScrutinisedByME,
                CountOfCasesHaveFinalCaseOutstandingOutcomes = dashboardOverview.CountOfHaveFinalCaseOutstandingOutcomes,
                CountOfCasesPendingAdmissionNotes = dashboardOverview.CountOfPendingAdmissionNotes,
                CountOfCasesPendingDiscussionWithQAP = dashboardOverview.CountOfPendingDiscussionWithQAP,
                CountOfCasesPendingDiscussionWithRepresentative = dashboardOverview.CountOfPendingDiscussionWithRepresentative,
                CountOfCasesReadyForMEScrutiny = dashboardOverview.CountOfReadyForMEScrutiny,
                Examinations = examinations.Result.Select(e => Mapper.Map<PatientCardItem>(e)).ToList()
            });
        }

        /// <summary>
        ///     Get Examination by ID.
        /// </summary>
        /// <param name="examinationId">Examination Id.</param>
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
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> CreateNewCase(
            [FromBody]
            PostNewCaseRequest postNewCaseRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutExaminationResponse());
            }

            var examination = Mapper.Map<Examination>(postNewCaseRequest);
            var result = await _examinationCreationService.Handle(new CreateExaminationQuery(examination));
            var res = new PutExaminationResponse
            {
                ExaminationId = result.ExaminationId
            };

            return Ok(res);
        }
    }
}