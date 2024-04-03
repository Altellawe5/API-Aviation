using Domain.GeoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.DTO
{
    public class VhfNavaidDTO
    {
        public string AreaCode { get; set; }
        public string VorIdentifier { get; set; }
        public string VorName { get; set; }
        public string FirIdentifier { get; set; }
        public string UirIdentifier { get; set; }

        public GeoJsonPoints SpatialData { get; set; }
    }
}
