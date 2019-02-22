using System.Collections.Generic;
using System.Threading.Tasks;
using Medical_Examiner_API.Enums;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Medical_Examiner_API.Extensions.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Medical_Examiner_API.Controllers
{
    [Route("datatype")]
    [ApiController]
    public class DataTypesController : BaseController
    {
        private readonly IExaminationPersistence _examinationPersistence;

        public DataTypesController(IExaminationPersistence examinationPersistence, IMELogger logger) : base(logger)
        {
            _examinationPersistence = examinationPersistence;
        }

        // GET datatype/mode_of_disposal
        [HttpGet("mode_of_disposal")]
        public async Task<ActionResult<IEnumerable<Examination>>> GetModesOfDisposal()
        {
            return Ok(EnumExtentions.GetDictionary(ModeOfDisposal));
        }

        // GET api/examinations
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