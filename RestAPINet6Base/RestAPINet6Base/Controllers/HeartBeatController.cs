using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestAPINet6Base.DTOs;

namespace RestAPINet6Base.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/HeartBeat")]
    public class HeartBeatController : ControllerBase
    {
        private ILogger<HeartBeatController> logger;
        private readonly IConfiguration configuration; // To read from configuration file

        public HeartBeatController(ILogger<HeartBeatController> logger,
                                   IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
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

        // GET: api/HeartBeat/TestControllerJWT
        [HttpGet("TestControllerJWT")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<object> TestControllerJWT()
        {
            try
            {
                Console.WriteLine(string.Format("{0} - [Testing Controller]", DateTime.Now));
                logger.LogInformation(string.Format("{0} - [Testing Controller]", DateTime.Now));

                return Ok(new { apiStatus = "Success", timeStamp = DateTime.Now, msg = "OK" });
            }
            catch (Exception ex)
            { Console.WriteLine(ex); logger.LogError("Error TestControllerJWT", ex); return BadRequest(new { apiStatus = "Error", timeStamp = DateTime.Now, msg = ex.ToString() }); }
        }

        // POST: api/HeartBeat/GetToken
        [HttpPost("GetToken")]
        public ActionResult<object> GetToken(UserCredentials userCredentials)
        {
            logger.LogInformation(string.Format("Attempting to register user: {0}", userCredentials.Email));

            if (!string.IsNullOrEmpty(userCredentials.Email))
            {
                return BuildToken(userCredentials);
            }
            else
            {
                return Ok(new { ok = false, Token = "", msg = "Error creating token" }) ;
            }
        }

        private object BuildToken(UserCredentials userCredentials)
        {
            //Claims - User Information in which we can trust
            var claims = new List<Claim>
            {
                new Claim("email", userCredentials.Email),
                new Claim("timestamp", DateTime.Now.ToString("MM-dd-yyyyTHH:mm:ss.fff"))
            };

            //TODO: Uncomment this section when admin claim is working
            //var claimsDB = await userManager.GetClaimsAsync(user);

            //claims.AddRange(claimsDB);

            //TODO: Configure KeyJWT as secret
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["KeyJWT"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new
            {
                ok = true,
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };
        }
    }
}
