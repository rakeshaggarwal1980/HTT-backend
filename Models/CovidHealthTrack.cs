using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HTTAPI.Models
{
    /// <summary>
    /// CovidHealthTrack
    /// </summary>
    public class CovidHealthTrack : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        /// <summary>
        /// Employee ForeignKey
        /// </summary>
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Employee
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Location ForeignKey
        /// </summary>
        [ForeignKey("Location")] public int LocationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Location Location { get; set; }


        /// <summary>
        /// DateOfSymptoms
        /// </summary>
        public DateTime DateOfSymptoms { get; set; }


        /// <summary>
        /// OfficeLastDay
        /// </summary>
        public DateTime OfficeLastDay { get; set; }


        /// <summary>
        /// CovidConfirmationDate
        /// </summary>
        public DateTime CovidConfirmationDate { get; set; }
        /// <summary>
        /// OthersInfectedInFamily
        /// </summary>
        public Boolean OthersInfectedInFamily { get; set; }

        /// <summary>
        /// FamilyMembersCount
        /// </summary>
        public int FamilyMembersCount { get; set; }


        /// <summary>
        /// HospitalizationNeed
        /// </summary>
        public Boolean HospitalizationNeed { get; set; }

    }
}
