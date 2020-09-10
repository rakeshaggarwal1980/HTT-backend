using HTTAPI.Enums;

namespace HTTAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSignUpViewModel
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
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeCode { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentResidentialAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PermanentResidentialAddress { get; set; }
    }
}
