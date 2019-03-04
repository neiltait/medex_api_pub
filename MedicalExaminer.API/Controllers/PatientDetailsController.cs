using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.PatientDetails;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    public class PatientDetailsController : BaseController
    {
        private readonly IPatientDetailsPersistence _patientDetailsPersistence;

        public PatientDetailsController(IMELogger logger, IMapper mapper, IPatientDetailsPersistence patientDetailsPersistence)
            : base(logger, mapper)
        {
            _patientDetailsPersistence = patientDetailsPersistence;
        }

        [HttpPut("{examinationId")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult> PutPatientDetails(string examinationId, [FromBody] PutPatientDetailsRequest patientDetailsRequest)
        {
            //  TODO: need mapper
            //var patientDetails = Mapper.Map<PutPatientDetailsRequest, PatientDetails>();
            _patientDetailsPersistence.AddPatientDetails();
            return Ok();
        }
    }
}