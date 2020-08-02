using AuthenticationService.Models;

namespace AuthenticationService.Repository
{
    public interface IAuthRepository
    {
        bool CreateUser(User user);
        bool IsUserExists(string userId);
        bool LoginUser(User user);
    }
}