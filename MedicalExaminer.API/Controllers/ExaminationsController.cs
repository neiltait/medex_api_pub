using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.Validators;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
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
//    [Authorize]
    public class ExaminationsController : BaseController
    {
        /// <summary>
        /// The examination persistance layer.
        /// </summary>
        private readonly IExaminationPersistence _examinationPersistence;

        private readonly IValidator<ExaminationItem> _examinationValidator;
        /// <summary>
        /// Initialise a new instance of the Examiantions Controller.
        /// </summary>
        /// <param name="examinationPersistence">The Examination Persistance.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        public ExaminationsController(IExaminationPersistence examinationPersistence, IMELogger logger, IMapper mapper, IValidator<ExaminationItem> examinationValidator)
            : base(logger, mapper)
        {
            _examinationPersistence = examinationPersistence;
            _examinationValidator = examinationValidator;
        }

    /// <summary>
    /// Get All Examinations as a list of <see cref="ExaminationItem"/>.
    /// </summary>
    /// <returns>A list of examinations.</returns>
    [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationsResponse>> GetExaminations()
        {
            var examinations = await _examinationPersistence.GetExaminationsAsync();
            return Ok(new GetExaminationsResponse()
            {
                Examinations = examinations.Select(e => Mapper.Map<ExaminationItem>(e)).ToList(),
            });
        }

        /// <summary>
        /// Get Examination by ID
        /// </summary>
        /// <param name="examinationId">The Examination Id.</param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpGet("{examinationId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationResponse>> GetExamination(string examinationId)
        {
            try
            {
                var examination = await _examinationPersistence.GetExaminationAsync(examinationId);
                var response = Mapper.Map<GetExaminationResponse>(examination);
                return Ok(response);
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create a new case.
        /// </summary>
        /// <param name="postNewCaseRequest">The PostNewCaseRequest.</param>
        /// <returns>A PostNewCaseResponse.</returns>
        // POST api/examinations
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> CreateNewCase(PostNewCaseRequest postNewCaseRequest)
        {
            var examinationItem = Mapper.Map<ExaminationItem>(postNewCaseRequest);
            var validationResult = await _examinationValidator.ValidateAsync(examinationItem);
            var res = new PutExaminationResponse();
            if (validationResult.Any())
            {
                foreach (var validationError in validationResult)
                {
                    res.AddError(validationError.Property, $"{validationError.Code}: {validationError.Message}");
                }
            }
            else
            {
                //  save the examination
                res.ExaminationId = Guid.NewGuid().ToString();
            }

            return Ok(Mapper.Map<PutExaminationResponse>(res));
        }
    }
}