using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EMS.Models
{

    public enum Department
    {
        IT,
        Finance,
        Marketing,
        Sales
    }

    public enum Role
    {
        Admin,
        Manager,
        Employee
    }
    public class User:IdentityUser
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Position { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public Department department { get; set; }
        [Required]
        public Role role { get; set; }
        [Required]
        public int Salary { get; set; }
    }
}
