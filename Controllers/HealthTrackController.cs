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
    /// 
    /// </summary>
    [Route("api/health")]
    [ApiController]
    public class HealthTrackController : ControllerBase
    {
        private readonly ILogger<HealthTrackController> _logger;
        IHealthTrackService _healthTrackService;
        /// <summary>
        /// Health Track Controller Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="healthTrackService"></param>
        public HealthTrackController(ILogger<HealthTrackController> logger, IHealthTrackService healthTrackService)
        {
            _logger = logger;
            _healthTrackService = healthTrackService;
        }

        /// <summary>
        ///  Returns declaration form controls details
        /// </summary>
        /// <returns></returns>
        [HttpGet("declarationData")]
        [ProducesResponseType(typeof(DeclarationViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetDeclarationFormData()
        {
            _logger.LogInformation("Get declaration data");
            var result = await _healthTrackService.GetDeclarationFormData();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        ///  Submit employee self declaration
        /// </summary>
        /// <returns></returns>
        [HttpPost("declarationData")]
        [ProducesResponseType(typeof(HealthTrackViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> SubmitDeclarationFormData([FromBody] HealthTrackViewModel healthTrackViewModel)
        {
            _logger.LogInformation("Save employee declaration detail");
            var result = await _healthTrackService.CreateHealthTrack(healthTrackViewModel);
            return StatusCode((int)result.StatusCode, result);
        }


        /// <summary>
        ///  Returns declaration made by employee
        /// </summary>
        /// <returns></returns>
        [HttpGet("selfDeclaration")]
        [ProducesResponseType(typeof(List<HealthTrackViewModel>), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> getSelfDeclaration(int employeeId, string requestNumber)
        {
            _logger.LogInformation("get employee declaration detail");
            var result = await _healthTrackService.GetSelfDeclarationByEmployeeForRequest(employeeId, requestNumber);
            return StatusCode((int)result.StatusCode, result);
        }


        /// <summary>
        ///  Returns declaration made by employee
        /// </summary>
        /// <returns></returns>
        [HttpPost("declarationList")]
        [ProducesResponseType(typeof(List<HealthTrackViewModel>), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public ActionResult<IResult> GetAllDeclarations(SearchSortModel search)
        {
            _logger.LogInformation("get all declarations");
            var result = _healthTrackService.GetAllDeclarations(search);
            return StatusCode((int)result.StatusCode, result);
        }


        /// <summary>
        ///  Get Declarations
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost("declarations")]
        [ProducesResponseType(typeof(HealthTrackViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public ActionResult<IResult> GetDeclarations([FromBody] SearchSortModel search)
        {
            _logger.LogInformation("get all declarations");
            var result = _healthTrackService.GetDeclarations(search);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        ///  Get Declarations By UserId
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost("declarationsByUserId")]
        [ProducesResponseType(typeof(HealthTrackViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public ActionResult<IResult> GetDeclarationsByUserId([FromBody] SearchSortModel search)
        {
            _logger.LogInformation("get all declarations by UserId");
            var result = _healthTrackService.GetDeclarationsByUserId(search);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost("coviddeclarations")]
        [ProducesResponseType(typeof(CovidHealthTrackViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public ActionResult<IResult> GetCovidDeclarations([FromBody] SearchSortModel search)
        {
            _logger.LogInformation("get all Covid declarations");
            var result = _healthTrackService.GetCovidDeclarations(search);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="covidHealthTrackViewModel"></param>
        /// <returns></returns>
        [HttpPost("covidDeclarationData")]
        [ProducesResponseType(typeof(CovidHealthTrackViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> SubmitCovidDeclarationData([FromBody] CovidHealthTrackViewModel covidHealthTrackViewModel)
        {
            _logger.LogInformation("Save employee covid declaration detail");
            var result = await _healthTrackService.CreateCovidHealthTrack(covidHealthTrackViewModel);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declarationId"></param>
        /// <returns></returns>
        [HttpGet("covidDeclaration")]
        [ProducesResponseType(typeof(List<CovidHealthTrackViewModel>), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> getCovidDeclaration(int declarationId)
        {
            _logger.LogInformation("get employee covid declaration detail");
            var result = await _healthTrackService.GetCovidDeclaration(declarationId);
            return StatusCode((int)result.StatusCode, result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        [HttpGet("existingSelfDeclaration")]
        [ProducesResponseType(typeof(List<HealthTrackViewModel>), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetExistingSelfDeclarationOfEmployee(int employeeId)
        {
            _logger.LogInformation("get employee declaration detail");
            var result = await _healthTrackService.GetExistingSelfDeclarationOfEmployee(employeeId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
