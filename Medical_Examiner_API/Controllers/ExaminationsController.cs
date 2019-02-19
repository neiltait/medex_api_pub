using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Medical_Examiner_API;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Medical_Examiner_API.Controllers
{
    [Route("api/examinations")]
    [ApiController]
    public class ExaminationsController : Controller
    {
        public DocumentClient client = null;
        private IExaminationPersistence _examination_persistence;

        public ExaminationsController(IExaminationPersistence examination_persistence)
        {
            _examination_persistence = examination_persistence;
        }

        // GET api/examinations
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<IEnumerable<Examination>>> GetExaminations()
        {
            var Examinations = await _examination_persistence.GetExaminationsAsync();
            return Ok(Examinations);
        }

        // GET api/examinations
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<Examination>> GetExamination(string id)
        {
            try
            {
                return Ok(await _examination_persistence.GetExaminationAsync(id));
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }
    }
}
