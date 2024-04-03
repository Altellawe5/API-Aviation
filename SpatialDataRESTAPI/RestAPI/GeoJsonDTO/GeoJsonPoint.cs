using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.GeoJsonDTO
{
    public class GeoJsonPoint : GeoJsonGeometry
    {
        public override string Type => "Point";
        public new decimal?[] Coordinates { get; set; }
    }
}
