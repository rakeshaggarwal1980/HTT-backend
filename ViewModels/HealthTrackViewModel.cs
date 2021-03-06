﻿using HTTAPI.Enums;
using HTTAPI.Models;
using System;
using System.Collections.Generic;
namespace HTTAPI.ViewModels
{
    /// <summary>
    /// Health Track View Model
    /// </summary>
    public class HealthTrackViewModel
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ResidentialAddress
        /// </summary>
        public string ResidentialAddress { get; set; }

        /// <summary>
        /// PreExistHealthIssue
        /// </summary>
        public bool PreExistHealthIssue { get; set; }

        /// <summary>
        /// ContactWithCovidPeople
        /// </summary>
        public bool ContactWithCovidPeople { get; set; }

        /// <summary>
        /// TravelOustSideInLast15Days
        /// </summary>
        public bool TravelOustSideInLast15Days { get; set; }


        /// <summary>
        /// LocationId
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// file checked out by user
        /// </summary>
        public int ZoneId { get; set; }
        /// <summary>
        /// created by user id
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EmployeeViewModel Employee { get; set; }

        /// <summary>
        ///Health Track Symptoms
        /// </summary>
        public List<HealthTrackSymptomViewModel> HealthTrackSymptoms { get; set; }

        /// <summary>
        ///Health Track Question Answer ViewModel
        /// </summary>
        public List<HealthTrackQuestionAnswerViewModel> HealthTrackQuestionAnswers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestNumber { get; set; }

        /// <summary>
        ///CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class HealthTrackEmailViewModel : HealthTrackViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string LinkUrl { get; set; }
    }
}
