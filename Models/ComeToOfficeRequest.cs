using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ComeToOfficeRequest : BaseEntity
    {
        /// <summary>
        /// Primary key of entity
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Random generated request Number
        /// </summary>
        [Required, Column(TypeName = "VARCHAR(50)")]
        public string RequestNumber { get; set; }

        /// <summary>
        ///  DateOfRequest
        /// </summary>
        [Required]
        public DateTime DateOfRequest { get; set; }


        /// <summary>
        /// IsApproved
        /// </summary>
        [Required]
        public bool IsApproved { get; set; }


        /// <summary>
        /// IsDeclined
        /// </summary>
        [Required]
        public bool IsDeclined { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HRComments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("Employee")] public int EmployeeId { get; set; }

        /// <summary>
        /// Employee ForeignKey
        /// </summary>
        public Employee Employee { get; set; }

    }
}
