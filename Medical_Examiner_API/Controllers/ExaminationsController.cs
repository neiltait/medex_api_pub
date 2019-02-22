using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models.V1.Examinations;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace Medical_Examiner_API.Controllers
{
    /// <summary>
    /// Examinations Controller
    /// </summary>
    [Route("examinations")]
    [ApiController]
    public class ExaminationsController : BaseController
    {
        /// <summary>
        /// The examination persistance layer.
        /// </summary>
        private readonly IExaminationPersistence _examinationPersistence;

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

        // GET api/examinations
        /// <summary>
        /// Get Examination by ID
        /// </summary>
        /// <param name="examinationId">The Examination Id.</param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpGet("{id}")]
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
    }
}