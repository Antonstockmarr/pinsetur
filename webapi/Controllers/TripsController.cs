using Microsoft.AspNetCore.Mvc;
using stockmarrdk_api.Common;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Services;

namespace stockmarrdk_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet()]
        public IActionResult GetTrips()
        {
            List<Trip> trips = _tripService.GetAllTrips();

            return StatusCode(StatusCodes.Status200OK, trips);
        }

        [HttpGet("{year}")]
        public IActionResult GetTrip(int year)
        {
            Trip? trip = _tripService.GetTrip(year);

            if (trip is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Trip from year {year} was not found");
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, trip);
            }
        }

        [HttpPost]
        public IActionResult PostTrip([FromForm] Trip trip)
        {
            try
            {
                _tripService.PostTrip(trip);
            }
            catch (AlreadyExistsException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPatch]
        public IActionResult PatchTrip([FromForm] Trip trip)
        {
            try
            {
                Trip? updatedTrip = _tripService.PatchTrip(trip);
                if (updatedTrip == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, updatedTrip);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
