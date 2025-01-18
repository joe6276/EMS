using System.ComponentModel.DataAnnotations;

namespace EMS.Models.DTO.UserDTO
{
    public class LoginRequestDTO
    {
        [Required]

        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
