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
    public class VhfNavaidRepository : IVhfNavaidRepository
    {
        private readonly MenuDbContext _context;
        private readonly IMapper _mapper;

        public VhfNavaidRepository(MenuDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<VhfNavaid>> GetAllVhfNavaid()
        {
            var entities = await _context.VhfNavaid
                .Where(a => a.VorLatitude != null && a.VorLongitude != null)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VhfNavaid>>(entities);
        }

        public async Task<VhfNavaid> GetVhfNavaidById(string id)
        {
            var entity = await _context.VhfNavaid.SingleOrDefaultAsync(a => a.VorIdentifier == id);
            return _mapper.Map<VhfNavaid>(entity);
        }

        public async Task<IEnumerable<VhfNavaid>> GetVhfNavaidsByAreaCode(string areaCode)
        {
            var entities = await _context.VhfNavaid
                                     .Where(a => a.AreaCode == areaCode)
                                     .ToListAsync();
            return _mapper.Map<IEnumerable<VhfNavaid>>(entities);
        }
    }
}
