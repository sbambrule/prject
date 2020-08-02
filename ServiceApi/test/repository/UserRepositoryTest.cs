using System;
using System.Collections.Generic;
using System.Text;
using Test;
using Test.InfraSetup;
using UserService.Models;
using UserService.Repository;
using Xunit;

namespace Repository.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class UserRepositoryTest:IClassFixture<UserDbFixture>
    {
        private readonly IUserRepository repository;

        public UserRepositoryTest(UserDbFixture _fixture)
        {
            repository = new UserRepository(_fixture.context);
        }

        [Fact, TestPriority(1)]
        public void RegisterUserShouldReturnUser()
        {
            User user = new User {UserId="Nishant", Name="Nishant", Contact="9892134560", AddedDate=DateTime.Now };

            var actual = repository.RegisterUser(user);

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<User>(actual);
        }

        [Fact, TestPriority(2)]
        public void DeleteUserShouldReturnTrue()
        {
            string userId = "Nishant";

            var actual = repository.DeleteUser(userId);
            Assert.True(actual);
        }

        [Fact, TestPriority(3)]

        public void UpdateUserShouldReturnTrue()
        {
            User user = new User { UserId = "Mukesh", Name = "Mukesh", Contact = "9822445566", AddedDate = DateTime.Now };

            var actual = repository.UpdateUser("Mukesh", user);

            Assert.True(actual);
        }

        [Fact, TestPriority(4)]
        public void GetUserByIdShouldReturnUser()
        {
            string userId = "Mukesh";

            var actual = repository.GetUserById(userId);

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<User>(actual);
        }
    }
}
