using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        /// <summary>
        /// Data Types Controller initialiser 
        /// </summary>
        /// <param name="logger"></param>
        public DataTypesController(IMELogger logger, IMapper mapper)
            : base(logger, mapper)
        {
        }

        /// <summary>
        /// Returns all the data types and the values for mode of disposal 
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