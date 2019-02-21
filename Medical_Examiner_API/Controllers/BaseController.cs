using AutoMapper;
using Medical_Examiner_API.Extensions.Models;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Examiner_API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Base Controller
    /// </summary>
    [ApiController]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Initialise a new instance of the Base Controller
        /// </summary>
        /// <param name="logger">The MELogger.</param>
        /// <param name="mapper">The Mapper.</param>
        protected BaseController(IMELogger logger, IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
        }

        /// <summary>
        /// Logger.
        /// </summary>
        public IMELogger Logger { get; set; }

        /// <summary>
        /// Mapper.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// Bad Request Response
        /// </summary>
        /// <remarks>Adds model errors from the model state to the default error list in the base response.</remarks>
        /// <typeparam name="TResponse">The response type, must be a subclass of <see cref="ResponseBase"/></typeparam>
        /// <param name="response">The response.</param>
        /// <returns>A bad request object result with the resposne set as the object.</returns>
        protected BadRequestObjectResult BadRequest<TResponse>(TResponse response)
            where TResponse : ResponseBase
        {
            response.AddModelErrors(ModelState);

            return new BadRequestObjectResult(response);
        }
    }
}
