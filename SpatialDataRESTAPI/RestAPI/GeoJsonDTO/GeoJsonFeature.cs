using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.GeoJsonDTO
{
    public class GeoJsonFeature
    {
        public string Type { get; } = "Feature";
        public GeoJsonGeometry? Geometry { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();


    }

}
