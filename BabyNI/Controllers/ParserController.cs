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
    public class ParserController : ControllerBase
    {
        //creating ParsingService instance to use functions
        public static ParsingService _parsingservice;



        //constructor.

        public ParserController(ParsingService parsingService)
        {
            _parsingservice = parsingService;
        }



        [HttpPost("Parse-File")]
        public IActionResult ParseData()
        {
            try
            {

                _parsingservice.ParseFile();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest("Error Parsing The File ");
            }

        }

        //[HttpPost("Re-ExecuteParser")]

        //public IActionResult ParseData(string custompath)
        //{
        //    try
        //    {

        //        _parsingservice.ReExecuteParse(custompath);
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest("Error Parsing The File ");
        //    }

        //}



    }
}

