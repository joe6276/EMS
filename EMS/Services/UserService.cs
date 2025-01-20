using System.Security.Cryptography;
using AutoMapper;
using Azure;
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
        private readonly IEmail _email;
        private readonly SignInManager<User> _signInManager;
        public UserService(ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IEmail mail, SignInManager<User> signInManager)
        {
            //Initialize
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
            _email = mail;
            _signInManager = signInManager;
        }
        public async  Task<bool> AssignUserRole(string email, string Rolename)
        {
            // Get User
        
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

                   var addRoleResult= await _userManager.AddToRoleAsync(user, Rolename);

                if (addRoleResult.Succeeded)
                {
                    return true;
                }
                }
                return false;
                  
        }

        public async Task<bool> UpdateUserRole(string email, string newRoleName)
        {
            // Get the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            // If user exists
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                if (!currentRoles.Contains(newRoleName))
                {
                    var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeRoleResult.Succeeded)
                    {
                        
                        return false;
                    }

                    var addRoleResult = await _userManager.AddToRoleAsync(user, newRoleName);
                    if (addRoleResult.Succeeded)
                    {
                        return true;
                    }
                }
            }

            // Return false if user doesn't exist or role update failed
            return false;
        }


        public async Task<string> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Trim() == email.Trim());

            if (user == null)
            {
                return "User Does Not Exist";
            }

            byte[] randomBytes = new byte[24];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert bytes to a hex string 
            var passwordResetToken = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
            var passwordResetExpires = DateTime.Now.AddHours(1); // after one Hour 

            user.PasswordResetToken=passwordResetToken;
            user.PasswordResetExpires = passwordResetExpires;   

            _context.Update(user);
            _context.SaveChanges();
            // Send Email 
            var res = await _email.SendEmailAsync( passwordResetToken, user.Name, user.Email);

            if (res)
            {
                return "";
            }
            return "Eror Occured";
        }

        public async Task<List<UserResponseDTO>> GetUsers()
        {
            var users = await _context.Users.ToListAsync(); // Fetch all users asynchronously

            var userResponses = new List<UserResponseDTO>();

            foreach (var user in users)
            {
                // Fetch roles asynchronously for each user
                var roles = await _userManager.GetRolesAsync(user);

                userResponses.Add(new UserResponseDTO
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Email = user.Email,
                    Position = user.Position,
                    Role = roles.First() // Convert the roles to a list or any suitable representation
                });
            }

            return userResponses;

        }

        public async Task<LoginResponseDTO> LoginUser(LoginRequestDTO userDetails)
        {
            //get User
            try
            {


                var users = await _context.Users.ToListAsync();
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Trim() == userDetails.Username.Trim());
                //Check password
                var isValid = await _userManager.CheckPasswordAsync(user, userDetails.Password);

                //if one is wrong
                if (!isValid || user==null || !string.IsNullOrWhiteSpace(user.ConfirmationToken))
                {
                    return new LoginResponseDTO();
                }

                //else return a token 
                var roles = await _userManager.GetRolesAsync(user);
                var loggedUser = _mapper.Map<LoginResponseDTO>(user);
                loggedUser.Role = roles.First();


                //sign in user 
                await _signInManager.SignInAsync(user, isPersistent: false);

                return loggedUser;
            }catch (Exception ex)
            {
                return new LoginResponseDTO();
            }
        }

        public async Task LogoutUser()
        {
          await _signInManager.SignOutAsync();
        }

        public async Task<bool> confirm(string token)
        {
            var user= await _context.Users.FirstOrDefaultAsync( x=>x.ConfirmationToken == token);

            if(user == null)
            {
                return false;
            }

            user.ConfirmationToken = "";
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        public async Task<string> RegisterUser(AddUSerDTO newUser)
        {
            
            var user = _mapper.Map<AddUSerDTO, User>(newUser);
            user.UserName = user.Email;
            user.Id = Guid.NewGuid().ToString();

            try
            {
                //Create user

                byte[] randomBytes = new byte[24];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }

                // Convert bytes to a hex string 
                var confirmationToken = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
                user.ConfirmationToken = confirmationToken;

               var result= await _userManager.CreateAsync(user, newUser.Password);
                //Assign Role
                await AssignUserRole(user.Email, newUser.Role.ToString());
                // Send Confirmation  Email

                await _email.sendConfirmationEmail(confirmationToken, user.Name, user.Email);

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

        public async Task<string> Resetpassword(string resetToken , ResetPasswordDTO resetDetails)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == resetToken);

            if(user== null)
            {
                return "Error Occured, Invalid Token ";
            }

            if(DateTime.Now > user.PasswordResetExpires)
            {
                return "Token Has Expired";
            }
            user.PasswordResetExpires = DateTime.Now;
            user.PasswordResetToken = "";
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // reset Password 
            await _userManager.ResetPasswordAsync(user ,token, resetDetails.Password);

          

            return "";
        }

        public async Task<bool> deleteUser(string userId)
        {   
       
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public async Task<AddUSerDTO> GetUserId(string userId)
        {
            var userDetails = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var user = _mapper.Map<AddUSerDTO>(userDetails);
            return user;
        }

        public async Task<bool> UpdateUser(string userId, UpdateUserDto updateUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var updated = _mapper.Map(updateUser, existingUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UpdateUserDto> GetUpdateDetails(string userId)
        {
            var userDetails = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var user = _mapper.Map<UpdateUserDto>(userDetails);
            return user;
        }
    }
}
