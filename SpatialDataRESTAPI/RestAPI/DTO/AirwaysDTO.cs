using Domain.GeoJson;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.DTO
{
    public class AirwaysDTO
    {
        public string RouteIdentifier { get; set; }
        public string Level { get; set; }
        public string AreaCode { get; set; }
        public string FirIdentifier { get; set; }
        public string UirIdentifier { get; set; }

        public GeoJsonLinestrings Spatial { get; set; }
    }
}
