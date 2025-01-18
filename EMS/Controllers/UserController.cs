using EMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public UserController( ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            var users = _dbContext.Users.ToList();
            return View(users);
        }
    }
}
