using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Medical_Examiner_API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("examinations")]
    [ApiController]
    public class ExaminationsController : BaseController
    {
        private readonly IExaminationPersistence _examinationPersistence;

        public ExaminationsController(IExaminationPersistence examinationPersistence, IMELogger logger)
            : base(logger)
        {
            _examinationPersistence = examinationPersistence;
        }

        // GET api/examinations
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<IEnumerable<Examination>>> GetExaminations()
        {
            var Examinations = await _examinationPersistence.GetExaminationsAsync();
            return Ok(Examinations);
        }

        // GET api/examinations
        /// <summary>
        /// 
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<Examination>> GetExamination(string examinationId)
        {
            try
            {
                return Ok(await _examinationPersistence.GetExaminationAsync(examinationId));
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }
    }
}