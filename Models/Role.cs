using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// Employee Roles
    /// </summary>
    public class Role
    {
        /// <summary>
        /// primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Location Name
        /// </summary>
        [Required, Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<EmployeeRole> EmployeeRoles { get; set; }
    }
}
