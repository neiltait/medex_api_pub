using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
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
    [Authorize]
    public class ExaminationsController : BaseController
    {
        /// <summary>
        /// The examination persistance layer.
        /// </summary>
        private readonly IExaminationPersistence _examinationPersistence;

        //private readonly IValidator<ExaminationItem> _examinationValidator;
        /// <summary>
        /// Initialise a new instance of the Examiantions Controller.
        /// </summary>
        /// <param name="examinationPersistence">The Examination Persistance.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        public ExaminationsController(IExaminationPersistence examinationPersistence, IMELogger logger, IMapper mapper)
            : base(logger, mapper)
        {
            _examinationPersistence = examinationPersistence;
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
        /// <param name="examinationId"></param>
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
        public async Task<ActionResult<PutExaminationResponse>> CreateNewCase([FromBody]PostNewCaseRequest postNewCaseRequest)
        {
            var examinationItem = Mapper.Map<ExaminationItem>(postNewCaseRequest);
    
            //var validationResult = await _examinationValidator.ValidateAsync(examinationItem);
            var res = new PutExaminationResponse();
            //if (validationResult.Any())
            //{
            //    res.AddValidationErrors(validationResult);
            //}
            //else
            //{

                var examination = Mapper.Map<Examination>(examinationItem);


                var documentId = await _examinationPersistence.CreateExaminationAsync(examination);

                res.ExaminationLink = new Link("examination", $"/examination/{documentId}");
                    //}

            return Ok(Mapper.Map<PutExaminationResponse>(res));
        }
    }
}