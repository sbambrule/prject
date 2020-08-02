using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Test;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;
using Xunit;

namespace Service.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class UserServiceTest
    {
        #region positive tests

        [Fact, TestPriority(1)]
        public void RegisterUserShouldReturnUser()
        {
            User user = new User { UserId = "Nishant", Name = "Nishant", Contact = "9892134560", AddedDate = DateTime.Now };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.RegisterUser(user)).Returns(user);
            var service = new UserService.Service.UserService(mockRepo.Object);

            var actual = service.RegisterUser(user);

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<User>(actual);
        }

        [Fact, TestPriority(2)]
        public void DeleteUserShouldReturnTrue()
        {
            User user = new User { UserId = "Nishant", Name = "Nishant", Contact = "9892134560", AddedDate = DateTime.Now };
            string userId = "Nishant";
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserById(userId)).Returns(user);
            mockRepo.Setup(repo => repo.DeleteUser(userId)).Returns(true);
            var service = new UserService.Service.UserService(mockRepo.Object);

            var actual = service.DeleteUser(userId);

            Assert.True(actual);
        }

        [Fact, TestPriority(3)]
        public void UpdateUserShouldreturnTrue()
        {
            string userId = "Sachin";
            User user = new User { UserId = "Sachin", Name = "Sachin", Contact = "9822445566", AddedDate = DateTime.Now };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserById(userId)).Returns(user);
            mockRepo.Setup(repo => repo.UpdateUser(userId,user)).Returns(true);
            var service = new UserService.Service.UserService(mockRepo.Object);

            var actual = service.UpdateUser(userId,user);

            Assert.True(actual);
        }

        [Fact, TestPriority(4)]
        public void GetUserByIdShouldReturnUser()
        {
            string userId = "Mukesh";
            User user = new User { UserId = "Mukesh", Name = "Mukesh", Contact = "9822445566", AddedDate = DateTime.Now };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserById(userId)).Returns(user);
            var service = new UserService.Service.UserService(mockRepo.Object);

            var actual = service.GetUserById(userId);

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<User>(actual);
            Assert.Equal("Mukesh", actual.Name);
        }

        #endregion positive tests

        #region negative tests

        [Fact, TestPriority(5)]
        public void RegisterUserShouldThrowException()
        {
            User user = new User { UserId = "Mukesh", Name = "Mukesh", Contact = "9822445566", AddedDate = new DateTime() };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserById(user.UserId)).Returns(user);
            var service = new UserService.Service.UserService(mockRepo.Object);

            var actual = Assert.Throws<UserNotCreatedException>(() => service.RegisterUser(user));
            Assert.Equal("This user id already exists", actual.Message);

        }

        [Fact, TestPriority(6)]
        public void DeleteUserShouldThrowException()
        {
            string userId = "Dinesh";
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.DeleteUser(userId)).Returns(false);
            var service = new UserService.Service.UserService(mockRepo.Object);

            var actual = Assert.Throws<UserNotFoundException>(() => service.DeleteUser(userId));

            Assert.Equal("This user id does not exist", actual.Message);
        }

        [Fact, TestPriority(7)]
        public void UpdateUserShouldThrowException()
        {
            string userId = "Dinesh";
            User user = new User { UserId = "Dinesh", Name = "Dinesh", Contact = "9892134560", AddedDate = new DateTime() };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.UpdateUser(userId, user)).Returns(false);
            var service = new UserService.Service.UserService(mockRepo.Object);

            var actual = Assert.Throws<UserNotFoundException>(() => service.UpdateUser(userId, user));

            Assert.Equal("This user id does not exist", actual.Message);
        }

        [Fact, TestPriority(8)]
        public void GetUserByIdShouldThrowException()
        {
            string userId = "Dinesh";
            User user = null;
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserById(userId)).Returns(user);
            var service = new UserService.Service.UserService(mockRepo.Object);

            var actual = Assert.Throws<UserNotFoundException>(() => service.GetUserById(userId));

            Assert.Equal("This user id does not exist", actual.Message);
        }


        #endregion negative tests
    }
}
