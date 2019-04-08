using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Common.Extensions.Examination;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Permission = MedicalExaminer.Common.Authorization.Permission;

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
    public class ExaminationsController : AuthorizedBaseController
    {
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview> _examinationsDashboardService;
        private readonly IAsyncQueryHandler<CreateExaminationQuery, Examination> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> _examinationsRetrievalService;
        private readonly IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> _locationParentsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExaminationsController"/> class.
        /// </summary>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="usersRetrievalByEmailService">Users Retrieval By Email Service.</param>
        /// <param name="authorizationService">Authorization Service.</param>
        /// <param name="examinationCreationService">examinationCreationService.</param>
        /// <param name="examinationsRetrievalService">examinationsRetrievalService.</param>
        /// <param name="examinationsDashboardService">Examination Dashboard Service.</param>
        /// <param name="locationParentsService">Location Parents Service.</param>
        public ExaminationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService,
            IAuthorizationService authorizationService,
            IAsyncQueryHandler<CreateExaminationQuery, Examination> examinationCreationService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview> examinationsDashboardService,
            IAsyncQueryHandler<LocationParentsQuery, IEnumerable<Location>> locationParentsService)
            : base(logger, mapper, usersRetrievalByEmailService, authorizationService)
        {
            _examinationCreationService = examinationCreationService;
            _examinationsRetrievalService = examinationsRetrievalService;
            _examinationsDashboardService = examinationsDashboardService;
            _locationParentsService = locationParentsService;
        }

        /// <summary>
        /// Get All Examinations as a list of <see cref="ExaminationItem"/>.
        /// </summary>
        /// <param name="filter">Filter.</param>
        /// <returns>A list of examinations.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationsResponse>> GetExaminations([FromQuery]GetExaminationsRequest filter)
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
        ///     Create a new case.
        /// </summary>
        /// <param name="postExaminationRequest">The PostExaminationRequest.</param>
        /// <returns>A PostExaminationResponse.</returns>
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> CreateExamination(
            [FromBody] PostExaminationRequest postExaminationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutExaminationResponse());
            }

            try
            {
                var examination = Mapper.Map<Examination>(postExaminationRequest);

                var locations = await
                    _locationParentsService.Handle(
                        new LocationParentsQuery(examination.MedicalExaminerOfficeResponsible));

                examination.UpdateLocationCache(locations);

                if (!CanAsync(Permission.CreateExamination, examination))
                {
                    return Forbid();
                }

                var result = await _examinationCreationService.Handle(new CreateExaminationQuery(examination));
                var res = new PutExaminationResponse
                {
                    ExaminationId = result.ExaminationId
                };

                return Ok(res);
            }
            catch (DocumentClientException)
            {
                return NotFound(new PostExaminationRequest());
            }
            catch (ArgumentException)
            {
                return NotFound(new PostExaminationRequest());
            }
        }
    }
}
