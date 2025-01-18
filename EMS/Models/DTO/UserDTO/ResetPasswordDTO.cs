using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace EMS.Models.DTO.UserDTO
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password  mismatch")]
        public string ConfirmPassword { get; set; }
    }
}
