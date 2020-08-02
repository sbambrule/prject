using Microsoft.AspNetCore.Authorization;
using System;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;

namespace UserService.Service
{
    [Authorize]
    public class UserService : IUserService
    {
        //define a private variable to represent repository
        IUserRepository userRepository = null;
        //Use constructor Injection to inject all required dependencies.

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        //This method should be used to delete an existing user.
        public bool DeleteUser(string userId)
        {
            var result = userRepository.DeleteUser(userId);
            if (!result)
            {
                throw new UserNotFoundException("This user id does not exist");
            }
            return result;
        }
        //This method should be used to delete an existing user
        public User GetUserById(string userId)
        {
            var result = userRepository.GetUserById(userId);

            if (result == null)
            {
                throw new UserNotFoundException("This user id does not exist");
            }
            return result;
        }
        //This method is used to register a new user
        public User RegisterUser(User user)
        {
            var findUser = userRepository.GetUserById(user.UserId);

            if (findUser != null)
            {
                throw new UserNotCreatedException("This user id already exists");
            }
            var result = userRepository.RegisterUser(user);
            return result;
        }
        //This methos is used to update an existing user
        public bool UpdateUser(string userId, User user)
        {
            var result = userRepository.UpdateUser(userId, user);

            if (!result)
            {
                throw new UserNotFoundException("This user id does not exist");
            }
            return result;
        }
    }
}
