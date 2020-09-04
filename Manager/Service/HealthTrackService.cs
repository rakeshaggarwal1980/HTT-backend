using HTTAPI.Enums;
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
        /// <param name="configuration"></param>      
        public HealthTrackService(ILogger<HealthTrackService> logger, IPrincipal principal,
            IHealthTrackRepository healthTrackRepository, IZoneRepository zoneRepository,
        ILocationRepository locationRepository, ISymptomRepository symptomRepository, IQuestionRepository questionRepository,
        IEmployeeRepository employeeRepository, IConfiguration configuration)
        {
            _logger = logger;
            _principal = principal as ClaimsPrincipal;
            _healthTrackRepository = healthTrackRepository;
            _questionRepository = questionRepository;
            _symptomRepository = symptomRepository;
            _locationRepository = locationRepository;
            _zoneRepository = zoneRepository;
            _employeeRepository = employeeRepository;
            _configuration = configuration;
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
                    // check if user already submitted self declaration for a request
                    var declaration = await _healthTrackRepository.GetSelfDeclarationByEmployeeForRequest(healthTrackViewModel.EmployeeId, healthTrackViewModel.RequestNumber);
                    if (declaration == null)
                    {
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
                        result.Body = healthTrackViewModel;

                    }
                    else
                    {
                        result.Status = Status.Fail;
                        result.StatusCode = HttpStatusCode.AlreadyReported;
                        result.Message = "You have already submitted self declaration";

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
            var healthViewModel = new HealthTrackViewModel();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var declaration = await _healthTrackRepository.GetSelfDeclarationByEmployeeForRequest(employeedId, requestNumber);
                if (declaration != null)
                {
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

                    result.Body = healthViewModel;
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
            var healthViewModel = new HealthTrackViewModel();
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            try
            {
                var declarations = _healthTrackRepository.GetDeclarations(search);
                if (declarations != null)
                {
                    healthViewModel.MapFromModel(declarations);
                    var employeeVm = new EmployeeViewModel();
                    declarations.ForEach(d =>
                    {   
                        if (d.Employee.Id > 0)
                        {
                            employeeVm.MapFromModel(d.Employee);
                            healthViewModel.Employee = employeeVm;
                        }
                    });



                    result.Body = healthViewModel;
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
            var hrEmployee = await _employeeRepository.GetEmployeeDetailsByRole(EmployeeRoles.HRManager.ToString());

            // send to security as well
            var securityEmp = await _employeeRepository.GetEmployeeDetailsByRole(EmployeeRoles.SecurityManager.ToString());

            appEmailHelper.ToMailAddresses.Add(new MailAddress(hrEmployee.Email, hrEmployee.Name));
            appEmailHelper.CCMailAddresses.Add(new MailAddress(securityEmp.Email, securityEmp.Name));

            appEmailHelper.MailTemplate = MailTemplate.EmployeeDeclaration;
            appEmailHelper.Subject = "Self declaration submission";
            HealthTrackEmailViewModel emailVm = new HealthTrackEmailViewModel();
            emailVm.MapFromViewModel(healthViewModel);
            emailVm.LinkUrl = $"{ _configuration["AppUrl"]}declaration/{healthViewModel.RequestNumber}/{healthViewModel.EmployeeId}";
            appEmailHelper.MailBodyViewModel = emailVm;
            await appEmailHelper.InitMailMessage();
        }

    }
}
