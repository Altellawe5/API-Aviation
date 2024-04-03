using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EnrouteWayPointRepository : IEnrouteWayPointRepository
    {
        private readonly MenuDbContext _context;
        private readonly IMapper _mapper;

        public EnrouteWayPointRepository(MenuDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnrouteWayPoint>> GetAllEnrouteWayPoints()
        {
            var entities = await _context.EnrouteWaypoint.ToListAsync();

            return _mapper.Map<IEnumerable<EnrouteWayPoint>>(entities);
        }

        public async Task<EnrouteWayPoint> GetWaypointById(string waypointId)
        {
            var entity = await _context.EnrouteWaypoint.SingleOrDefaultAsync(w => w.WaypointId == waypointId);
            return _mapper.Map<EnrouteWayPoint>(entity);
        }

        public async Task<IEnumerable<EnrouteWayPoint>> GetWaypointsByAreaCode(string areaCode)
        {
            var entities = await _context.EnrouteWaypoint
                                          .Where(w => w.AreaCode == areaCode)
                                          .ToListAsync();
            return _mapper.Map<IEnumerable<EnrouteWayPoint>>(entities);
        }
    }
}
