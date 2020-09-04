using System.ComponentModel.DataAnnotations;

namespace HTTAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
