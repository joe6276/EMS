
using System.Security.Claims;
using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using Org.BouncyCastle.Cms;

namespace EMS.Hubs
{
    public class ChatHub:Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SendMessage( string receiverId, string content)
        {

            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; ;
            var message = new Message
            {
                SenderId = userId,
                ReceiverId = receiverId,
                Text = content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Send to specific user
            await Clients.User(receiverId).SendAsync("ReceiveMessage", new
            {
                senderId = userId,
                message = content,
                timestamp = DateTime.UtcNow
            });
            

            //await Clients.User(receiverId).SendAsync(userId, content);


           
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task MarkAsRead(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
                await Clients.User(message.SenderId).SendAsync("MessageRead", messageId);
            }
        }


    }
}
