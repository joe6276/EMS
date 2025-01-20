using System.Security.Claims;
using EMS.Data;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Controllers
{
    public class ChatController : Controller
    {   
        private readonly IUser _user;
        private readonly ApplicationDbContext _context;
        public ChatController(IUser user, ApplicationDbContext context)
        {
            _user = user;
            _context = context;
        }
        public  async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var users = await _user.GetUsers();
            var myusers = users.FindAll(x => x.Id != currentUserId);
            return View(myusers);
        }
        public async Task<IActionResult> Test()
        {
            var users = await _user.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Chat()
        {
            var users = await _user.GetUsers();
            return View(users);
        }
        public async Task<IActionResult> GetConversation(string userId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var messages = await _context.Messages
                .Where(m =>
                    (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                    (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Json(messages);
        }



        public ActionResult Chat(string userId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var messages = _context.Messages
                   .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                            (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .ToList();

            ViewBag.RecipientId = userId;
            return View(messages);
        }
    }
}
