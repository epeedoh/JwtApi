using JwtApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtAuthenticationService _JwtAuthenticationService;
        private readonly IConfiguration _config;

        public AuthenticationController(IJwtAuthenticationService JwtAuthenticationService, IConfiguration config)
        {
            this._JwtAuthenticationService = JwtAuthenticationService;
            this._config = config;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] LoginModel model)
        {

            var user = _JwtAuthenticationService.Authenticate(model.Email, model.Password);
            if(user != null )
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email)
                };

                var token = _JwtAuthenticationService.GenerateToken(_config["Jwt:Key"], claims);
                return Ok(token);
            }

            return Unauthorized();
        }

    }
}
