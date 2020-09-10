using HTTAPI.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HTTAPI.Models
{
    /// <summary>
    /// Employee entity
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Primary key of entity
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required, Column(TypeName = "VARCHAR(50)")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [EmailAddress]
        [Required, Column(TypeName = "VARCHAR(100)")]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required, Column(TypeName = "VARCHAR(500)")]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int EmployeeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string CurrentResidentialAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string PermanentResidentialAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<EmployeeRole> EmployeeRoles { get; set; }

    }


}
