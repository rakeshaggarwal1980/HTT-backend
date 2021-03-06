﻿using System.Collections.Generic;

namespace HTTAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EmployeeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentResidentialAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PermanentResidentialAddress { get; set; }
        /// <summary>
        /// User Role
        /// </summary>
        public List<EmployeeRoleViewModel> Roles { get; set; }
    }
}
