using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class VhfNavaidEF
    {
        public string AreaCode {  get; set; }
        public string VorIdentifier { get; set; }
        public string VorName { get; set;}
        public string FirIdentifier { get; set; }
        public string UirIdentifier { get; set; }

        public decimal? VorLatitude { get; set; }
        public decimal? VorLongitude { get; set; }
    }
}
