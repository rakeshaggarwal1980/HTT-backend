using HTTAPI.Enums;

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
        /// Emloyee Role Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Role Name
        /// </summary>
        public string RoleName { get; set; }
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
}
