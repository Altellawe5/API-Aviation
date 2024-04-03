using Domain.Models;
using Domain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;



namespace Infrastructure.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly MenuDbContext _context;
        private readonly IMapper _mapper;

        public AirportRepository(MenuDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Airport> GetAirportById(string id)
        {
            var entity = await _context.Airport.SingleOrDefaultAsync(a => a.AirportId == id);
            return _mapper.Map<Airport>(entity);
        }

        public async Task<IEnumerable<Airport>> GetAirportsByAreaCode(string areaCode)
        {
            var entities = await _context.Airport
                                      .Where(a => a.AreaCode == areaCode)
                                      .ToListAsync();
            return _mapper.Map<IEnumerable<Airport>>(entities);
        }

        public async Task<IEnumerable<Airport>> GetAllAirports()
        {
            var entities = await _context.Airport.ToListAsync();

            return  _mapper.Map<IEnumerable<Airport>>(entities);
        }
    }
}
