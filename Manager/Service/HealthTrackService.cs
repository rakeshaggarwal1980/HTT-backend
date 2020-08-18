using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using HTTAPI.Models;
using HTTAPI.Repository.Contracts;
using HTTAPI.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public HealthTrackService(ILogger<HealthTrackService> logger, IPrincipal principal,
            IHealthTrackRepository healthTrackRepository)
        {
            _logger = logger;
            _principal = principal as ClaimsPrincipal;
            _healthTrackRepository = healthTrackRepository;

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
                Operation = Enums.Operation.Create,
                Status = Enums.Status.Success,
                StatusCode = System.Net.HttpStatusCode.OK
            };
            if (healthTrackViewModel != null)
            {
                var healthTrackModel = new HealthTrack();
                // To map health Track detail
                healthTrackModel.MapFromViewModel(healthTrackViewModel, (ClaimsIdentity)_principal.Identity);

                var healthTrackSymptoms = new List<HealthTrackSymptom>();

                if (healthTrackViewModel.HealthTrackSymptoms.Any())
                {
                    // To map HealthTrack Symptoms
                    var rowSymptom = healthTrackViewModel.HealthTrackSymptoms.Select(t =>
                    {
                        // To map HealthTrack Symptoms one by one
                        var symptom = new HealthTrackSymptom();
                        symptom.MapFromViewModel(t, (ClaimsIdentity)_principal.Identity);
                        healthTrackSymptoms.Add(symptom);
                        return healthTrackSymptoms;

                    }).ToList();
                }

                var healthTrackQuestions = new List<HealthTrackQuestionAnswer>();
                if (healthTrackViewModel.HealthTrackQuestions.Any())
                {
                    // To map HealthTrack Questions
                    var rowQuestions = healthTrackViewModel.HealthTrackQuestions.Select(t =>
                    {
                        // To map HealthTrack HealthTrackQuestionAnswer one by one
                        var question = new HealthTrackQuestionAnswer();
                        question.MapFromViewModel(t, (ClaimsIdentity)_principal.Identity);
                        healthTrackQuestions.Add(question);
                        return healthTrackQuestions;

                    }).ToList();
                }

                healthTrackModel.HealthTrackSymptoms = healthTrackSymptoms;
                healthTrackModel.CreatedBy = "Employee";
                healthTrackModel.CreatedDate = DateTime.Now;
                healthTrackModel.HealthTrackQuestions = healthTrackQuestions;

                healthTrackModel = await _healthTrackRepository.CreateHealthTrack(healthTrackModel);
                healthTrackViewModel.Id = healthTrackModel.Id;
                result.Body = healthTrackViewModel;
                return result;
            }
            result.Status = Enums.Status.Fail;
            result.StatusCode = System.Net.HttpStatusCode.BadRequest;
            return result;
        }


    }
}
