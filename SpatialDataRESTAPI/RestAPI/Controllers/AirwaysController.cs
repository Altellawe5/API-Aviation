using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
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
    public class AirwaysController : ControllerBase
    {
        private readonly IAirwaysRepository _airwaysRepository;
        private readonly IMapper _mapper;

        public AirwaysController(IAirwaysRepository airwaysRepository, IMapper mapper)
        {
            _airwaysRepository = airwaysRepository;
            _mapper = mapper;
        }

        //[HttpGet("high")]
        //public async Task<ActionResult<IEnumerable<AirwaysDTO>>> GetHighAirways()
        //{
        //    var highAirways = await _airwaysRepository.GetHighAirways();
        //    return Ok(_mapper.Map<IEnumerable<AirwaysDTO>>(highAirways));
        //}

        //[HttpGet("low")]
        //public async Task<ActionResult<IEnumerable<AirwaysDTO>>> GetLowAirways()
        //{
        //    var lowAirways = await _airwaysRepository.GetLowAirways();
        //    return Ok(_mapper.Map<IEnumerable<AirwaysDTO>>(lowAirways));
        //}

        //[HttpGet("high")]
        //public async Task<IActionResult> GetHighAirways()
        //{
        //    var highAirways = await _airwaysRepository.GetHighAirways();
        //    var highAirwayFeatures = _mapper.Map<IEnumerable<GeoJsonFeature>>(highAirways);

        //    var featureCollection = new GeoJsonFeatureCollection
        //    {
        //        Features = highAirwayFeatures.ToList()
        //    };

        //    return Ok(featureCollection);
        //}
        [HttpGet("high")]
        public async Task<IActionResult> GetHighAirways()
        {
            var waypoints = await _airwaysRepository.GetHighAirways();  // Fetch high waypoints

            var airwayFeatures = ConstructAirwayFeatures(waypoints);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = airwayFeatures.ToList()
            };

            return Ok(featureCollection);
        }


        [HttpGet("low")]
        public async Task<IActionResult> GetLowAirways()
        {
            var waypoints = await _airwaysRepository.GetLowAirways();  // Fetch high waypoints

            var airwayFeatures = ConstructAirwayFeatures(waypoints);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = airwayFeatures.ToList()
            };

            return Ok(featureCollection);
        }

        private IEnumerable<GeoJsonFeature> ConstructAirwayFeatures(IEnumerable<Airways> waypoints)
        {
            var features = new List<GeoJsonFeature>();

            foreach (var group in waypoints
                .GroupBy(w => new { w.RouteIdentifier, w.AreaCode })
                .Select(g => new { g.Key, Points = g.OrderBy(w => w.SequenceNumber).ToList() }))
            {
                for (int i = 0; i < group.Points.Count - 1; i++)
                {
                    var start = group.Points[i];
                    var end = group.Points[i + 1];

                    // description code's second letter being 'E' only marks the end of a segment.
                    var startDescriptionFlag = start.WaypointDescriptionCode.Length >= 3 && start.WaypointDescriptionCode[1] == 'E';
                    var endDescriptionFlag = end.WaypointDescriptionCode.Length >= 3 && end.WaypointDescriptionCode[1] == 'E';

                    // If neither point marks an immediate end (or it's the end for start and beginning for end), form the line
                    if (!startDescriptionFlag || (startDescriptionFlag && endDescriptionFlag))
                    {
                        features.Add(new GeoJsonFeature
                        {
                            Geometry = new GeoJsonLineString
                            {
                                Coordinates = new List<decimal[]>
                                {
                                    new decimal[] { (decimal)start.WaypointLongitude, (decimal)start.WaypointLatitude },
                                    new decimal[] { (decimal)end.WaypointLongitude, (decimal)end.WaypointLatitude }
                                }.ToArray()
                            },
                            Properties = new Dictionary<string, object>
                            {
                                { "RouteIdentifier", group.Key.RouteIdentifier },
                                { "AreaCode", group.Key.AreaCode },
                                { "FIRIdentifier", start.FirIdentifier },
                                { "UIRIdentifier", start.UirIdentifier },
                            }
                        });

                        // If end point also starts a new segment, it will naturally create a new line with the next in the next loop iteration
                    }
                }
            }

            return features;
        }






    }
}
