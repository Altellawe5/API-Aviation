using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FirUirRepository : IFirUirRepository
    {
        private readonly MenuDbContext _context;
        private readonly IMapper _mapper;

        public FirUirRepository(MenuDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<FirUir>> GetFir()
        {
            var fir = await _context.FirUir.Where(a => a.FirUirIndicator == "F" || a.FirUirIndicator == "B").ToListAsync();
            return _mapper.Map<IEnumerable<FirUir>>(fir);
        }

        public async Task<IEnumerable<FirUir>> GetFirByAreaCode(string areaCode)
        {
            var fir = await _context.FirUir
                .Where(a => (a.FirUirIndicator == "F" || a.FirUirIndicator == "B") && a.AreaCode == areaCode)
                .ToListAsync();
            return _mapper.Map<IEnumerable<FirUir>>(fir);
        }

        public async Task<IEnumerable<FirUir>> GetUir()
        {
            var uir = await _context.FirUir.Where(a => a.FirUirIndicator == "U" || a.FirUirIndicator == "B").ToListAsync();
            return _mapper.Map<IEnumerable<FirUir>>(uir);
        }

        public async Task<IEnumerable<FirUir>> GetUirByAreaCode(string areaCode)
        {
            var uir = await _context.FirUir
                .Where(a => (a.FirUirIndicator == "U" || a.FirUirIndicator == "B") && a.AreaCode == areaCode)
                .ToListAsync();
            return _mapper.Map<IEnumerable<FirUir>>(uir);
        }
    }
}
