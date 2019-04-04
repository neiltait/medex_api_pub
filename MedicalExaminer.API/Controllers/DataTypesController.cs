using System;
using System.Collections.Generic;
using AutoMapper;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <summary>
    /// Data Type Controller required for providing lists of types.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/data_types")]
    [ApiController]
    public class DataTypesController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypesController"/> class.
        /// </summary>
        /// <param name="logger">Instance of ILogger</param>
        /// <param name="mapper">Instance of IMapper</param>
        public DataTypesController(IMELogger logger, IMapper mapper)
            : base(logger, mapper)
        {
        }

        /// <summary>
        /// Returns all Analysis Entry Type types
        /// </summary>
        /// <returns>Dictionary of Analysis Entry Type</returns>
        [HttpGet("analysis_entry_type")]
        public ActionResult GetAnalysisEntryTypes()
        {
            return Ok(GetDictionary(typeof(AnalysisEntryType)));
        }

        /// <summary>
        /// Returns all Overall Circumstances Of Death.
        /// </summary>
        /// <returns>Dictionary of Overall Circumstances Of Death.</returns>
        [HttpGet("Overall_Circumstances_Of_Death")]
        public ActionResult GetOverallCircumstancesOfDeath()
        {
            return Ok(GetDictionary(typeof(OverallCircumstancesOfDeath)));
        }

        /// <summary>
        /// Returns all Overall Outcome Of Pre-Scrutiny.
        /// </summary>
        /// <returns>Dictionary of Overall Outcome Of Pre-Scrutiny.</returns>
        [HttpGet("Overall_Outcome_Of_Pre_Scrutiny")]
        public ActionResult GetOverallOutcomeOfPreScrutiny()
        {
            return Ok(GetDictionary(typeof(OverallOutcomeOfPreScrutiny)));
        }

        /// <summary>
        /// Returns all Clinical Governance Review.
        /// </summary>
        /// <returns>Dictionary of Clinical Governance Review.</returns>
        [HttpGet("Clinical_Governance_Review")]
        public ActionResult GetClinicalGovernanceReview()
        {
            return Ok(GetDictionary(typeof(ClinicalGovernanceReview)));
        }

        /// <summary>
        /// Returns all Coroner Statuses
        /// </summary>
        /// <returns>Dictionary of Coroner Statuses</returns>
        [HttpGet("coroner_status")]
        public ActionResult GetCoronerStatuses()
        {
            return Ok(GetDictionary(typeof(CoronerStatus)));
        }
        
        
        /// <summary>
        /// Returns all Examination Genders
        /// </summary>
        /// <returns>Dictionary of Examination Genders</returns>
        [HttpGet("examination_genders")]
        public ActionResult GetExaminationGenders()
        {
            return Ok(GetDictionary(typeof(ExaminationGender)));
        }
        
        /// <summary>
        /// Returns all location types
        /// </summary>
        /// <returns>List of values for location types</returns>
        [HttpGet("location_types")]
        public ActionResult GetLocationTypes()
        {
            return Ok(GetDictionary(typeof(LocationType)));
        }
   
        /// <summary>
        /// Returns all options for the present at death value for a bereaved .
        /// </summary>
        /// <returns>List of values for present at death</returns>
        [HttpGet("present_at_death")]
        public ActionResult GetPresentAtDeath()
        {
            return Ok(GetDictionary(typeof(PresentAtDeath)));
        }
            
        /// <summary>
        /// Returns all the data types and the values for mode of disposal.
        /// </summary>
        /// <returns>Dictionary of Modes of disposal</returns>
        [HttpGet("mode_of_disposal")]
        public ActionResult GetModesOfDisposal()
        {
            return Ok(GetDictionary(typeof(ModeOfDisposal)));
        }
        
        /// <summary>
        /// List of user roles
        /// </summary>
        /// <returns>List of user roles</returns>
        [HttpGet("user_roles")]
        public ActionResult GetUserRoles()
        {
            return Ok(GetDictionary(typeof(UserRoles)));
        }

        /// <summary>
        /// Returns all Event Type
        /// </summary>
        /// <returns>Dictionary of Event Types</returns>
        [HttpGet("event_type")]
        public ActionResult GetEventTypes()
        {
            return Ok(GetDictionary(typeof(EventType)));
        }


        /// Returns all Case Statuses
        /// </summary>
        /// <returns>Dictionary of Case Statuses</returns>
        [HttpGet("case_statuses")]
        public ActionResult GetCaseStatuses()
        {
            return Ok(GetDictionary(typeof(CaseStatus)));
        }

        private Dictionary<string, int> GetDictionary(Type enumeratorType)
        {
            var dic = new Dictionary<string, int>();

            var values = Enum.GetValues(enumeratorType);

            foreach (var value in values)
            {
                dic.Add(value.ToString(),(int)value);
            }

            return dic;
        }
    }
}