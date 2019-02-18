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
using Medical_Examiner_API.Loggers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Medical_Examiner_API.Controllers
{
    [Route("api/examinations")]
    [ApiController]
    public class ExaminationsController : BaseController
    {
        public DocumentClient client = null;
        private IExaminationPersistence _examination_persistence;

        public ExaminationsController(IExaminationPersistence examination_persistence, IMELogger logger): base(logger)
        {
            _examination_persistence = examination_persistence;
        }

        // GET api/values
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<IEnumerable<Examination>>> GetExaminations()
        {
            var Examinations = await _examination_persistence.GetExaminationsAsync();
            return Ok(Examinations);
        }

        // GET api/values
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

        // GET api/values/seed
        [HttpGet("seed")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<IEnumerable<Examination>>> Seed()
        {
            Examination ex1 = new Examination();
            Examination ex2 = new Examination();
            Examination ex3 = new Examination();
            ex1.DateOfBirth = new DateTime(2010, 2, 1);
            ex2.DateOfBirth = new DateTime(1998, 6, 23);
            ex3.DateOfBirth = new DateTime(1964, 1, 30);

            ex1.DateOfDeath = new DateTime(2012, 3, 4, 13, 22, 00);
            ex2.DateOfBirth = new DateTime(2017, 4, 17, 23, 1, 00);
            ex3.DateOfBirth = new DateTime(2018, 11, 29, 15, 15, 00);

            ex1.Completed = false;
            ex2.Completed = false;
            ex3.Completed = false;

            ex1.FullName = "Robert Bobert";
            ex2.FullName = "Louise Cheese";
            ex3.FullName = "Crowbar Jones";

            ex1.CreatedAt = DateTime.Now;
            ex2.CreatedAt = DateTime.Now;
            ex3.CreatedAt = DateTime.Now;

            ex1.ModifiedAt = DateTime.Now;
            ex2.ModifiedAt = DateTime.Now;
            ex3.ModifiedAt = DateTime.Now;

            ex1.DeletedAt = null;


            await _examination_persistence.SaveExaminationAsync(ex1);
            await _examination_persistence.SaveExaminationAsync(ex2);
            await _examination_persistence.SaveExaminationAsync(ex3);

            var Examinations = await _examination_persistence.GetExaminationsAsync();
            return Ok(Examinations);
        }
    }
}
