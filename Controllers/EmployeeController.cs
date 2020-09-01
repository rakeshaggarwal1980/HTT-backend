using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HTTAPI.Controllers
{
    /// <summary>
    /// Employee Controller
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        IEmployeeService _employeeService;
        /// <summary>
        /// Employee Controller Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="employeeService"></param>
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Registers an employee
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> CreateEmployee([FromBody] UserSignUpViewModel employeeSignUpViewModel)
        {
            _logger.LogInformation("Creating Employee");
            var result = await _employeeService.CreateEmployee(employeeSignUpViewModel);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Updates an employee detail
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> UpdateEmployee([FromBody] EmployeeViewModel employeeViewModel)
        {
            _logger.LogInformation("updating Employee");
            var result = await _employeeService.UpdateEmployee(employeeViewModel);
            return StatusCode((int)result.StatusCode, result);
        }


        /// <summary>
        /// Employee Confirmation
        /// </summary>
        /// <returns></returns>
        [HttpGet("/accountstatus/{id}/{value}")]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> UpdateAccountStatus(int id, EntityStatus value)
        {
            var result = await _employeeService.UpdateAccountStatus(id, value);
            return StatusCode((int)result.StatusCode, result);
        }



        /// <summary>
        /// Get an employee by email
        /// </summary>
        /// <returns></returns>
        [HttpGet("/employeebyemail/{email}")]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetEmployeeByEmail(string email)
        {
            _logger.LogInformation("get Employee by email");
            var result = await _employeeService.GetEmployeeByEmail(email);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get an employee by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("/employeebyid/{id}")]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetEmployeeById(int id)
        {
            _logger.LogInformation("get Employee by Id");
            var result = await _employeeService.GetEmployeeById(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns></returns>
        [HttpGet("/list")]
        [ProducesResponseType(typeof(List<EmployeeViewModel>), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetEmployees()
        {
            _logger.LogInformation("get All Employees");
            var result = await _employeeService.GetEmployeeList();
            return StatusCode((int)result.StatusCode, result);
        }



    }
}
