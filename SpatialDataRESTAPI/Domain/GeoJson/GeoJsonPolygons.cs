using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.GeoJson
{
    public class GeoJsonPolygons
    {
        public string Type { get; set; }
        public decimal[][][] Coordinates { get; set; }
    }
}
