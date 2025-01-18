namespace EMS.Services.Interfaces
{
    public interface  IEmail
    {

        Task<bool> SendEmailAsync(string token , string name, string email);
    }
}
