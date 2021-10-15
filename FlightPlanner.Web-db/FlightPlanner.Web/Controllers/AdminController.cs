using System.Linq;
using FlightPlanner.Web.DbContext;
using FlightPlanner.Web.Models;
using FlightPlanner.Web.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Web.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly FlightPlannerDbContext _context;
        private static readonly object FlightLock = new object();

        public AdminController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            lock (FlightLock)
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

        [HttpPut]
        [Route("flights")]
        public IActionResult PutFlight(Flight flight)
        {
            lock (FlightLock)
            {
                if (!FlightStorage.IsValidFlight(flight))
                    return BadRequest();

                if (FlightStorage.FlightExists(flight, _context))
                    return Conflict();
                
                _context.Flights.Add(flight);
                _context.SaveChanges();
                return Created("", flight);
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (FlightLock)
            {
                var flight = _context.Flights
                    .Include(f => f.To)
                    .Include(f => f.From)
                    .SingleOrDefault(f => f.Id == id);

                if (flight is not null)
                {
                    _context.Airports.Remove(flight.To);
                    _context.Airports.Remove(flight.From);
                    _context.Flights.Remove(flight);

                    _context.SaveChanges();
                }
                return Ok();
            }
        }
    }
}