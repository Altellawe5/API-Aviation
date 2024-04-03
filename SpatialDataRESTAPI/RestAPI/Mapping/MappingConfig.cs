using Domain.GeoJson;
using Domain.Models;
using Infrastructure.DTO;
using NetTopologySuite.Geometries;
using NavSpatialData.DTO;
using NavSpatialData.GeoJsonDTO;
using NetTopologySuite.Features;
using AutoMapper.Features;

namespace NavSpatialData.Mapping
{
    public class MappingConfig : Infrastructure.Mapping.MappingConfig
    {
        // mapping airports 

        public MappingConfig() : base() 
        {
            

            CreateMap<Airport, GeoJsonFeature>()
                .ForMember(feature => feature.Geometry, opt => opt.MapFrom(
                    src => new GeoJsonPoint { Coordinates = new[] { src.Longitude, src.Latitude } }))
                .ForMember(feature => feature.Properties, opt => opt.MapFrom(
                    src => new Dictionary<string, object>
                    {
                        { "AirportId", src.AirportId },
                        { "AirportName", src.AirportName },
                        { "UirIdentifier", src.UirIdentifier },
                        { "FirIdentifier", src.FirIdentifier },
                        { "AreaCode", src.AreaCode }
                    }));


            CreateMap<EnrouteWayPoint, GeoJsonFeature>()
                .ForMember(feature => feature.Geometry, opt => opt.MapFrom(
                    src => new GeoJsonPoint { Coordinates = new[] { src.Longitude, src.Latitude } }))
                .ForMember(feature => feature.Properties, opt => opt.MapFrom(
                    src => new Dictionary<string, object>
                    {
                        { "AreaCode", src.AreaCode },
                        { "WaypointId", src.WaypointId },
                        { "WaypointName", src.WaypointName },
                        { "FirIdentifier", src.FIRIdentifier },
                        { "UirIdentifier", src.UIRIdentifier },
                    }));

            CreateMap<VhfNavaid, GeoJsonFeature>()
                .ForMember(feature => feature.Geometry, opt => opt.MapFrom(
                    src => new GeoJsonPoint { Coordinates = new[] { src.VorLongitude, src.VorLatitude } }))
                .ForMember(feature => feature.Properties, opt => opt.MapFrom(
                    src => new Dictionary<string, object>
                    {
                        { "AreaCode", src.AreaCode },
                        { "VorIdentifier", src.VorIdentifier },
                        { "VorName", src.VorName },
                        { "FirIdentifier", src.FirIdentifier },
                        { "UirIdentifier", src.UirIdentifier }
                    }));


            CreateMap<FirUir, GeoJsonFeature>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Feature")) // Standard for all GeoJSON Features
            .ForMember(feature => feature.Properties, opt => opt.MapFrom(
                    src => new Dictionary<string, object>
                    {
                        { "FirUirIdentifier", src.FirUirIdentifier },
                        { "FirUirIndicator", src.FirUirIndicator },
                        { "AreaCode", src.AreaCode },
                        { "FirUirName", src.FirUirName },
                        { "AdjacentFirIdentifier", src.AdjacentFirIdentifier },
                        { "AdjacentUirIdentifier", src.AdjacentUirIdentifier  }
                    }));


            CreateMap<Airways, GeoJsonFeature>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Feature"))  // Standard for all GeoJSON Features
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => new Dictionary<string, object>
                {
                        {"RouteIdentifier", src.RouteIdentifier},
                        {"Level", src.Level},
                        {"AreaCode", src.AreaCode},
                        {"FirIdentifier", src.FirIdentifier},
                        {"UirIdentifier", src.UirIdentifier}
                }));  



        }
    }
}
