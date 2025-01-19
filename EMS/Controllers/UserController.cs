﻿using System.Security.Claims;
using Azure;
using EMS.Data;
using EMS.Extensions;
using EMS.Models.DTO.UserDTO;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
       
        public async Task<IActionResult> Index()
        {

            var users = await _user.GetUsers();
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
            if (string.IsNullOrEmpty(response.Name))
            {
                ModelState.AddModelError("", "Invalid Credentials");
            }
            return RedirectToAction("Homepage", "Home");
        }



      

      
        public async Task<IActionResult> Confirm()
        {
            var token = Request.Query["token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("", "Error Occured");
            }
            var res = await _user.confirm(token);


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


        [HttpPost]
        public async Task<IActionResult> UpdateRole(string email, string role)
        {
            var result = await _user.UpdateUserRole(email, role);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Error updating role" });
            }
        }



        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var result = await _user.deleteUser(Id);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Error updating role" });
            }
        }


        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user= await _user.GetUserId(userId);
          
            return View(user);
        }


        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _user.GetUpdateDetails(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserDto addUSerDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                     var response = await _user.UpdateUser(userId, addUSerDTO);
            return RedirectToAction("index");

        }
    }
}
