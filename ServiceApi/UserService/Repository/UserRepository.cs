using System;
using System.Linq;
using MongoDB.Driver;
using UserService.Models;

namespace UserService.Repository
{
    public class UserRepository:IUserRepository
    {
        //define a private variable to represent UserContext
        UserContext context = null;
        public UserRepository(UserContext _context)
        {
            this.context = _context;
        }
        //This method should be used to delete an existing user.
        public bool DeleteUser(string userId)
        {
            var r = this.context.Users.DeleteMany(user => user.UserId == userId);
            return r.DeletedCount > 0 && r.IsAcknowledged;
        }

        //This method should be used to delete an existing user
        public User GetUserById(string userId)
        {
            return this.context.Users.Find<User>(x => x.UserId == userId).SingleOrDefault();
        }
        //This method is used to register a new user
        public User RegisterUser(User user)
        {
            user.AddedDate = DateTime.Now;
            this.context.Users.InsertOne(user);
            return user;
        }
        //This methos is used to update an existing user
        public bool UpdateUser(string userId, User user)
        {
            var olduser= this.context.Users.Find<User>(x => x.UserId == userId).SingleOrDefault();
            if (olduser == null)
            {
                return false;
            }
            olduser.Name = user.Name;
            olduser.Contact = user.Contact;
            return this.context.Users.ReplaceOne(u => u.UserId == userId, olduser).IsAcknowledged;
        }
    }
}
