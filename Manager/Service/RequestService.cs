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

                            //List<MailUser> mailUsers = new List<MailUser>();
                            //var hrEmployee = await _employeeRepository.GetHRDetails();
                            //if (hrEmployee != null)
                            //{
                            //    MailUser user = new MailUser
                            //    {
                            //        Email = hrEmployee.Email,
                            //        Name = hrEmployee.Name
                            //    };
                            //    mailUsers.Add(user);
                            //}
                            //var emailOptions = PrepareEmailOptions(requestViewModel, MailTemplate.RequestToHR, mailUsers);
                            //if (!string.IsNullOrEmpty(emailOptions.HtmlBody))
                            //{
                            //    AppEmailHelper.SendMail(emailOptions);
                            //}
                            SendRequestEmail(requestViewModel);

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
                        // send emal to HR
                        SendRequestEmail(requestViewModel);
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

        private EmailOptions PrepareEmailOptions(ComeToOfficeRequestViewModel requestViewModel, MailTemplate template, List<MailUser> mailUsers)
        {
            var emailOptions = new EmailOptions
            {
                Template = template,
                PlainBody = string.Empty,
                Attachments = new List<Attachment>()
            };
            var msgBody = AppEmailHelper.MailBody(_hostingEnvironment, emailOptions.Template);
            if (string.IsNullOrEmpty(msgBody)) return emailOptions;
            var link = $"{ _configuration["AppUrl"]}requests";
            emailOptions.Subject = "Request for attending Office";
            emailOptions.ToMailsList = mailUsers;
            emailOptions.HtmlBody = msgBody.Replace("{RequestNumber}", requestViewModel.RequestNumber).
                Replace("{HRName}", mailUsers[0].Name).
                Replace("{DateOfRequest}", requestViewModel.DateOfRequest.ToString("dd-MM-yyyy")).
                Replace("{EmployeeName}", requestViewModel.Employee.Name).
                Replace("{link}", link).
                Replace("{EmployeeCode}", requestViewModel.Employee.EmployeeCode.ToString());
            return emailOptions;
        }

        private async void SendRequestEmail(ComeToOfficeRequestViewModel requestViewModel)
        {
            List<MailUser> mailUsers = new List<MailUser>();
            var hrEmployee = await _employeeRepository.GetHRDetails();
            if (hrEmployee != null)
            {
                MailUser user = new MailUser
                {
                    Email = hrEmployee.Email,
                    Name = hrEmployee.Name
                };
                mailUsers.Add(user);
            }
            var emailOptions = PrepareEmailOptions(requestViewModel, MailTemplate.RequestToHR, mailUsers);
            if (!string.IsNullOrEmpty(emailOptions.HtmlBody))
            {
                AppEmailHelper.SendMailExtended(_configuration, emailOptions);
            }
        }

    }
}
