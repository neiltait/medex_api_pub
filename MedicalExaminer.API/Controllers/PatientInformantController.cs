using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.PatientInformants;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Controllers
{
    public class PatientInformantController : BaseController
    {
        private readonly IInformantPersistence _informantPersistence;
        public PatientInformantController(IMELogger logger, IMapper mapper, IInformantPersistence informantPersistence) : base(logger, mapper)
        {
            _informantPersistence = informantPersistence;
        }

        /// <summary>
        /// Get all case informants by case ID
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns></returns>
        [HttpGet("/{examinationId}/Informants")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<object>> Informants(string examinationId)
        {
            try
            {
                var informants = await _informantPersistence.GetInformants(examinationId);
                var response = Mapper.Map<GetInformantsResponse>(informants);
                return Ok(response);
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get a case informant by informant ID
        /// </summary>
        /// <param name="examinationId"></param>
        /// <param name="informantId"></param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpGet("/{examinationId}/Informant/{informantId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<object>> Informant(string examinationId, string informantId)
        {
            try
            {
                var informants = await _informantPersistence.GetInformant(examinationId, informantId);
                var response = Mapper.Map<GetInformantsResponse>(informants);
                return Ok(response);
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Delete a case informant by informant id
        /// </summary>
        /// <param name="examinationId"></param>
        /// <param name="informantId"></param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpDelete("/{examinationId}/Informant/{informantId}")]
        [ActionName("Informant")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult> Informant_Delete(string examinationId, string informantId)
        {
            try
            {
                var informants = await _informantPersistence.DeleteInformant(examinationId, informantId);
                var response = Mapper.Map<GetInformantsResponse>(informants);
                return Ok(response);
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Add new informant details to a case
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpPost("/{examinationId}/Informant")]
        [ActionName("Informant")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult> Informant_Post(string examinationId)
        {
            try
            {
                var informants = await _informantPersistence.AddInformant(examinationId);
                var response = Mapper.Map<GetInformantsResponse>(informants);
                return Ok(response);
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update informant details
        /// </summary>
        /// <param name="examinationId"></param>
        /// <param name="informantId"></param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpPut("/{examinationId}/Informant/{informantId}")]
        [ActionName("Informant")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult> Informant_Put(string examinationId, string informantId)
        {
            try
            {
                var informants = await _informantPersistence.UpdateInformant(examinationId, informantId);
                var response = Mapper.Map<GetInformantsResponse>(informants);
                return Ok(response);
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }
    }
}