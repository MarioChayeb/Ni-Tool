using BabyNI.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabyNI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregatingController : ControllerBase
    {
        public AggregatingService _aggregatingService;

        public AggregatingController(AggregatingService parsingService)
        {
            _aggregatingService = parsingService;
        }


        [HttpPut("Aggregate-Data")]


        public IActionResult AggregateData()
        {
            _aggregatingService.AggregateData();
            return Ok();
        }


    }
}
