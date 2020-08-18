using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace HTTAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        IEmployeeService _employeeService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="employeeService"></param>
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("employee")]
        [ProducesResponseType(typeof(EmployeeViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> CreateEmployee([FromBody] EmployeeViewModel employeeViewModel)
        {
            _logger.LogInformation("Creating Employee");
            var result = await _employeeService.CreateEmployee(employeeViewModel);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
