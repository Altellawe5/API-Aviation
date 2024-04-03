using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.GeoJsonDTO
{
    public abstract class GeoJsonGeometry
    {
        public abstract string Type { get; }
    }
}
