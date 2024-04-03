using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class VhfNavaid
    {
        public string AreaCode { get; set; }
        public string VorIdentifier { get; set; }
        public string VorName { get; set; }
        public string FirIdentifier { get; set; }
        public string UirIdentifier { get; set; }

        public decimal? VorLatitude { get; set; }
        public decimal? VorLongitude { get; set; }
    }
}
