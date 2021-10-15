using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Web.DbContext;
using FlightPlanner.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Web.Storage
{
    public static class FlightStorage
    {
        public static bool IsValidFlight(Flight flight)
        {
            return !String.IsNullOrEmpty(flight.ArrivalTime) &&
                   !String.IsNullOrEmpty(flight.Carrier) &&
                   IsValidAirport(flight.From) &&
                   IsValidAirport(flight.To) &&
                   IsDifferentAirport(flight.From, flight.To) &&
                   !String.IsNullOrEmpty(flight.ArrivalTime) &&
                   !String.IsNullOrEmpty(flight.DepartureTime) &&
                   IsValidDate(flight.DepartureTime, flight.ArrivalTime);
        }

        public static bool IsValidAirport(Airport airport)
        {
            return airport is not null &&
                   !String.IsNullOrEmpty(airport.Country) &&
                   !String.IsNullOrEmpty(airport.City) &&
                   !String.IsNullOrEmpty(airport.AirportCode);
        }

        public static bool IsDifferentAirport(Airport from, Airport to)
        {
            return from.AirportCode.ToLower().Trim() != to.AirportCode.ToLower().Trim();
        }

        public static bool IsValidDate(string departureTime, string arrivalTime)
        {
            DateTime departure = Convert.ToDateTime(departureTime);
            DateTime arrival = Convert.ToDateTime(arrivalTime);
            return departure < arrival;
        }

        public static bool FlightExists(Flight flight, FlightPlannerDbContext context)
        {
            return context.Flights
                .Include(a => a.To)
                .Include(a => a.From).Any(x =>
                                x.From.Country == flight.From.Country &&
                                x.From.City == flight.From.City &&
                                x.From.AirportCode == flight.From.AirportCode &&
                                x.To.City == flight.To.City &&
                                x.To.Country == flight.To.Country &&
                                x.To.AirportCode == flight.To.AirportCode &&
                                x.Carrier == flight.Carrier &&
                                x.ArrivalTime == flight.ArrivalTime &&
                                x.DepartureTime == flight.DepartureTime);
        }

        public static List<Airport> SearchAirportByKeyword(string search, FlightPlannerDbContext context)
        {
            search = search.Trim().ToLower();
            List<Airport> airportFound = new List<Airport>();
            foreach (Airport airport in context.Airports)
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

        public static PageResult SearchFlight(SearchFlightRequest searchFlightRequest, FlightPlannerDbContext context)
        {
            var flight = context.Flights
                .Include(a => a.To)
                .Include(a => a.From)
                .Where(item => item.From.AirportCode == searchFlightRequest.From ||
                               item.To.AirportCode == searchFlightRequest.To ||
                               item.DepartureTime == searchFlightRequest.Date).ToList();
            return new PageResult(flight);
        }
    }
}
