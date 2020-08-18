using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HTTAPI.Manager.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestService : IRequestService
    {
        /// <summary>
        /// logger RequestService
        /// </summary>
        readonly ILogger<RequestService> _logger;

        /// <summary>
        /// 
        /// </summary>
        IRequestRepository _requestRepository;
        /// <summary>
        /// Claim Identity
        /// </summary>
        private readonly ClaimsPrincipal _principal;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="principal"></param>
        /// <param name="requestRepository"></param>
        public RequestService(ILogger<RequestService> logger, IPrincipal principal,
            IRequestRepository requestRepository)
        {
            _logger = logger;
            _principal = principal as ClaimsPrincipal;
            _requestRepository = requestRepository;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IResult> GetRequestsList()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = System.Net.HttpStatusCode.OK
            };
            try
            {
                var contents = await _requestRepository.GetRequestsList();
                if (contents.Count()==0) {
                    result.Message = "No records found";
                }
                result.Body = contents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Status = Enums.Status.Error;
                result.Message = ex.Message;
                result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        public async Task<IResult> CreateRequest(ComeToOfficeRequestViewModel requestViewModel)
        {
            var result = new Result
            {
                Operation = Enums.Operation.Create,
                Status = Enums.Status.Success,
                StatusCode = System.Net.HttpStatusCode.OK
            };
            try
            {
                if (requestViewModel != null)
                {
                    var requesModel = new ComeToOfficeRequest();
                    // To map employee detail
                    requesModel.MapFromViewModel(requestViewModel, (ClaimsIdentity)_principal.Identity);

                    requesModel = await _requestRepository.CreateRequest(requesModel);
                    requestViewModel.Id = requesModel.Id;
                    result.Body = requestViewModel;
                    return result;
                }
                result.Status = Enums.Status.Fail;
                result.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Message = ex.Message;
                result.Status = Status.Error;
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        public async Task<IResult> UpdateRequest(ComeToOfficeRequestViewModel requestViewModel)
        {
            var result = new Result
            {
                Operation = Enums.Operation.Update,
                Status = Enums.Status.Success,
                StatusCode = System.Net.HttpStatusCode.OK
            };
            try
            {
                if (requestViewModel != null)
                {
                    var requestModel = new ComeToOfficeRequest();
                    // To map employee detail
                    requestModel.MapFromViewModel(requestViewModel, (ClaimsIdentity)_principal.Identity);

                    requestModel = await _requestRepository.UpdateRequest(requestModel);
                    result.Body = requestViewModel;
                    return result;
                }
                result.Status = Enums.Status.Fail;
                result.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                result.Message = e.Message;
                result.Status = Status.Error;
                return result;
            }
        }
    }

}
