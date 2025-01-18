using System.ComponentModel.DataAnnotations;

namespace EMS.Models.DTO.UserDTO
{
    public class AddUSerDTO
    {

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]

        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Position { get; set; } = string.Empty;

        [Required]
        public Department department { get; set; }
        [Required]
        public Role role { get; set; }
        [Required]
        public int Salary { get; set; }
    }
}
