using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Service
{
    public interface IUserService
    {
        User RegisterUser(User user);
        bool UpdateUser(string userId, User user);
        bool DeleteUser(string userId);
        User GetUserById(string userId);
    }
}
