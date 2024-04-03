using Domain.GeoJson;
using Microsoft.SqlServer.Types;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.DTO
{
    public class AirportDTO
    {
        public string AirportId { get; set; }
        public string AirportName { get; set; }
        public string UirIdentifier { get; set; }
        public string FirIdentifier { get; set; }

        public GeoJsonPoints SpatialData { get; set; }
    }
}
