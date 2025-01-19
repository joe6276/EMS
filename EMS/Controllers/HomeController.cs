using System.Diagnostics;
using EMS.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUser _user;
        public HomeController(ILogger<HomeController> logger, IUser user)
        {
            _logger = logger;
            _user = user;
        }

        public  IActionResult Index()
        {
     
            return View();
        }

        public async Task<IActionResult> Logout()
        {
             await _user.LogoutUser();  // Sign the user out
            return RedirectToAction("Index", "Home"); // Redirect to home page or login page
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Homepage()
        {
            return View();
        }
    }
}
