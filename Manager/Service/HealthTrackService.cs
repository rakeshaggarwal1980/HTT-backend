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

        /// <summary>
        /// Claim Identity
        /// </summary>
        private readonly ClaimsPrincipal _principal;

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
        public HealthTrackService(ILogger<HealthTrackService> logger, IPrincipal principal,
            IHealthTrackRepository healthTrackRepository, IZoneRepository zoneRepository,
        ILocationRepository locationRepository, ISymptomRepository symptomRepository, IQuestionRepository questionRepository)
        {
            _logger = logger;
            _principal = principal as ClaimsPrincipal;
            _healthTrackRepository = healthTrackRepository;
            _questionRepository = questionRepository;
            _symptomRepository = symptomRepository;
            _locationRepository = locationRepository;
            _zoneRepository = zoneRepository;

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

    }
}
