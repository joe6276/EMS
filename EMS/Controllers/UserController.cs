using Azure;
using EMS.Data;
using EMS.Models.DTO.UserDTO;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMS.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUser _user;
        public UserController( ApplicationDbContext context, IUser user)
        {
            _dbContext = context;
            _user = user;
        }
        public IActionResult Index()
        {
            var users = _dbContext.Users.ToList();
            return View(users);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> Login(LoginRequestDTO user)
        {

            var response = await _user.LoginUser(user);
            if (string.IsNullOrEmpty(response.Token))
            {
                ModelState.AddModelError("", "Invalid Credentials");
            }

            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AddUSerDTO newUser)
        {
           var response= await _user.RegisterUser(newUser);
            if (!string.IsNullOrEmpty(response))
            {
                ModelState.AddModelError("", response);
            }

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            var response = await _user.ForgotPassword(forgotPassword.Email);
            if (!string.IsNullOrEmpty(response))
            {
                ModelState.AddModelError("",response);
            }
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> ResetPassword( ResetPasswordDTO resetDetails)
        {
            var token = Request.Query["token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("", "Error Occured");
            }
            var res = await _user.Resetpassword(token, resetDetails);
            if (!string.IsNullOrWhiteSpace(res))
            {
                ModelState.AddModelError("", res);
            }

            return View();
        }
    }
}
