using FlightPlanner.Core.Dto;
using FlightPlanner.Core.Services;

namespace Flight.Planner.Services.Validators
{
    public class SearchValidator: ISearchValidator
    {
        public bool IsValid(SearchFlightRequest searchFlightRequest)
        {
            return searchFlightRequest.From != searchFlightRequest.To;
        }
    }
}
