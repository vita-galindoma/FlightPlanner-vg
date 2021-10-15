using System.Linq;
using FlightPlanner.Web.DbContext;
using Microsoft.AspNetCore.Mvc;
using FlightPlanner.Web.Models;
using FlightPlanner.Web.Storage;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Web.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly FlightPlannerDbContext _context;

        public CustomerController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports(string search)
        {
            var airportArray = FlightStorage.SearchAirportByKeyword(search, _context);
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
            return Ok(FlightStorage.SearchFlight(searchFlightRequest, _context));
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _context.Flights
                .Include(f => f.To)
                .Include(f => f.From)
                .SingleOrDefault(f => f.Id == id);

            if (flight is null)
                return NotFound();
            return Ok(flight);
        }
    }
}