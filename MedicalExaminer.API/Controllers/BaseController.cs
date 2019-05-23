using AutoMapper;
using MedicalExaminer.API.Extensions.Models;
using MedicalExaminer.API.Models.v1;
using MedicalExaminer.Common.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Base Controller.
    /// </summary>
    /// <remarks>By default requires Authorization.</remarks>
    [ApiController]
    [Authorize]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="logger">The MELogger.</param>
        /// <param name="mapper">The Mapper.</param>
        protected BaseController(IMELogger logger, IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
        }

        /// <summary>
        ///     Logger.
        /// </summary>
        public IMELogger Logger { get; set; }

        /// <summary>
        ///     Mapper.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        ///     Bad Request Response.
        /// </summary>
        /// <remarks>Adds model errors from the model state to the default error list in the base response.</remarks>
        /// <typeparam name="TResponse">The response type, must be a subclass of <see cref="ResponseBase" />.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>A bad request object result with the response set as the object.</returns>
        protected BadRequestObjectResult BadRequest<TResponse>(TResponse response)
            where TResponse : ResponseBase
        {
            response.AddModelErrors(ModelState);

            return new BadRequestObjectResult(response);
        }
    }
}