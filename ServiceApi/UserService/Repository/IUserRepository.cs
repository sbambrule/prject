using UserService.Models;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        User RegisterUser(User user);
        bool UpdateUser(string userId, User user);
        bool DeleteUser(string userId);
        User GetUserById(string userId);
    }
}
