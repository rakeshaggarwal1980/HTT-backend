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
        ///  Form detail
        /// </summary>
        /// <returns></returns>
        [HttpGet("declarationData")]
        [ProducesResponseType(typeof(DeclarationViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetDeclarationFormData()
        {
            _logger.LogInformation("Get request detail");
            var result = await _healthTrackService.GetDeclarationFormData();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        ///  Form detail
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

    }
}
