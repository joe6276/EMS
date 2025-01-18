using System.Net.Mail;
using EMS.Services.Interfaces;
using MimeKit;
namespace EMS.Services
{
    public class EmailService : IEmail
    {
        private readonly IConfiguration _configuration;

        public EmailService( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync( string token ,string name, string email)
        {
            var myemail = _configuration.GetSection("EmailService:Email").Get<string>();
            var password = _configuration.GetSection("EmailService:Password").Get<string>();


            MimeMessage message1 = new MimeMessage();
            message1.From.Add(new MailboxAddress("Reset Email ", email));

            // Set the recipient's email address
            message1.To.Add(new MailboxAddress(name, email));

            message1.Subject = "EMS Password reset request";

            var builder = new BodyBuilder();
            var resetLink = "https://localhost:7007/User/ResetPassword?token=" + token;
            string htmlTemplate = await File.ReadAllTextAsync("Templates/email.html");
            builder.HtmlBody = htmlTemplate.Replace("{reset_link}", resetLink);

            
            message1.Body = builder.ToMessageBody();

            var client = new MailKit.Net.Smtp.SmtpClient();

            client.Connect("smtp.gmail.com", 587, false);

            client.Authenticate(myemail, password);

            await client.SendAsync(message1);

            await client.DisconnectAsync(true);

            return true;
        }
    }
}
