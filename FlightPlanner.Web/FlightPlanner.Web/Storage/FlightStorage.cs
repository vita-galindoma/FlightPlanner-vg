using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Web.Models;

namespace FlightPlanner.Web.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id;

        public static Flight GetById(int id)
        {
            return _flights.SingleOrDefault(f => f.Id == id);
        }

        public static void ClearFlights()
        {
            _flights.Clear();
        }

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = _id;
            _id++;
            _flights.Add(flight);
            return flight;
        }

        public static void DeleteFlight(Flight flight)
        {
            _flights.Remove(flight);
        }

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

        public static bool FlightExists(Flight flight)
        {
            return _flights.Any(x =>
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

        public static List<Airport> SearchAirportByKeyword(string search)
        {
            search = search.Trim().ToLower();
            List<Airport> airportFound = new List<Airport>();
            foreach (Flight flights in _flights)
            {
                if (flights.From.City.ToLower().Contains(search) ||
                    flights.From.Country.ToLower().Contains(search) ||
                    flights.From.AirportCode.ToLower().Contains(search))
                {
                    airportFound.Add(flights.From);
                    return airportFound;
                }
            }
            return airportFound;
        }

        public static PageResult SearchFlight(SearchFlightRequest searchFlightRequest)
        {
            var flight = _flights.
                Where(item => item.From.AirportCode == searchFlightRequest.From ||
                              item.To.AirportCode == searchFlightRequest.To ||
                              item.DepartureTime == searchFlightRequest.Date).ToList();
            return new PageResult(flight);
        }
    }
}
