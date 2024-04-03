using Microsoft.SqlServer.Types;
using NetTopologySuite.Geometries;

namespace Domain.Models
{
    public class Airport
    {
        public string CycleId { get; set; }
        public string? AreaCode { get; set; }
        public string? IcaoCode { get; set; }
        public string AirportId { get; set; }
        public string? AirportName { get; set; }
        public bool? IFRCapability { get; set; }
        public string? LongestRunwaySurfaceCode { get; set; }
        public int? AirportElevation { get; set; }
        public int? SpeedLimit { get; set; }
        public string? SpeedLimitAltitude { get; set; }
        public int? TransitionAltitude { get; set; }
        public int? TransitionLevel { get; set; }
        public string? IATADesignator { get; set; }
        public string? DmsLatitude { get; set; }
        public decimal? Latitude { get; set; }
        public string? DmsLongitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? LongestRunway { get; set; }
        public string? MagneticVariation { get; set; }
        public string? RecommendedNavaid { get; set; }
        public string? PublicMilitaryIndicator { get; set; }
        public string? TimeZone { get; set; }
        public string? DaylightIndicator { get; set; }
        public string? MagneticTrueIndicator { get; set; }
        public string? FirIdentifier { get; set; }
        public string? UirIdentifier { get; set; }
        public string? StartEndIndicator { get; set; }
        public DateTime? StartEndDate { get; set; }
        public string? ControlledASIndicator { get; set; }
        public string? ControlledASArptIdent { get; set; }
        public string? ControlledASArptICAO { get; set; }
        public string? CycleDate { get; set; }
    }

}
