using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Medical_Examiner_API.Extensions.Data;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.v1.Examinations;
using Medical_Examiner_API.Models.V1.Examinations;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace Medical_Examiner_API.Controllers
{
    /// <summary>
    /// Examinations Controller
    /// </summary>
    [Route("api/examinations")]
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
        /// Get a list of <see cref="ExaminationItem"/>.
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
        /// Get an Examination by Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationResponse>> GetExamination(string id)
        {
            try
            {
                var examination = await _examinationPersistence.GetExaminationAsync(id);
                var response = Mapper.Map<GetExaminationResponse>(examination);
                return Ok(response);
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


            await _examinationPersistence.SaveExaminationAsync(ex1);
            await _examinationPersistence.SaveExaminationAsync(ex2);
            await _examinationPersistence.SaveExaminationAsync(ex3);

            var Examinations = await _examinationPersistence.GetExaminationsAsync();
            return Ok(Examinations);
        }
    }
}
