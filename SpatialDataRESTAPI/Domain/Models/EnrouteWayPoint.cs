using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class EnrouteWayPoint
    {
        public string CycleId { get; set; }
        public string AreaCode { get; set; }
        public string RegionCode { get; set; }
        public string IcaoCode { get; set; }
        public string WaypointId { get; set; }
        public string WaypointName { get; set; }
        public string WaypointType { get; set; }
        public string WaypointUsage { get; set; }
        public string DmsLatitude { get; set; }
        public string DmsLongitude { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string DynamicMagneticVariation { get; set; }
        public string NameFormatIndicator { get; set; }
        public string FIRIdentifier { get; set; }
        public string UIRIdentifier { get; set; }
        public string StartEndIndicator { get; set; }
        public DateTime? StartEndDate { get; set; }
        public string CycleDate { get; set; }

    }


}
