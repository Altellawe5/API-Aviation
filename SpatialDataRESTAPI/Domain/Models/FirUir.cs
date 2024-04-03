using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class FirUir
    {
        public string FirUirIdentifier { get; set; }
        public string FirUirIndicator { get; set; }
        public string AreaCode { get; set; }
        public string FirUirName { get; set; }
        public int SequenceNumber { get; set; }
        public decimal FirUirLongitude { get; set; }
        public decimal FirUirLatitude { get; set; }
        //public string CycleData { get; set; }
        public string? AdjacentFirIdentifier { get; set; }
        public string? AdjacentUirIdentifier { get; set; }


     }
}
