using AutoMapper;
using EMS.Data;
using EMS.Models;
using EMS.Models.DTO.UserDTO;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS.Services
{
    public class UserService : IUser
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwt _jwt;

        public UserService( ApplicationDbContext context , IMapper mapper , RoleManager<IdentityRole> roleManager , UserManager<User> userManager, IJwt jwt)
        {   
            //Initialize
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
            _jwt = jwt;
        }

        public async  Task<bool> AssignUserRole(string email, string Rolename)
        {
            // Get User
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                // If user Exist
                if (user != null)
                {
                    //Check if Role Exists or Not 
                    if (!_roleManager.RoleExistsAsync(Rolename).GetAwaiter().GetResult())
                    {
                        //first create it
                        _roleManager.CreateAsync(new IdentityRole(Rolename)).GetAwaiter().GetResult();
                    }

                    //assign role if it exist

                    await _userManager.AddToRoleAsync(user, Rolename);

                    return true;
                }
            }catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public async Task<LoginResponseDTO> LoginUser(LoginRequestDTO userDetails)
        {
            //get User
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userDetails.Username);
                //Check password
                var isValid = await _userManager.CheckPasswordAsync(user, userDetails.Password);

                //if one is wrong
                if (!isValid || user == null)
                {
                    return new LoginResponseDTO();
                }

                //else return a token 
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwt.GenerateToken(user, roles[0]);
                var loggedUser = _mapper.Map<LoginResponseDTO>(user);
                loggedUser.Token = token;

                return loggedUser;
            }catch (Exception ex)
            {
                return new LoginResponseDTO();
            }
        }

        public async Task<string> RegisterUser(AddUSerDTO newUser)
        {
            
            var user = _mapper.Map<User>(newUser);

            try
            {
                //Create user
                var result= await _userManager.CreateAsync(user, newUser.Password);

                if(result.Succeeded) 
                    {
                    //User was created
                    return "";

                    }
                else
                {   

                    //return the Error
                    return result.Errors.FirstOrDefault().Description;
                }
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
