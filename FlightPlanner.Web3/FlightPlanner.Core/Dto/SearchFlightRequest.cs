using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Core.Dto
{
    public class SearchFlightRequest
    {
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public string DepartureDate { get; set; }
    }
}
