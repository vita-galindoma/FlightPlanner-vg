using AutoMapper;
using FlightPlanner.Core.Dto;

namespace FlightPlanner.Web3.Mappings
{
    public class AutoMapperConfiguration
    {
        public static IMapper GetConfig()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FlightRequest, Core.Models.Flight>().ForMember(f => f.Id, opt => opt.Ignore());
                cfg.CreateMap<AirportRequest, Core.Models.Airport>().ForMember(a => a.AirportCode, opt => opt.MapFrom(s => s.Airport)).ForMember(a => a.Id, opt => opt.Ignore());
                cfg.CreateMap<Core.Models.Flight, FlightResponse>();
                cfg.CreateMap<Core.Models.Airport, AirportResponse>().ForMember(a =>a.Airport, opt => opt.MapFrom(a => a.AirportCode));
            });
            // only during development, validate your mappings; remove it before release
            configuration.AssertConfigurationIsValid();
            // use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
            var mapper = configuration.CreateMapper();
            return mapper;
        }
    }
}
