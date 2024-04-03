using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportRepository _airportRepository;
        private readonly IMapper _mapper;

        public AirportsController(IAirportRepository airportRepository, IMapper mapper)
        {
            _airportRepository = airportRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAirports()
        {
            var airports = await _airportRepository.GetAllAirports();
            var airportFeatures = _mapper.Map<IEnumerable<GeoJsonFeature>>(airports);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = airportFeatures.ToList()
            };

            return Ok(featureCollection); // Returns the feature collection in GeoJSON format
        }

        [HttpGet("{airportId}")]
        public async Task<ActionResult<Airport>> GetAirportById(string airportId)
        {
            var airport = await _airportRepository.GetAirportById(airportId);

            if (airport == null)
            {
                return NotFound();
            }
            var airportFeatures = _mapper.Map<IEnumerable<GeoJsonFeature>>(airport);

            return Ok(airportFeatures);
        }

        [HttpGet("byAreaCode/{areaCode}")]
        public async Task<ActionResult<IEnumerable<Airport>>> GetAirportsByAreaCode(string areaCode)
        {
            var airports = await _airportRepository.GetAirportsByAreaCode(areaCode);
            var airportFeatures = _mapper.Map<IEnumerable<GeoJsonFeature>>(airports);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = airportFeatures.ToList()
            };

            return Ok(featureCollection);
        }
    }
}
