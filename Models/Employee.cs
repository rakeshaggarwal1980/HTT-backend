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
        [Required, Column(TypeName = "VARCHAR(20)")]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int EmployeeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool isHRManager { get; set; }
    }


}
