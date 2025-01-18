using System.ComponentModel.DataAnnotations;

namespace EMS.Models.DTO.UserDTO
{
    public enum Role
    {
        Admin,
        Manager,
        Employee
    }
    public class UserResponseDTO
    {

 
        public string Name { get; set; } = string.Empty;

   
        public string Email { get; set; } = string.Empty;

        public string Id { get; set; }
        
        public string  Role { get; set; } 
       
        public string Position { get; set; } 
    }
}
