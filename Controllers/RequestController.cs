using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HTTAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/request")]
    [ApiController]
    public class RequestController : Controller
    {
        private readonly ILogger<RequestController> _logger;

        IRequestService _requestService;
        /// <summary>
        /// 
        /// </summary>
        public RequestController(ILogger<RequestController> logger, IRequestService requestService)
        {
            _logger = logger;
            _requestService = requestService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("request")]
        [ProducesResponseType(typeof(ComeToOfficeRequestViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> CreateRequest([FromBody] ComeToOfficeRequestViewModel requestiewModel)
        {
            _logger.LogInformation("Creating Request");
            var result = await _requestService.CreateRequest(requestiewModel);
            return StatusCode((int)result.StatusCode, result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut("request")]
        [ProducesResponseType(typeof(ComeToOfficeRequestViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> UpdateRequest([FromBody] ComeToOfficeRequestViewModel requestiewModel)
        {
            _logger.LogInformation("Updating Request");
            var result = await _requestService.UpdateRequest(requestiewModel);
            return StatusCode((int)result.StatusCode, result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("requests")]
        [ProducesResponseType(typeof(List<ComeToOfficeRequestViewModel>), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetAllRequests()
        {
            _logger.LogInformation("Get all requests");
            var result = await _requestService.GetRequestsList();
            return StatusCode((int)result.StatusCode, result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("requests/{userId}")]
        [ProducesResponseType(typeof(List<ComeToOfficeRequestViewModel>), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetAllRequestsByUserId(int userId)
        {
            _logger.LogInformation("Get all requests by userId");
            var result = await _requestService.GetRequestsListByUserId(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        ///  Returns request detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ComeToOfficeRequestViewModel), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IResult>> GetRequestDetail(int id)
        {
            _logger.LogInformation("Get request detail");
            var result = await _requestService.GetRequestDetail(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
