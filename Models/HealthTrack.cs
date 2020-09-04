using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// HealthTrack
    /// </summary>
    public class HealthTrack : BaseEntity
    {
        /// <summary>
        /// primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// ResidentialAddress
        /// </summary>
        [Required, Column(TypeName = "nvarchar(500)")]
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
        /// 
        /// </summary>
        public string RequestNumber { get; set; }

        /// <summary>
        /// Location ForeignKey
        /// </summary>
        [ForeignKey("Location")] public int LocationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Location Location { get; set; }


        /// <summary>
        /// Zone ForeignKey
        /// </summary>
        [ForeignKey("Zone")] public int ZoneId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Zone Zone { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("Employee")] public int EmployeeId { get; set; }

        /// <summary>
        /// Employee ForeignKey
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Collection HealthTrackSymptom
        /// </summary>
        public ICollection<HealthTrackSymptom> HealthTrackSymptoms { get; set; }


        /// <summary>
        /// Collection HealthTrackQuestionAnswer
        /// </summary>
        public ICollection<HealthTrackQuestionAnswer> HealthTrackQuestions { get; set; }

    }
}
