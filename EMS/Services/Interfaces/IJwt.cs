using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface  IJwt
    {

        string GenerateToken(User user, string role);
    }
}
