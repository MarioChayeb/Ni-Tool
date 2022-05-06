using BabyNI.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BabyNI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        //creating Loadservice instance to use functions
        public static LoadService _loadService;



        //constructor.
        public LoadController(LoadService loadservice)
        {
            _loadService = loadservice;

        }





        //Copy data
        [HttpPost("Copy-Data")]
        public IActionResult CopyData()
        {
            _loadService.CopyData();

            return Ok();
        }

        [HttpPost("Re-ExecuteLoader")]

        public IActionResult ParseData(string custompath)
        {
            try
            {

                _loadService.ReExecuteLoader(custompath);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest("Error Parsing The File ");
            }

        }
    }
}
