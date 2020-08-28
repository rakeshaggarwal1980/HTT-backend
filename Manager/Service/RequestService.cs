using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        IEmployeeRepository _employeeRepository;
        /// <summary>
        /// Claim Identity
        /// </summary>
        private readonly ClaimsPrincipal _principal;

        /// <summary>
        /// 
        /// </summary>
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        public AppEmailHelper appEmailHelper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="principal"></param>
        /// <param name="requestRepository"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="configuration"></param>
        /// <param name="employeeRepository"></param>

        public RequestService(ILogger<RequestService> logger, IPrincipal principal,
            IRequestRepository requestRepository, IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration,
            IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _principal = principal as ClaimsPrincipal;
            _requestRepository = requestRepository;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
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
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(t.Employee);
                        requestViewModel.Employee = employeeVm;

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
                var requesModel = new ComeToOfficeRequest();
                if (requestViewModel != null)
                {
                    // check if employee already has active request for current and next days
                    var empRequests = await _requestRepository.GetRequestsByEmployee(requestViewModel.EmployeeId);
                    if (empRequests.Count() > 0)
                    {
                        //  if request for the same date exists and is approved , then can not raise new request
                        if (empRequests.Where(x => x.DateOfRequest.Date == requestViewModel.DateOfRequest.Date && x.IsApproved).Any())
                        {
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.NotAcceptable;
                            result.Message = "You already have approval for this date";
                            return result;
                        }
                        //  if request for the same date exists and is declined , then can raise new request
                        //  and if no request matching date exists
                        else if (empRequests.Where(x => x.DateOfRequest.Date == requestViewModel.DateOfRequest.Date && x.IsDeclined).Any()
                            || empRequests.Where(x => x.DateOfRequest.Date != requestViewModel.DateOfRequest.Date).Any())
                        {

                            requesModel.MapFromViewModel(requestViewModel, (ClaimsIdentity)_principal.Identity);
                            requesModel = await _requestRepository.CreateRequest(requesModel);
                            requestViewModel.Id = requesModel.Id;
                            var employeeVm = new EmployeeViewModel();
                            employeeVm.MapFromModel(requesModel.Employee);
                            requestViewModel.Employee = employeeVm;
                            // send email to HR
                            await SendRequestEmail(requestViewModel, MailTemplate.RequestToHR);

                            result.Body = requestViewModel;
                            return result;
                        }
                        else if (empRequests.Where(x => x.DateOfRequest.Date == requestViewModel.DateOfRequest.Date && !x.IsDeclined && !x.IsApproved).Any())
                        {
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.NotAcceptable;
                            result.Message = "You already have request pending for approval for this date";
                            return result;
                        }
                    }
                    else
                    {
                        // To map employee detail
                        requesModel.MapFromViewModel(requestViewModel, (ClaimsIdentity)_principal.Identity);
                        requesModel = await _requestRepository.CreateRequest(requesModel);
                        requestViewModel.Id = requesModel.Id;
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(requesModel.Employee);
                        requestViewModel.Employee = employeeVm;
                        // send email to HR
                        await SendRequestEmail(requestViewModel, MailTemplate.RequestToHR);
                        result.Body = requestViewModel;
                        return result;
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

                    var employeeVm = new EmployeeViewModel();
                    employeeVm.MapFromModel(requestModel.Employee);
                    requestViewModel.Employee = employeeVm;

                    // send HR response in email to employee and send link for declaration form to Employee if approved
                    await SendResponseEmail(requestViewModel, MailTemplate.ResponseFromHR);
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
                    var employeeVm = new EmployeeViewModel();
                    employeeVm.MapFromModel(request.Employee);
                    requestViewModel.Employee = employeeVm;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <param name="mailTemplate"></param>
        private async Task SendResponseEmail(ComeToOfficeRequestViewModel requestViewModel, MailTemplate mailTemplate)
        {
            appEmailHelper = new AppEmailHelper();
            var hrEmployee = await _employeeRepository.GetHRDetails();
            if (hrEmployee != null)
            {
                appEmailHelper.FromMailAddress = new MailAddress(hrEmployee.Email, hrEmployee.Name);
            }
            appEmailHelper.ToMailAddresses.Add(new MailAddress(requestViewModel.Employee.Email, requestViewModel.Employee.Name));
            appEmailHelper.MailTemplate = mailTemplate;
            ComeToOfficeRequestEmailViewModel emailVm = new ComeToOfficeRequestEmailViewModel();
            emailVm.MapFromViewModel(requestViewModel);
            if (requestViewModel.IsApproved)
            {
                emailVm.Status = "Approved";
                appEmailHelper.Subject = "Your request is approved by HR";
            }
            else if (requestViewModel.IsDeclined)
            {
                emailVm.Status = "Declined";
                appEmailHelper.Subject = "Your request is declined by HR";
            }

            emailVm.LinkUrl = $"{ _configuration["AppUrl"]}declaration/{requestViewModel.RequestNumber}";
            emailVm.HRName = hrEmployee.Name;
            appEmailHelper.MailBodyViewModel = emailVm;
            await appEmailHelper.InitMailMessage();
        }

        private async Task SendRequestEmail(ComeToOfficeRequestViewModel requestViewModel, MailTemplate mailTemplate)
        {
            appEmailHelper = new AppEmailHelper();
            var hrEmployee = await _employeeRepository.GetHRDetails();
            appEmailHelper.ToMailAddresses.Add(new MailAddress(hrEmployee.Email, hrEmployee.Name));
            appEmailHelper.CCMailAddresses.Add(new MailAddress(requestViewModel.Employee.Email, requestViewModel.Employee.Name));
            appEmailHelper.MailTemplate = mailTemplate;
            appEmailHelper.Subject = "Request for attending Office";
            ComeToOfficeRequestEmailViewModel emailVm = new ComeToOfficeRequestEmailViewModel();
            emailVm.MapFromViewModel(requestViewModel);
            emailVm.LinkUrl = $"{ _configuration["AppUrl"]}requests";
            emailVm.HRName = hrEmployee.Name;
            appEmailHelper.MailBodyViewModel = emailVm;
            await appEmailHelper.InitMailMessage();
        }

    }
}
