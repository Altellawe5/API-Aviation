using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.GeoJsonDTO
{
    public class GeoJsonFeatureCollection
    {
        public string Type { get; } = "FeatureCollection";
        public List<GeoJsonFeature> Features { get; set; } = new List<GeoJsonFeature>();
    }

}
