﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Models.v1.CaseOutcome;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MedicalExaminer.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    public class CaseOutcomeController : AuthenticatedBaseController
    {
        private IAsyncQueryHandler<CoronerReferralQuery, string> _coronerReferralService;
        private IAsyncQueryHandler<CloseCaseQuery, string> _closeCaseService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string> _saveOutstandingCaseItems;

        public CaseOutcomeController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CoronerReferralQuery, string> coronerReferralService,
            IAsyncQueryHandler<CloseCaseQuery, string> closeCaseService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string> saveOutstandingCaseItems,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _coronerReferralService = coronerReferralService;
            _closeCaseService = closeCaseService;
            _examinationRetrievalService = examinationRetrievalService;
            _saveOutstandingCaseItems = saveOutstandingCaseItems;
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
        [Route("coroner_referral")]
        public async Task<ActionResult> PutCoronerReferral(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var user = await CurrentUser();
            var result = await _coronerReferralService.Handle(new CoronerReferralQuery(examinationId, user));

            return Ok();
        }

        [HttpPut]
        [Route("outstanding_case_items")]
        public async Task<ActionResult> PutOutstandingCaseItems(
            string examinationId,
            [FromBody]
            PutOutstandingCaseItemsRequest putOutstandingCaseItemsRequest)
        {
            // TODO:  Implement

            //  get examination to update
            var user = await CurrentUser();
            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, user));

            if (examination == null)
            {
                return NotFound();
            }

            //  map the putoutstandingcaseitemsrequest onto the examinationtoUpdate
            var request = Mapper.Map<PutOutstandingCaseItemsRequest>(examination);

            //  save the examination
            if (examination.CoronerReferralSent)
            {
                var result = await _closeCaseService.Handle(new CloseCaseQuery(examinationId, user));
                return Ok();
            }
            else
            {
                return BadRequest(); // for now
            }
        }

        [HttpPut]
        [Route("close_case")]
        public async Task<ActionResult> PutCloseCase(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            Guid examinationGuid;
            if (!Guid.TryParse(examinationId, out examinationGuid))
            {
                return new BadRequestObjectResult(nameof(examinationId));
            }

            var user = await CurrentUser();
            var result = await _closeCaseService.Handle(new CloseCaseQuery(examinationId, user));

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
