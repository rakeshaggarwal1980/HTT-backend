﻿using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="employeeService"></param>
        public LoginController(ILogger<LoginController> logger,
             IEmployeeService employeeService)
        {
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
            var result = await _employeeService.GetEmployee(loginViewModel);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
