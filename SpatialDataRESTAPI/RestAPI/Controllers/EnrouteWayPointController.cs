using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using NavSpatialData.DTO;
using NavSpatialData.GeoJsonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialData.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrouteWayPointController : ControllerBase
    {
        private readonly IEnrouteWayPointRepository _enrouteWayPointRepository;
        private readonly IMapper _mapper;

        public EnrouteWayPointController(IEnrouteWayPointRepository enrouteWayPointRepository, IMapper mapper)
        {
            _enrouteWayPointRepository = enrouteWayPointRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnrouteWayPoints()
        {
            var enroutewayPoints = await _enrouteWayPointRepository.GetAllEnrouteWayPoints();
            var wayPointsFeatures = _mapper.Map<IEnumerable<GeoJsonFeature>>(enroutewayPoints);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = wayPointsFeatures.ToList()
            };

            return Ok(featureCollection); // Returns the feature collection in GeoJSON format
        }

        // Method to get a single waypoint by ID (or another unique identifier like the combination of keys)
        [HttpGet("{waypointId}")]
        public async Task<IActionResult> GetWaypointById(string waypointId)
        {
            var waypoint = await _enrouteWayPointRepository.GetWaypointById(waypointId);
            if (waypoint == null)
            {
                return NotFound(); // Return 404 if no waypoint is found
            }

            var waypointFeature = _mapper.Map<GeoJsonFeature>(waypoint);
            return Ok(waypointFeature); // Returns the waypoint in GeoJSON format
        }

        // Method to get waypoints by area code
        [HttpGet("byAreaCode/{areaCode}")]
        public async Task<IActionResult> GetWaypointsByAreaCode(string areaCode)
        {
            var waypoints = await _enrouteWayPointRepository.GetWaypointsByAreaCode(areaCode);
            var waypointFeatures = _mapper.Map<IEnumerable<GeoJsonFeature>>(waypoints);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = waypointFeatures.ToList()
            };

            return Ok(featureCollection); // Returns the waypoints in GeoJSON format
        }
    }
}
