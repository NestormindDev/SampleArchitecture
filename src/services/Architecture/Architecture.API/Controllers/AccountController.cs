using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture.API.Application.Models;
using Architecture.API.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc; 

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Architecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtHandler _jwtHandler;

        public AccountController(JwtHandler jwtHandler)
        {

            this._jwtHandler = jwtHandler;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate(User user)
        {
            // Pass parameter for login
            //var employee = new
            //{
            //    UserGuid = Guid.NewGuid().ToString(),
            //    EmployeeId = 1,
            //    Email = "test@mailinator.com",
            //    Password = "123456",
            //    FirstName = "Gurpreet",
            //    LastName = "Singh"
            //};

            AuthToken authToken = new AuthToken();
            try
            {
                authToken.JWTToken = this._jwtHandler.GenerateToken(user);
                authToken.RefreshToken = this._jwtHandler.GenerateRefreshToken();
            }
            catch (Exception)
            {
                return Unauthorized();
            }

            return Ok(authToken);
        }
    }
}
