using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HTTAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/login")]
    [ApiController]
    public class LoginController : Controller
    {

        private readonly ILogger<LoginController> _logger;
        readonly IEmployeeService _employeeService;
        IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="employeeService"></param>
        public LoginController(IConfiguration configuration, ILogger<LoginController> logger,
             IEmployeeService employeeService)
        {
            _configuration = configuration;
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> LoginUser([FromBody] UserLoginViewModel loginViewModel)
        {
            _logger.LogInformation("Log in Employee");
            var result = await _employeeService.GetEmployee(loginViewModel);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
