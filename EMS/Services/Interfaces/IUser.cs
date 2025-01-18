using EMS.Models.DTO.UserDTO;

namespace EMS.Services.Interfaces
{
    public interface IUser
    {

        Task<string> RegisterUser(AddUSerDTO newUser);
        Task<LoginResponseDTO> LoginUser(LoginRequestDTO userDetails);

        Task<bool> AssignUserRole(string email, string Rolename);


    }
}
