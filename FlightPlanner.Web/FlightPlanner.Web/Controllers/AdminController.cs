using FlightPlanner.Web.Models;
using FlightPlanner.Web.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Web.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private static readonly object FlightLock = new object();

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            lock (FlightLock)
            {
                var flight = FlightStorage.GetById(id);
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

                if (FlightStorage.FlightExists(flight))
                    return Conflict();

                FlightStorage.AddFlight(flight);
                return Created("", flight);
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (FlightLock)
            {
                var flight = FlightStorage.GetById(id);
                if (flight is not null)
                    FlightStorage.DeleteFlight(flight);
                return Ok();
            }
        }
    }
}