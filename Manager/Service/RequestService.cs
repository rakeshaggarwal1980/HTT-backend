using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var requestViewModels = new List<ComeToOfficeRequestViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var requests = await _requestRepository.GetRequestsList();
                if (requests.Any())
                {
                    requestViewModels = requests.Select(t =>
                    {
                        var requestViewModel = new ComeToOfficeRequestViewModel();
                        requestViewModel.MapFromModel(t);
                        return requestViewModel;
                    }).ToList();
                }
                else
                {
                    result.Message = "No records found";
                }
                result.Body = requestViewModels;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
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
                Operation = Operation.Create,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (requestViewModel != null)
                {
                    // check if employee already has active request
                    var empRequest = _requestRepository.GetRequestByEmployee(requestViewModel.EmployeeId);
                    if (empRequest == null)
                    {
                        var requesModel = new ComeToOfficeRequest();
                        // To map employee detail
                        requesModel.MapFromViewModel(requestViewModel, (ClaimsIdentity)_principal.Identity);

                        requesModel = await _requestRepository.CreateRequest(requesModel);
                        requestViewModel.Id = requesModel.Id;
                        result.Body = requestViewModel;
                        return result;
                    }
                    else
                    {
                        result.Status = Status.Fail;
                        result.StatusCode = HttpStatusCode.NotAcceptable;
                        result.Message = "You have already raised a request";
                    }
                }
                result.Status = Status.Fail;
                result.StatusCode = HttpStatusCode.BadRequest;
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
                Operation = Operation.Update,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
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
                result.Status = Status.Fail;
                result.StatusCode = HttpStatusCode.BadRequest;
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

        /// <summary>
        /// Returns Request detail
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task<IResult> GetRequestDetail(int requestId)
        {
            var requestViewModel = new ComeToOfficeRequestViewModel();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var request = await _requestRepository.GetRequestById(requestId);
                if (request != null)
                {
                    requestViewModel.MapFromModel(request);
                }
                else
                {
                    result.Message = "No record found matching Id";
                }
                result.Body = requestViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }
    }

}
