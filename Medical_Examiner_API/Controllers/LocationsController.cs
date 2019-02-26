using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Models.V1.Locations;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace Medical_Examiner_API.Controllers
{
    public class LocationsController : BaseController
    {
        /// <summary>
        /// The location persistance layer.
        /// </summary>
        private readonly ILocationPersistence _locationPersistence;


        /// <summary>
        /// Initialise a new instance of the Loctions Controller.
        /// </summary>
        /// <param name="locationPersistence">The Location Persistance.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        public LocationsController(ILocationPersistence locationPersistence, IMELogger logger, IMapper mapper)
            : base(logger, mapper)
        {
            _locationPersistence = locationPersistence;
        }

        /// <summary>
        /// Get All Locations as a list of <see cref="LocationItem"/>.
        /// </summary>
        /// <returns>A list of location.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetLocationResponse>> GetLocations()
        {
            var locations = await _locationPersistence.GetLocationsAsync();
            return Ok(new GetLocationsResponse()
            {
                Locations = locations.Select(e => Mapper.Map<LocationItem>(e)).ToList(),
            });
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}