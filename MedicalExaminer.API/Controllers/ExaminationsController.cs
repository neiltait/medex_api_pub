using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries;
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
    /// Examinations Controller
    /// </summary>
    [Route("examinations")]
    [ApiController]
    [Authorize]
    public class ExaminationsController : BaseController
    {
        private readonly IAsyncQueryHandler<CreateExaminationQuery, string> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationRetrivalQuery, Examination> _examinationRetrivalService;
        private readonly IAsyncQueryHandler<ExaminationsRetrivalQuery, IEnumerable<Examination>> _examinationsRetrivalService;
        //private readonly IValidator<ExaminationItem> _examinationValidator;
        /// <summary>
        /// Initialise a new instance of the Examiantions Controller.
        /// </summary>
        /// <param name="examinationPersistence">The Examination Persistance.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="examinationCreationService"></param>
        /// <param name="examinationRetrivalService"></param>
        /// <param name="examinationsRetrivalService"></param>
        public ExaminationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateExaminationQuery, string> examinationCreationService,
            IAsyncQueryHandler<ExaminationRetrivalQuery, Examination> examinationRetrivalService,
            IAsyncQueryHandler<ExaminationsRetrivalQuery, IEnumerable<Examination>> examinationsRetrivalService)
            : base(logger, mapper)
        {
            _examinationCreationService = examinationCreationService;
            _examinationRetrivalService = examinationRetrivalService;
            _examinationsRetrivalService = examinationsRetrivalService;
        }

    /// <summary>
    /// Get All Examinations as a list of <see cref="ExaminationItem"/>.
    /// </summary>
    /// <returns>A list of examinations.</returns>
    [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationsResponse>> GetExaminations()
        {
            var ex = await _examinationsRetrivalService.Handle(new ExaminationsRetrivalQuery());
            return Ok(new GetExaminationsResponse()
            {
                Examinations = ex.Select(e => Mapper.Map<ExaminationItem>(e)).ToList()
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
                var result = await _examinationRetrivalService.Handle(new ExaminationRetrivalQuery(examinationId));
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
        // POST api/examinations
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> CreateNewCase([FromBody]PostNewCaseRequest postNewCaseRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutExaminationResponse());
            }

            var examination = Mapper.Map<Examination>(postNewCaseRequest);
            
            var result = _examinationCreationService.Handle(new CreateExaminationQuery(examination));
            var res = new PutExaminationResponse()
            {
                ExaminationId = result.Result
            };

            return Ok(res);
        }
    }
}