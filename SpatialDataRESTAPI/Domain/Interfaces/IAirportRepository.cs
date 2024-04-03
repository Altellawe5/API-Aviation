using Domain.Models;


namespace Domain.Interfaces
{
    public interface IAirportRepository
    {
        Task<IEnumerable<Airport>> GetAllAirports();
        Task<Airport> GetAirportById(string id);
        Task<IEnumerable<Airport>> GetAirportsByAreaCode(string areaCode);
    }
}
