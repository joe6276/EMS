using System.ComponentModel.DataAnnotations;

namespace EMS.Models.DTO.UserDTO
{
    public class ForgotPassword
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; } =string.Empty;
    }
}
