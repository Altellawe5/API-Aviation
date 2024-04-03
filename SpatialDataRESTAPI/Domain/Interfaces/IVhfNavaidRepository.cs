using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IVhfNavaidRepository
    {
        Task<IEnumerable<VhfNavaid>> GetAllVhfNavaid();

        Task<VhfNavaid> GetVhfNavaidById(string id);
        Task<IEnumerable<VhfNavaid>> GetVhfNavaidsByAreaCode(string areaCode);
    }
}
