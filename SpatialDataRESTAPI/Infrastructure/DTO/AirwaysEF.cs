using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class AirwaysEF
    {

        public string RouteIdentifier { get; set; }

        public string CycleId { get; set; }

        public string WaypointDescriptionCode { get; set; }
        public string Level { get; set; }
        public string AreaCode { get; set; }
        public string? FirIdentifier { get; set; }
        public string? UirIdentifier { get; set; }
        public decimal? WaypointLatitude { get; set; }
        public decimal? WaypointLongitude { get; set; }
        public int SequenceNumber { get; set; }
    }
}
