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
    public class VhfNavaidController : ControllerBase
    {
        private readonly IVhfNavaidRepository _vhfNavaidRepository;
        private readonly IMapper _mapper;

        public VhfNavaidController(IVhfNavaidRepository vhfNavaidRepository, IMapper mapper)
        {
            _vhfNavaidRepository = vhfNavaidRepository;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<VhfNavaidDTO>>> GetVhfNavaids()
        //{
        //    var navaids = await _vhfNavaidRepository.GetAllVhfNavaid();

        //    var navaidsDto = _mapper.Map<IEnumerable<VhfNavaidDTO>>(navaids);
        //    return Ok(navaidsDto);
        //}

        [HttpGet]
        public async Task<IActionResult> GetVhfNavaids()
        {
            var navaids = await _vhfNavaidRepository.GetAllVhfNavaid();
            var navaidsFeatures = _mapper.Map<IEnumerable<GeoJsonFeature>>(navaids);

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = navaidsFeatures.ToList()
            };

            return Ok(featureCollection); // Returns the feature collection in GeoJSON format
        }
    }
}
