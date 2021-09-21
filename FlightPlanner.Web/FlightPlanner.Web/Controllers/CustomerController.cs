using Microsoft.AspNetCore.Mvc;
using FlightPlanner.Web.Models;
using FlightPlanner.Web.Storage;

namespace FlightPlanner.Web.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports(string search)
        {
            var airportArray = FlightStorage.SearchAirportByKeyword(search);
            if (airportArray.Count > 0)
                return Ok(airportArray);
            return Ok();
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult PostSearchFlights(SearchFlightRequest searchFlightRequest)
        {
            if (searchFlightRequest.From == searchFlightRequest.To)
                return BadRequest();
            return Ok(FlightStorage.SearchFlight(searchFlightRequest));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetById(id);
            if (flight is null)
                return NotFound();
            return Ok(flight);
        }
    }
}