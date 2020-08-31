using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace HTTAPI.Controllers
{
    /// <summary>
    /// Employee Controller
    /// </summary>
    [Produces("application/json")]
    [ApiController]
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
        [HttpPost("api/employee")]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> CreateEmployee([FromBody] UserSignUpViewModel employeeSignUViewModel)
        {
            _logger.LogInformation("Creating Employee");
            var result = await _employeeService.CreateEmployee(employeeSignUViewModel);
            return StatusCode((int)result.StatusCode, result);
        }


        /// <summary>
        /// Get an employee
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/employee/{email}")]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetEmployee(string email)
        {
            _logger.LogInformation("get Employee by email");
            var result = await _employeeService.GetEmployeeByEmail(email);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
