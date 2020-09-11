using HTTAPI.Enums;
using HTTAPI.Models;
using System.Collections.Generic;

namespace HTTAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class EmployeeViewModel
    {

        /// <summary>
        ///  Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeCode { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        /// Emloyee Roles
        /// </summary>
        
        public List<EmployeeRoleViewModel> Roles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CurrentResidentialAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PermanentResidentialAddress { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmployeeRegisterationEmailViewModel : EmployeeViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string HRName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LinkUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmployeePasswordResetEmailViewModel : EmployeeViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
    }

    public class EmployeeRoleViewModel
    {
        public int Id { get; set; }
        public int  RoleId { get; set; }
        public string RoleName { get; set; }
        public int EmployeeId { get; set; }
    }
}
