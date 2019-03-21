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
        /// Returns all the data types and the values for mode of disposal.
        /// </summary>
        /// <returns>Dictionary of Modes of disposal</returns>
        [HttpGet("mode_of_disposal")]
        public ActionResult GetModesOfDisposal()
        {
            return Ok(GetDictionary(typeof(ModeOfDisposal)));
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
        [HttpGet("examination_gender")]
        public ActionResult GetExaminationGenders()
        {
            return Ok(GetDictionary(typeof(ExaminationGender)));
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