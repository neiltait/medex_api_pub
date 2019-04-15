using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    public class CaseOutcomeController : BaseAuthorizationController
    {
        public CaseOutcomeController(IMELogger logger, IMapper mapper, IAuthorizationService authorizationService) 
            : base(logger, mapper, authorizationService)
        {

        }


        [HttpPut]
        [Route("confirmation_of_scrutiny")]
        public ActionResult<PutConfirmationOfScrutinyResponse> PutConfirmationOfScrutiny()
        {
            // TODO:  Implement
            return Ok(new PutConfirmationOfScrutinyResponse()
            {
                ScrutinyConfirmedOn = DateTime.Now
            });
        }


        [HttpPut]
        [Route("outstanding_case_items")]
        public ActionResult PutOutstandingCaseItems([FromBody] PutOutstandingCaseItemsRequest request)
        {
            // TODO:  Implement
            return Ok();
        }

        [HttpPut]
        [Route("close_case")]
        public ActionResult PutCloseCase()
        {
            // TODO:  Implement
            return Ok();
        }

        [HttpGet]
        [Route("case_outcome")]
        public ActionResult<GetCaseOutcomeResponse> GetCaseOutcome()
        {
            // TODO:  Implement
            return Ok(new GetCaseOutcomeResponse());
        }
    }
}
