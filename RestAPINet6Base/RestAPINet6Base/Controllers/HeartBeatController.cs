using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RestAPINet6Base.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/HeartBeat")]
    public class HeartBeatController : ControllerBase
    {
        private ILogger<HeartBeatController> logger;

        public HeartBeatController(ILogger<HeartBeatController> logger)
        {
            this.logger = logger;
        }

        // GET: api/HeartBeat/TestController
        [HttpGet("TestController")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<object> TestController()
        {
            try
            {
                Console.WriteLine(string.Format("{0} - [Testing Controller]", DateTime.Now));
                logger.LogInformation(string.Format("{0} - [Testing Controller]", DateTime.Now));

                return Ok( new { apiStatus = "Success", timeStamp = DateTime.Now, msg = "OK" });
            }
            catch (Exception ex)
            { Console.WriteLine(ex); logger.LogError("Error TestController", ex); return BadRequest(new { apiStatus = "Error", timeStamp = DateTime.Now, msg = ex.ToString() }); }
        }
    }
}
