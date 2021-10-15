using System.Linq;
using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace Flight.Planner.Services
{
    public class FlightService : EntityService<FlightPlanner.Core.Models.Flight>, IFlightService
    {
        public FlightService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public FlightPlanner.Core.Models.Flight GetFullFlightById(int id)
        {
            return _context.Flights
                .Include(f => f.From)
                .Include(f => f.To).SingleOrDefault(f => f.Id == id);
        }

        public bool Exists(FlightPlanner.Core.Models.Flight flight)
        {
            return Query().Any(f => f.ArrivalTime == flight.ArrivalTime
                                    && f.DepartureTime == flight.DepartureTime
                                    && f.Carrier == flight.Carrier
                                    && f.From.AirportCode == flight.From.AirportCode
                                    && f.To.AirportCode == flight.To.AirportCode);
        }

        public void DeleteById(int id)
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
        }

        public PageResult SearchFlight(SearchFlightRequest searchFlightRequest)
        {
            var flight = _context.Flights
                .Include(a => a.To)
                .Include(a => a.From)
                .Where(item => item.From.AirportCode.ToLower().Trim() == searchFlightRequest.From.ToLower().Trim() &&
                               item.To.AirportCode.ToLower().Trim() == searchFlightRequest.To.ToLower().Trim() &&
                               item.DepartureTime.Substring(0, 10) == searchFlightRequest.DepartureDate).ToList();
            return new PageResult(flight);
        }
    }
}
