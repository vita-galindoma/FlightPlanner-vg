using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight GetFullFlightById(int id);

        bool Exists(Flight flight);

        void DeleteById(int id);

        PageResult SearchFlight(SearchFlightRequest searchFlightRequest);
    }
}
