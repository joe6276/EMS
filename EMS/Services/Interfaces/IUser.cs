﻿using EMS.Models;
using EMS.Models.DTO.UserDTO;

namespace EMS.Services.Interfaces
{
    public interface IUser
    {

        Task<string> RegisterUser(AddUSerDTO newUser);
        Task<LoginResponseDTO> LoginUser(LoginRequestDTO userDetails);

        Task<bool> AssignUserRole(string email, string Rolename);

        Task<bool> UpdateUserRole(string email, string Rolename);

        Task<string> ForgotPassword(string email);

        Task<string> Resetpassword(string resetToken, ResetPasswordDTO resetDetails);


        Task LogoutUser();

        Task<List<UserResponseDTO>> GetUsers();

        Task<bool> confirm(string token);

        Task<bool> deleteUser(string userId);

        Task<AddUSerDTO> GetUserId(string userId);
        
        Task<bool> UpdateUser(string userId , UpdateUserDto updateUser); 
         Task <UpdateUserDto> GetUpdateDetails(string userId);


    }
}
