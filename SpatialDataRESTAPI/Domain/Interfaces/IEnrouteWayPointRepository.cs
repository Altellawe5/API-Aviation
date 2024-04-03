using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEnrouteWayPointRepository
    {
        Task<IEnumerable<EnrouteWayPoint>> GetAllEnrouteWayPoints();
        Task<EnrouteWayPoint> GetWaypointById(string id);
        Task<IEnumerable<EnrouteWayPoint>> GetWaypointsByAreaCode(string areaCode);

    }
}
