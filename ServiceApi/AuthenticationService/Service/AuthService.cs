using AuthenticationService.Exceptions;
using AuthenticationService.Models;
using AuthenticationService.Repository;
using System;

namespace AuthenticationService.Service
{
    public class AuthService : IAuthService
    {
        //define a private variable to represent repository
        IAuthRepository authRepository;
        //Use constructor Injection to inject all required dependencies.

        public AuthService(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        //This methos should be used to register a new user
        public bool RegisterUser(User user)
        {
           var exist= authRepository.IsUserExists(user.UserId);
            if (exist)
            {
                throw new UserAlreadyExistsException($"This userId {user.UserId} already in use");
            }

            return authRepository.CreateUser(user);
        }

        //This method should be used to login for existing user
        public bool LoginUser(User user)
        {
            var exist = authRepository.LoginUser(user);
            if (!exist)
            {
                throw new UserNotExistsException($"Invalid user id or password");
            }

            return exist;
        }
    }
}
