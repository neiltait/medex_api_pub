using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Models.Enums;
using MedicalExaminer.Models.V1.Locations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Locations Controller
    /// </summary>
    [Route("locations")]
    [ApiController]
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
        /// Get all Locations as a list of <see cref="LocationItem"/>.
        /// </summary>
        /// <returns>A list of location.</returns>
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetLocationsResponse>> GetLocations()
        {
            var locations = await _locationPersistence.GetLocationsAsync();
            return Ok(new GetLocationsResponse()
            {
                Locations = locations.Select(e => Mapper.Map<LocationItem>(e)).ToList(),
            });
        }

        /// <summary>
        /// Get Location by ID
        /// </summary>
        /// <param name="locationId">The Location Id.</param>
        /// <returns>A GetLocationsResponse.</returns>
        [HttpGet("/id/{locationId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetLocationResponse>> GetLocation(string locationId)
        {
            try
            {
                var location = await _locationPersistence.GetLocationAsync(locationId);
                var response = Mapper.Map<GetLocationResponse>(location);
                return Ok(response);
            }
            catch (DocumentClientException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get Locations as a list of <see cref="LocationItem"/> where location name contains locationName.
        /// </summary>
        /// <param name="locationName">The value to be used in the selection of matching names.</param>
        /// <returns>A list of locations whose names contain locationName.</returns>
        [HttpGet("/name/{locationName}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetLocationsResponse>> GetLocationsByName(string locationName)
        {
            var locations = await _locationPersistence.GetLocationsByNameAsync(locationName);
            return Ok(new GetLocationsResponse()
            {
                Locations = locations.Select(location => Mapper.Map<LocationItem>(location)).ToList(),
            });
        }

        /// <summary>
        /// Get Locations as a list of <see cref="LocationItem"/> where locations are under the location whose locationId = parentId
        /// </summary>
        /// <param name="parentId">The locationId of the location whose children are to be returned as list</param>
        /// <returns>list of locations that are under the location whose location = parentId</returns>
        [HttpGet("/parentId/{parentId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetLocationsResponse>> GetLocationsByParentId(string parentId)
        {
            var locations = await _locationPersistence.GetLocationsByParentIdAsync(parentId);
            return Ok(new GetLocationsResponse()
            {
                Locations = locations.Select(location => Mapper.Map<LocationItem>(location)).ToList(),
            });
        }
    }
}