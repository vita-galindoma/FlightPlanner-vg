using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Web3.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IAirportService _airportService;
        private readonly IMapper _mapper;
        private readonly ISearchValidator _searchValidator;

        public CustomerController(IFlightService flightService, IAirportService airportService, IMapper mapper, ISearchValidator searchValidator)
        {
            _flightService = flightService;
            _mapper = mapper;
            _airportService = airportService;
            _searchValidator = searchValidator;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports(string search)
        {
            var airportArray = _airportService.SearchAirportByKeyword(search);
            return Ok(_mapper.Map<AirportResponse[]>(airportArray));
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult PostSearchFlights(SearchFlightRequest searchFlightRequest)
        { 
            if (_searchValidator.IsValid(searchFlightRequest))
            {
                var flights = _flightService.SearchFlight(searchFlightRequest);
                return Ok(flights);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightService.GetFullFlightById(id);
            if (flight == null)
                return NotFound();

            return Ok(_mapper.Map<FlightResponse>(flight));
        }
    }
}
