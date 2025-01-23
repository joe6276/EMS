using System.ComponentModel.DataAnnotations;
using EMS.Models.DTO.UserDTO;
using Microsoft.AspNetCore.Identity;

namespace EMS.Models
{

     public enum Department
    {
        IT ,
        Finance,
        Marketing,
        Sales
    }

   
    public class User:IdentityUser
    {
        [Key]
        public override string  Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Position { get; set; } = string.Empty;
        [Required]
        public Department department { get; set; }  
        [Required]
        public Role role { get; set; }
        [Required]
        public int Salary { get; set; }

        public string PasswordResetToken  { get; set; } = string.Empty;

        public string ConfirmationToken { get; set; } = string.Empty;

        public DateTime PasswordResetExpires { get; set; } = DateTime.Now;
    }
}
