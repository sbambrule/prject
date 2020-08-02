using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Models;

namespace AuthenticationService.Repository
{
    public class AuthRepository : IAuthRepository
    {
        //Define a private variable to represent AuthDbContext
        AuthDbContext dbContext;
        public AuthRepository(AuthDbContext dbContext)
        {
            this.dbContext = dbContext; 
        }

        //This methos should be used to Create a new User
        public bool CreateUser(User user)
        {
            this.dbContext.Users.Add(user);
            int count =this.dbContext.SaveChanges();
            return count > 0;
        }

        //This methos should be used to check the existence of user
        public bool IsUserExists(string userId)
        {
            return this.dbContext.Users.Count(x => x.UserId == userId)>0;
        }

        //This methos should be used to Login a user
        public bool LoginUser(User user)
        {
            return this.dbContext.Users.Count(x => x.UserId == user.UserId && x.Password==user.Password)>0;
        }
    }
}
