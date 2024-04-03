using Domain.GeoJson;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.DTO
{
    public class FirUirDTO
    {
        public string FirUirIdentifier { get; set; }
        public string FirUirIndicator { get; set; }
        public string AreaCode { get; set; }
        public string FirUirName { get; set; }
        public string Spatial { get; set; }
    }
}
