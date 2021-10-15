using System.Collections.Generic;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace Flight.Planner.Services
{
    public class AirportService : EntityService<Airport>, IAirportService

    {
    public AirportService(IFlightPlannerDbContext context) : base(context)
    {
    }

    public List<Airport> SearchAirportByKeyword(string search)
    {
        search = search.Trim().ToLower();
        List<Airport> airportFound = new List<Airport>();
        foreach (Airport airport in _context.Airports)
        {
            if (airport.City.ToLower().Contains(search) ||
                airport.Country.ToLower().Contains(search) ||
                airport.AirportCode.ToLower().Contains(search))
            {
                airportFound.Add(airport);
                return airportFound;
            }
        }

        return airportFound;
        }
    }
}

