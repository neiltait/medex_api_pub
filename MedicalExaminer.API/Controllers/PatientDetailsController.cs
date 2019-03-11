using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    [Route("patientdetails")]
    [ApiController]
    [Authorize]
    public class PatientDetailsController : BaseController
    {
        //private readonly IPatientDetailsPersistence _patientDetailsPersistence;

        public PatientDetailsController(IMELogger logger, IMapper mapper)
            : base(logger, mapper)
        {
           // _patientDetailsPersistence = patientDetailsPersistence;
        }

        [HttpPut]
        [Route("{caseId")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutPatientDetailsResponse>> CreateNewCase(string caseId, [FromBody]PutPatientDetailsRequest putPatientDetailsRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutPatientDetailsResponse());
            }

            //var examination = Mapper.Map<Examination>(postNewCaseRequest);

            //var result = _examinationCreationService.Handle(new CreateExaminationQuery(examination));
            //var res = new PutExaminationResponse()
            //{
            //    ExaminationId = result.Result
            //};

            return Ok();
        }

    }
}