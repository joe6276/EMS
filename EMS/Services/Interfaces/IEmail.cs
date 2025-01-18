namespace EMS.Services.Interfaces
{
    public interface  IEmail
    {

        Task<bool> SendEmailAsync(string token , string name, string email);

        Task<bool> sendConfirmationEmail(string confirmToken,  string name, string email);
    }
}
