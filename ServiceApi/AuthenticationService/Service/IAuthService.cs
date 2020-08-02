using AuthenticationService.Models;

namespace AuthenticationService.Service
{
    public interface IAuthService
    {
        bool LoginUser(User user);
        bool RegisterUser(User user);
    }
}