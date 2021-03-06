﻿using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
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
    /// use in with HealthController to manage HealthTrack entity
    /// </summary>
    public class HealthTrackService : IHealthTrackService
    {
        /// <summary>
        /// logger HealthTrackService
        /// </summary>
        readonly ILogger<HealthTrackService> _logger;

        /// <summary>
        /// contract with HealthTrackRepository
        /// </summary>
        IHealthTrackRepository _healthTrackRepository;
        IZoneRepository _zoneRepository;
        ILocationRepository _locationRepository;
        ISymptomRepository _symptomRepository;
        IQuestionRepository _questionRepository;
        IEmployeeRepository _employeeRepository;
        IRequestRepository _requestRepository;
        /// <summary>
        /// Claim Identity
        /// </summary>
        private readonly ClaimsPrincipal _principal;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// HealthTrackService constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="principal"></param>
        /// <param name="healthTrackRepository"></param>
        /// <param name="zoneRepository"></param>
        /// <param name="locationRepository"></param>
        /// <param name="symptomRepository"></param>
        /// <param name="questionRepository"></param>
        /// <param name="employeeRepository"></param>
        /// <param name="requestRepository"></param>
        /// <param name="configuration"></param>      
        public HealthTrackService(ILogger<HealthTrackService> logger, IPrincipal principal,
            IHealthTrackRepository healthTrackRepository, IZoneRepository zoneRepository,
        ILocationRepository locationRepository, ISymptomRepository symptomRepository, IQuestionRepository questionRepository,
        IEmployeeRepository employeeRepository, IRequestRepository requestRepository, IConfiguration configuration)
        {
            _logger = logger;
            _principal = principal as ClaimsPrincipal;
            _healthTrackRepository = healthTrackRepository;
            _questionRepository = questionRepository;
            _symptomRepository = symptomRepository;
            _locationRepository = locationRepository;
            _zoneRepository = zoneRepository;
            _employeeRepository = employeeRepository;
            _requestRepository = requestRepository;
            _configuration = configuration;
        }


        private async Task<HealthTrackViewModel> SaveHealthTrack(HealthTrackViewModel healthTrackViewModel)
        {
            // user can add declaration
            var healthTrackModel = new HealthTrack();
            // To map health Track detail
            healthTrackModel.MapFromViewModel(healthTrackViewModel, (ClaimsIdentity)_principal.Identity);
            var healthTrackSymptoms = new List<HealthTrackSymptom>();

            if (healthTrackViewModel.HealthTrackSymptoms.Any())
            {
                // To map HealthTrack Symptoms
                healthTrackSymptoms = healthTrackViewModel.HealthTrackSymptoms.Select(t =>
                {
                    var symptom = new HealthTrackSymptom();
                    symptom.MapFromViewModel(t);
                    return symptom;
                }).ToList();
            }

            var healthTrackQuestionAnswer = new List<HealthTrackQuestionAnswer>();
            if (healthTrackViewModel.HealthTrackQuestionAnswers.Any())
            {
                healthTrackQuestionAnswer = healthTrackViewModel.HealthTrackQuestionAnswers.Select(t =>
                {
                    var questionAns = new HealthTrackQuestionAnswer();
                    questionAns.MapFromViewModel(t);
                    return questionAns;
                }).ToList();
            }

            healthTrackModel.HealthTrackSymptoms = healthTrackSymptoms;
            healthTrackModel.HealthTrackQuestions = healthTrackQuestionAnswer;

            healthTrackModel = await _healthTrackRepository.CreateHealthTrack(healthTrackModel);
            healthTrackViewModel.Id = healthTrackModel.Id;
            var employeeVm = new EmployeeViewModel();
            employeeVm.MapFromModel(healthTrackModel.Employee);
            healthTrackViewModel.Employee = employeeVm;
            // send email to HR and security
            await SendEmail(healthTrackViewModel);
            return healthTrackViewModel;
        }

        /// <summary>
        /// Create Health Track
        /// </summary>
        /// <param name="healthTrackViewModel"></param>
        /// <returns></returns>
        public async Task<IResult> CreateHealthTrack(HealthTrackViewModel healthTrackViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (healthTrackViewModel != null)
                {
                    var todayDate = DateTime.Now.Date;
                    dynamic request = null;
                    if (string.IsNullOrEmpty(healthTrackViewModel.RequestNumber))
                    {
                        // find recent approved request
                        var searchModel = new SearchSortModel();
                        searchModel.userId = healthTrackViewModel.EmployeeId;
                        searchModel.roleId = 0;
                        searchModel.SearchString = "";
                        var requests = _requestRepository.GetRequestsListByUserId(searchModel);
                        request = requests.OrderBy(x => x.FromDate)
                             .Where(x => x.IsApproved &&
                                    (x.FromDate.Date <= todayDate && x.ToDate.Date >= todayDate)).FirstOrDefault();
                        if (request != null)
                        {
                            healthTrackViewModel.RequestNumber = request.RequestNumber;
                        }
                        else
                        {
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.NotFound;
                            result.Message = "Either you do not have any approved request or you can not declare prior to the request From date";
                            return result;
                        }
                    }
                    else
                    {
                        request = await _requestRepository.GetRequestByNumber(healthTrackViewModel.RequestNumber);
                        if (request.IsDeclined)
                        {
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.NotAcceptable;
                            result.Message = "You cannot submit declaration for declined request";
                            return result;
                        }
                    }


                    // check if user has declarations for a request
                    var declarations = await _healthTrackRepository.GetSelfDeclarationByEmployeeForRequest(healthTrackViewModel.EmployeeId, healthTrackViewModel.RequestNumber);
                    if (declarations.Any())
                    {
                        // if user has already submiited declaration for today
                        if (declarations.Any(d => d.CreatedDate.Date == todayDate))
                        {
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.NotAcceptable;
                            result.Message = "You already have submitted declaration for today";
                        }
                        // if request has expired 
                        else if (todayDate > request.ToDate.Date)
                        {
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.Forbidden;
                            result.Message = "Your request has expired. Please raise a new request to submit declaration";
                        }
                        else
                        {
                            result.Body = await SaveHealthTrack(healthTrackViewModel);
                        }
                    }
                    else
                    {
                        if (todayDate <= request.ToDate.Date)
                        {
                            // now add logic to create declaration
                            result.Body = await SaveHealthTrack(healthTrackViewModel);
                        }
                        else
                        {
                            // You have to raise a new request to submit declaration
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.Forbidden;
                            result.Message = "Your request has expired. Please raise a new request to submit declaration";
                        }
                    }
                    return result;
                }
                result.Status = Status.Fail;
                result.StatusCode = HttpStatusCode.BadRequest;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
                return result;
            }

        }


        /// <summary>
        ///  Returns data to bind on declaration form
        /// </summary>
        /// <returns></returns>
        public async Task<IResult> GetDeclarationFormData()
        {
            var formViewModel = new DeclarationViewModel
            {
                Locations = new List<OptionsViewModel>(),
                Questions = new List<OptionsViewModel>(),
                Symptoms = new List<OptionsViewModel>(),
                Zones = new List<OptionsViewModel>()
            };
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var locations = await _locationRepository.GetLocations();
                if (locations.Any())
                {
                    formViewModel.Locations = locations.Select(t =>
                     {
                         var optionViewModel = new OptionsViewModel();
                         optionViewModel.MapFromModel(t);
                         return optionViewModel;
                     }).ToList();
                }

                var zones = await _zoneRepository.GetZones();
                if (zones.Any())
                {
                    formViewModel.Zones = zones.Select(t =>
                    {
                        var optionViewModel = new OptionsViewModel();
                        optionViewModel.MapFromModel(t);
                        return optionViewModel;
                    }).ToList();
                }

                var questions = await _questionRepository.GetQuestions();
                if (questions.Any())
                {
                    formViewModel.Questions = questions.Select(t =>
                    {
                        var optionViewModel = new OptionsViewModel();
                        optionViewModel.MapFromModel(t);
                        return optionViewModel;
                    }).ToList();
                }


                var symptoms = await _symptomRepository.GetSymptoms();
                if (symptoms.Any())
                {
                    formViewModel.Symptoms = symptoms.Select(t =>
                    {
                        var optionViewModel = new OptionsViewModel();
                        optionViewModel.MapFromModel(t);
                        return optionViewModel;
                    }).ToList();
                }
                result.Body = formViewModel;
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
        ///  Returns data to bind on declaration form
        /// </summary>
        /// <returns></returns>
        public async Task<IResult> GetSelfDeclarationByEmployeeForRequest(int employeedId, string requestNumber)
        {
            var healthViewModelList = new List<HealthTrackViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var declarationList = await _healthTrackRepository.GetSelfDeclarationByEmployeeForRequest(employeedId, requestNumber);
                if (declarationList.Any())
                {
                    healthViewModelList = declarationList.Select(declaration =>
                    {
                        var healthViewModel = new HealthTrackViewModel();
                        healthViewModel.MapFromModel(declaration);
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(declaration.Employee);
                        healthViewModel.Employee = employeeVm;
                        var symptoms = new List<HealthTrackSymptomViewModel>();
                        var questions = new List<HealthTrackQuestionAnswerViewModel>();
                        if (declaration.HealthTrackQuestions.Any())
                        {
                            questions = declaration.HealthTrackQuestions.Select(t =>
                            {
                                var questions = new HealthTrackQuestionAnswerViewModel();
                                questions.MapFromModel(t);
                                return questions;
                            }).ToList();
                        }

                        if (declaration.HealthTrackSymptoms.Any())
                        {
                            symptoms = declaration.HealthTrackSymptoms.Select(t =>
                            {
                                var symptom = new HealthTrackSymptomViewModel();
                                symptom.MapFromModel(t);
                                return symptom;
                            }).ToList();
                        }
                        healthViewModel.HealthTrackSymptoms = symptoms;
                        healthViewModel.HealthTrackQuestionAnswers = questions;
                        return healthViewModel;
                    }).ToList();

                    result.Body = healthViewModelList;
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "No declaration exists";
                }
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
        ///  Returns data to export in excel
        /// </summary>
        /// <returns></returns>
        public IResult GetAllDeclarations(SearchSortModel search)
        {
            var healthViewModelList = new List<HealthTrackViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var declarationList = _healthTrackRepository.GetAllDeclarations(search);
                if (declarationList.Any())
                {
                    healthViewModelList = declarationList.Select(declaration =>
                    {
                        var healthViewModel = new HealthTrackViewModel();
                        healthViewModel.MapFromModel(declaration);
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(declaration.Employee);
                        healthViewModel.Employee = employeeVm;
                        var symptoms = new List<HealthTrackSymptomViewModel>();
                        var questions = new List<HealthTrackQuestionAnswerViewModel>();
                        if (declaration.HealthTrackQuestions.Any())
                        {
                            questions = declaration.HealthTrackQuestions.Select(t =>
                            {
                                var questions = new HealthTrackQuestionAnswerViewModel();
                                questions.MapFromModel(t);
                                questions.Question = t.Question.Name;
                                return questions;
                            }).ToList();
                        }

                        if (declaration.HealthTrackSymptoms.Any())
                        {
                            symptoms = declaration.HealthTrackSymptoms.Select(t =>
                            {
                                var symptom = new HealthTrackSymptomViewModel();
                                symptom.MapFromModel(t);
                                symptom.Name = t.Symptom.Name;
                                return symptom;
                            }).ToList();
                        }
                        healthViewModel.HealthTrackSymptoms = symptoms;
                        healthViewModel.HealthTrackQuestionAnswers = questions;
                        return healthViewModel;
                    }).ToList();

                    result.Body = healthViewModelList;
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "No declaration exists";
                }
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
        /// <param name="search"></param>
        /// <returns></returns>
        public IResult GetDeclarations(SearchSortModel search)
        {
            var healthViewModelList = new List<HealthTrackViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var declarationList = _healthTrackRepository.GetDeclarations(search);
                if (declarationList.Any())
                {
                    healthViewModelList = declarationList.Select(declaration =>
                    {
                        var healthViewModel = new HealthTrackViewModel();
                        healthViewModel.MapFromModel(declaration);
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(declaration.Employee);
                        healthViewModel.Employee = employeeVm;
                        return healthViewModel;
                    }).ToList();

                    search.SearchResult = healthViewModelList;
                    result.Body = search;
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "No declaration exists";
                }
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
        /// <param name="search"></param>
        /// <returns></returns>
        public IResult GetDeclarationsByUserId(SearchSortModel search)
        {
            var healthViewModelList = new List<HealthTrackViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var declarationList = _healthTrackRepository.GetDeclarationsByUserId(search);
                if (declarationList.Any())
                {
                    healthViewModelList = declarationList.Select(declaration =>
                    {
                        var healthViewModel = new HealthTrackViewModel();
                        healthViewModel.MapFromModel(declaration);
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(declaration.Employee);
                        healthViewModel.Employee = employeeVm;
                        return healthViewModel;
                    }).ToList();

                    search.SearchResult = healthViewModelList;
                    result.Body = search;
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "No declaration exists";
                }
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
        private async Task SendEmail(HealthTrackViewModel healthViewModel)
        {
            var appEmailHelper = new AppEmailHelper();
            var hrEmployeeList = await _employeeRepository.GetEmployeeDetailsByRole(EmployeeRolesEnum.HRManager.ToString());
            if (hrEmployeeList.Count > 0)
            {
                foreach (var hrEmployee in hrEmployeeList)
                {
                    appEmailHelper.ToMailAddresses.Add(new MailAddress(hrEmployee.Email, hrEmployee.Name));
                }
            }


            // send to security as well
            var securityEmpList = await _employeeRepository.GetEmployeeDetailsByRole(EmployeeRolesEnum.SecurityManager.ToString());
            if (securityEmpList.Count > 0)
            {
                foreach (var securityEmp in securityEmpList)
                {
                    appEmailHelper.ToMailAddresses.Add(new MailAddress(securityEmp.Email, securityEmp.Name));
                }
            }
            // appEmailHelper.ToMailAddresses.Add(new MailAddress(hrEmployee.Email, hrEmployee.Name));
            // appEmailHelper.CCMailAddresses.Add(new MailAddress(securityEmp.Email, securityEmp.Name));

            appEmailHelper.MailTemplate = MailTemplate.EmployeeDeclaration;
            appEmailHelper.Subject = "Self declaration submission";
            HealthTrackEmailViewModel emailVm = new HealthTrackEmailViewModel();
            emailVm.MapFromViewModel(healthViewModel);
            emailVm.LinkUrl = $"{ _configuration["AppUrl"]}declaration/{healthViewModel.RequestNumber}/{healthViewModel.EmployeeId}";
            appEmailHelper.MailBodyViewModel = emailVm;
            await appEmailHelper.InitMailMessage();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="covidHealthTrackViewModel"></param>
        /// <returns></returns>
        public async Task<IResult> CreateCovidHealthTrack(CovidHealthTrackViewModel covidHealthTrackViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                if (covidHealthTrackViewModel != null)
                {
                    var todayDate = DateTime.Now.Date;

                    var existingDeclaration = _healthTrackRepository.GetExistingCovidDeclaration(covidHealthTrackViewModel.CovidConfirmationDate, covidHealthTrackViewModel.EmployeeId);
                    if (existingDeclaration.Result != null)
                    {
                        result.Status = Status.Fail;
                        result.StatusCode = HttpStatusCode.NotAcceptable;
                        result.Message = "You have already submitted declaration for this confirmation date.";
                        return result;
                    }
                    else
                    {
                        // now add logic to create Covid declaration
                        var covidHealthTrackModel = new CovidHealthTrack();
                        // To map health Track detail
                        covidHealthTrackModel.MapFromViewModel(covidHealthTrackViewModel, (ClaimsIdentity)_principal.Identity);
                        covidHealthTrackModel = await _healthTrackRepository.CreateCovidHealthTrack(covidHealthTrackModel);
                        covidHealthTrackViewModel.Id = covidHealthTrackModel.Id;
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(covidHealthTrackModel.Employee);
                        covidHealthTrackViewModel.Employee = employeeVm;
                        // send email to HR
                        await SendCovidEmail(covidHealthTrackViewModel);
                        result.Body = covidHealthTrackViewModel;
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
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
                return result;
            }

        }

        private async Task SendCovidEmail(CovidHealthTrackViewModel covidHealthTrackViewModel)
        {
            var appEmailHelper = new AppEmailHelper();
            var hrEmployeeList = await _employeeRepository.GetEmployeeDetailsByRole(EmployeeRolesEnum.HRManager.ToString());
            if (hrEmployeeList.Count > 0)
            {
                foreach (var hrEmployee in hrEmployeeList)
                {
                    appEmailHelper.ToMailAddresses.Add(new MailAddress(hrEmployee.Email, hrEmployee.Name));
                }
            }


            // appEmailHelper.ToMailAddresses.Add(new MailAddress(hrEmployee.Email, hrEmployee.Name));
            // appEmailHelper.CCMailAddresses.Add(new MailAddress(securityEmp.Email, securityEmp.Name));

            appEmailHelper.MailTemplate = MailTemplate.EmployeeCovidDeclaration;
            appEmailHelper.Subject = "Covid declaration submission";
            CovidHealthTrackViewModel emailVm = new CovidHealthTrackViewModel();
            emailVm.MapFromViewModel(covidHealthTrackViewModel);
            appEmailHelper.MailBodyViewModel = emailVm;
            await appEmailHelper.InitMailMessage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IResult GetCovidDeclarations(SearchSortModel search)
        {
            var covidHealthViewModelList = new List<CovidHealthTrackViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var declarationList = _healthTrackRepository.GetCovidDeclarations(search);
                if (declarationList.Any())
                {
                    covidHealthViewModelList = declarationList.Select(declaration =>
                    {
                        var healthViewModel = new CovidHealthTrackViewModel();
                        healthViewModel.MapFromModel(declaration);
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(declaration.Employee);
                        healthViewModel.Employee = employeeVm;
                        return healthViewModel;
                    }).ToList();

                    search.SearchResult = covidHealthViewModelList;
                    result.Body = search;
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "No declaration exists";
                }
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
        /// <param name="declarationId"></param>
        /// <returns></returns>
        public async Task<IResult> GetCovidDeclaration(int declarationId)
        {
            var covidHealthViewModelList = new List<CovidHealthTrackViewModel>();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var declarationList = await _healthTrackRepository.GetCovidDeclaration(declarationId);
                if (declarationList.Any())
                {
                    covidHealthViewModelList = declarationList.Select(declaration =>
                    {
                        var covidHealthViewModel = new CovidHealthTrackViewModel();
                        covidHealthViewModel.MapFromModel(declaration);
                        var employeeVm = new EmployeeViewModel();
                        employeeVm.MapFromModel(declaration.Employee);
                        covidHealthViewModel.Employee = employeeVm;
                        return covidHealthViewModel;
                    }).ToList();

                    result.Body = covidHealthViewModelList;
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "No declaration exists";
                }
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
        /// <param name="employeeId"></param>
        /// <param name="requestNumber"></param>
        /// <returns></returns>
        public async Task<IResult> GetExistingSelfDeclarationOfEmployee(int employeeId)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var todayDate = DateTime.Now.Date;
                var requests =await _requestRepository.GetRequestsByUserId(employeeId);
               var request = requests.OrderBy(x => x.FromDate)
                     .Where(x => x.IsApproved &&
                            (x.FromDate.Date <= todayDate && x.ToDate.Date >= todayDate)).FirstOrDefault();
                if (request != null)
                {
                    var declarations = await _healthTrackRepository.GetSelfDeclarationByEmployeeForRequest(employeeId, request.RequestNumber);
                    if (declarations.Any())
                    {
                        // if user has already submiited declaration for today
                        if (declarations.Any(d => d.CreatedDate.Date == todayDate))
                        {
                            result.Status = Status.Fail;
                            result.StatusCode = HttpStatusCode.NotAcceptable;
                            result.Message = "You already have submitted declaration for today";
                        }
                        else
                        {
                            result.Status = Status.Success;
                            result.StatusCode = HttpStatusCode.Accepted;
                            result.Message = "";
                        }

                    }
                }
                else
                {
                    result.Status = Status.Fail;
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = "Either you do not have any approved request or you can not declare prior to the request From date";
                    return result;
                }
               
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

