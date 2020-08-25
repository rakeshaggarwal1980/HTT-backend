using HTTAPI.Models;
using System;

namespace HTTAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ComeToOfficeRequestViewModel
    {
        /// <summary>
        /// Primary key of entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Random generated request Number
        /// </summary>
        public string RequestNumber { get; set; }


        /// <summary>
        ///  DateOfRequest
        /// </summary>
        public DateTime DateOfRequest { get; set; }


        /// <summary>
        /// IsApproved
        /// </summary>
        public bool IsApproved { get; set; }


        /// <summary>
        /// IsDeclined
        /// </summary>
        public bool IsDeclined { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HRComments { get; set; }
    }
}
