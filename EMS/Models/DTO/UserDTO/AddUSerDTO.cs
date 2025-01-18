using System.ComponentModel.DataAnnotations;

namespace EMS.Models.DTO.UserDTO
{
    public class AddUSerDTO
    {

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Position { get; set; } = string.Empty;

        [Required]
        public Department Department { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public int Salary { get; set; }
    }
}
