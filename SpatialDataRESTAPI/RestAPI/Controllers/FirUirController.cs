using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
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
    public class FirUirController : ControllerBase
    {
        private readonly IFirUirRepository _firRepo;
        private readonly IMapper _mapper;

        public FirUirController(IFirUirRepository firRepo, IMapper mapper)
        {
            _firRepo = firRepo;
            _mapper = mapper;
        }

        //[HttpGet("fir")]
        //public async Task<ActionResult<IEnumerable<FirUirDTO>>> GetFir()
        //{
        //    var fir = await _firRepo.GetFir();
        //    return Ok(_mapper.Map<IEnumerable<FirUirDTO>>(fir));
        //}

        //[HttpGet("uir")]
        //public async Task<ActionResult<IEnumerable<FirUirDTO>>> GetUir()
        //{
        //    var uir = await _firRepo.GetUir();
        //    return Ok(_mapper.Map<IEnumerable<FirUirDTO>>(uir));
        //}

        [HttpGet("fir")]
        public async Task<IActionResult> GetFir()
        {

            var firpoints = await _firRepo.GetFir();

            var firFeatures = ConstructFirUirFeatures(firpoints);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = firFeatures.ToList()
            };

            return Ok(featureCollection);
        }

        [HttpGet("fir/byAreaCode/{areaCode}")]
        public async Task<ActionResult<IEnumerable<FirUir>>> GetFirByAreaCode(string areaCode)
        {
            var firpoints = await _firRepo.GetFirByAreaCode(areaCode);

            var firFeatures = ConstructFirUirFeatures(firpoints);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = firFeatures.ToList()
            };

            return Ok(featureCollection);
        }


        [HttpGet("uir")]
        public async Task<IActionResult> GetUir()
        {
            var uirpoints = await _firRepo.GetUir();

            var uirFeatures = ConstructFirUirFeatures(uirpoints);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = uirFeatures.ToList()
            };

            return Ok(featureCollection);
        }

        [HttpGet("uir/byAreaCode/{areaCode}")]
        public async Task<ActionResult<IEnumerable<FirUir>>> GetUirByAreaCode(string areaCode)
        {
            var uirpoints = await _firRepo.GetUirByAreaCode(areaCode);

            var uirFeatures = ConstructFirUirFeatures(uirpoints);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = uirFeatures.ToList()
            };

            return Ok(featureCollection);
        }

        private IEnumerable<GeoJsonFeature> ConstructFirUirFeatures(IEnumerable<FirUir> firUirPoints)
        {
            var excludedIdentifiers = new HashSet<string> {  "UHMM", "PAZA", "XX02", "FAJO", "BGGL", "ULLL", "UHPP" };

            var features = new List<GeoJsonFeature>();

            foreach (var group in firUirPoints
                .Where(p => !excludedIdentifiers.Contains(p.FirUirIdentifier)) 
                .GroupBy(p => new { p.FirUirIdentifier, p.FirUirIndicator, p.FirUirName })
                .Select(g => new { g.Key, Points = g.OrderBy(p => p.SequenceNumber).ToList() }))
            {
                // Check if the polygon is closed; if not, add the first point at the end.
                if (group.Points.Count > 1 &&
                    (group.Points.First().FirUirLatitude != group.Points.Last().FirUirLatitude ||
                     group.Points.First().FirUirLongitude != group.Points.Last().FirUirLongitude))
                {
                    group.Points.Add(group.Points.First());
                }

                // Convert FIR/UIR points to the format expected by your GeoJsonPolygon class.
                var polygonCoordinates = group.Points
                    .Select(p => new decimal[] { (decimal)p.FirUirLongitude, (decimal)p.FirUirLatitude })
                    .ToArray(); // Convert to array since your property expects decimal[][][]


                var firstPoint = group.Points.First();


                features.Add(new GeoJsonFeature
                {
                    Geometry = new GeoJsonPolygon
                    {
                        // Wrap the coordinates array to match the decimal[][][] structure.
                        Coordinates = new decimal[][][] { polygonCoordinates }
                    },
                    Properties = new Dictionary<string, object>
                    {
                        { "FirUirIdentifier", group.Key.FirUirIdentifier },
                        { "FirUirIndicator", group.Key.FirUirIndicator },
                        { "FirUirName", group.Key.FirUirName },
                        { "AreaCode", firstPoint.AreaCode },
                        { "AdjacentFirIdentifier", firstPoint.AdjacentFirIdentifier },
                        { "AdjacentUirIdentifier", firstPoint.AdjacentUirIdentifier  }
                    }
                });
            }

            return features;
        }



    }
}
