using Domain.GeoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.DTO
{
    public class EnrouteWayPointDTO
    {
        public string AreaCode { get; set; }
        public string WaypointId { get; set; }
        public string WaypointName { get; set; }
        public string FirIdentifier { get; set; }
        public string UirIdentifier { get; set; }

        public GeoJsonPoints SpatialData { get; set; }
    }
}
