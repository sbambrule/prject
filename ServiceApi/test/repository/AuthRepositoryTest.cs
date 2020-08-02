using AuthenticationService.Models;
using AuthenticationService.Repository;
using Test;
using Test.InfraSetup;
using Xunit;

namespace Repository.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class AuthRepositoryTest:IClassFixture<AuthDbFixture>
    {
        private IAuthRepository repository;
        public AuthRepositoryTest(AuthDbFixture fixture)
        {
            repository = new AuthRepository(fixture.context);
        }

        #region positivetests
        [Fact, TestPriority(1)]
        public void CreateUserShouldSuccess()
        {
            User user = new User { UserId = "Sachin", Password = "admin123" };

            var actual = repository.CreateUser(user);
            Assert.True(actual);

            var isExists = repository.IsUserExists(user.UserId);
            Assert.True(isExists);
        }

        [Fact, TestPriority(2)]
        public void LoginUserShouldSuccess()
        {
            User user = new User { UserId = "Mukesh", Password = "admin123" };

            var actual = repository.LoginUser(user);
            Assert.True(actual);
        }
        #endregion positivetests

        #region negativetests
        [Fact, TestPriority(3)]
        public void LoginUserShouldFail()
        {
            User user = new User { UserId = "John", Password = "admin123" };

            var actual = repository.LoginUser(user);
            Assert.False(actual);
        }
        #endregion negativetests
    }
}
