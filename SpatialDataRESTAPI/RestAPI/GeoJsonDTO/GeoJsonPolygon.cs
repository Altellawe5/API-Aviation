using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.GeoJsonDTO
{
    public class GeoJsonPolygon : GeoJsonGeometry
    {
        public override string Type => "Polygon";
        public decimal[][][] Coordinates { get; set; }



    }
}
