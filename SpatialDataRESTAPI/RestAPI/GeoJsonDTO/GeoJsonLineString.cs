using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.GeoJsonDTO
{
    public class GeoJsonLineString : GeoJsonGeometry
    {
        public override string Type => "LineString";
        public new decimal[][]? Coordinates { get; set; }
    }
}
