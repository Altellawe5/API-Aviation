using AutoMapper;
using Domain.Models;
using Infrastructure.DTO;


namespace Infrastructure.Mapping
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<AirportEF, Airport>().ReverseMap();

            CreateMap<EnrouteWayPointEF, EnrouteWayPoint>().ReverseMap();

            CreateMap<VhfNavaidEF, VhfNavaid>().ReverseMap();

            CreateMap<AirwaysEF, Airways>().ReverseMap();

            CreateMap<FirUirEF, FirUir>().ReverseMap();



        }
        
    }
}
