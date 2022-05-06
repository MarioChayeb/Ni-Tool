using BabyNI.Data.Model;
using BabyNI.Data.Services;
using Microsoft.AspNetCore.Cors;
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
    public class FetchController : ControllerBase
    {
        public static FetchingService _fetchingservice;
        private List<HourlyAgg> data = new List<HourlyAgg>{
            new HourlyAgg{Time=new DateTime(2019,05,09,9,15,0),MaxRxLevel=52.5},
            new HourlyAgg{Time=new DateTime(2022,05,09,9,15,0),MaxRxLevel=56.5}
        };

        public FetchController(FetchingService fetchingservice)
        {
            _fetchingservice = fetchingservice;

        }
        [EnableCors("AllowOrigin")]
        [HttpGet("Daily-data")]
        
        public IActionResult GetDailyData()
        {
           return Ok( _fetchingservice.GetDailyData());
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("Hourly-data")]

        public IActionResult GetHourlyData()
        {
            return Ok(_fetchingservice.GetHourlyData());
        }


        [EnableCors("AllowOrigin")]
        [HttpGet("Hourly-data/{NE_ALIAS}")]

        public IActionResult GetDailyData_link(string NE_ALIAS)
        {
            return Ok(_fetchingservice.GetHourlyData_link_slot( NE_ALIAS));
        }


        [EnableCors("AllowOrigin")]
        [HttpGet("GetLinks/{NE_ALIAS}")]

        public IActionResult Getdistinctlinks(string NE_ALIAS)
        {
            return Ok(_fetchingservice.Getdistinctlinks(NE_ALIAS));
        }
    }
}
