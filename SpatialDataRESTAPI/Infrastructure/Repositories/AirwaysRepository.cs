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
    public class AirwaysRepository : IAirwaysRepository
    {
        private readonly MenuDbContext _context;
        private readonly IMapper _mapper;

        public AirwaysRepository(MenuDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Airways>> GetHighAirways()
        {
            var highAirways = await _context.EnrouteAirwaysLines
                  .Where(a => (a.Level == "H" || a.Level == "B") && a.WaypointLongitude != null && a.WaypointLatitude != null)
                  .ToListAsync();
            return _mapper.Map<IEnumerable<Airways>>(highAirways);
        }

        public async Task<IEnumerable<Airways>> GetLowAirways()
        {
            var lowAirways = await _context.EnrouteAirwaysLines
                    .Where(a => (a.Level == "L" || a.Level == "B") && a.WaypointLongitude != null && a.WaypointLatitude != null)
                    .ToListAsync();
            return _mapper.Map<IEnumerable<Airways>>(lowAirways);
        }
    }
}
