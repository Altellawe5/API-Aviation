using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFirUirRepository
    {
        Task<IEnumerable<FirUir>> GetFir();
        Task<IEnumerable<FirUir>> GetUir();
        Task<IEnumerable<FirUir>> GetFirByAreaCode(string areaCode);
        Task<IEnumerable<FirUir>> GetUirByAreaCode(string areaCode);


    }
}
