
using System.ComponentModel.DataAnnotations;

namespace HTTAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// User email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
