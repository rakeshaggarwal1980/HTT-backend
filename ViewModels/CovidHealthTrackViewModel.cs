using HTTAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTAPI.ViewModels
{
    public class CovidHealthTrackViewModel
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Employee
        /// </summary>
        public EmployeeViewModel Employee { get; set; }

        /// <summary>
        /// Location ForeignKey
        /// </summary>
        public int LocationId { get; set; }
        
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


        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityStatus Status { get; set; }

    }


    /// <summary>
    /// 
    /// </summary>
    public class CovidHealthTrackEmailViewModel : CovidHealthTrackViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string LinkUrl { get; set; }
    }
}
