using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using UserService;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UserService.Models;
using AuthenticationService;
using AuthenticationService.Models;
using System.Net.Http.Formatting;
using System.Net;
using Test.InfraSetup;

namespace Test.controller
{
    [Collection("Auth API")]
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class UserControllerTest:IClassFixture<UserWebApplicationFactory<UserService.Startup>>
    {
        private readonly HttpClient _client, _authclient;
        public UserControllerTest(UserWebApplicationFactory<UserService.Startup> factory, AuthWebApplicationFactory<AuthenticationService.Startup> authFactory)
        {
            //calling Auth API to get JWT
            AuthenticationService.Models.User user = new AuthenticationService.Models.User { UserId = "Mukesh", Password = "admin123" };
            _authclient = authFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = _authclient.PostAsync<AuthenticationService.Models.User>("/api/auth/login", user, formatter);
            httpResponse.Wait();
            // Deserialize and examine results.
            var stringResponse = httpResponse.Result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TokenModel>(stringResponse.Result);

            _client = factory.CreateClient();
            //Attaching token in request header
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.Token}");
        }

        #region positivetests
        [Fact, TestPriority(1)]
        public async Task GetByIdShouldReturnUser()
        {
            // The endpoint or route of the controller action.
            string userId = "Mukesh";
            var httpResponse = await _client.GetAsync($"/api/user/{userId}");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserService.Models.User>(stringResponse);
            Assert.Equal("Mukesh", user.Name);
        }

        [Fact, TestPriority(2)]
        public async Task RegisterUserShouldSuccess()
        {
            UserService.Models.User user = new UserService.Models.User { UserId = "Sam", Name = "Sam Gomes", Contact = "9876543210" };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsync<UserService.Models.User>("/api/user", user, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var _user = JsonConvert.DeserializeObject<UserService.Models.User>(stringResponse);

            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.NotNull(_user);
            Assert.Equal("Sam Gomes", _user.Name);
        }


        [Fact, TestPriority(3)]
        public async Task UpdateUserShouldSuccess()
        {
            string userId = "Mukesh";
            UserService.Models.User user = new UserService.Models.User { UserId = "Mukesh", Contact = "1234567890", Name = "Mukesh Goel" };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync<UserService.Models.User>($"/api/user/{userId}", user, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var _user = JsonConvert.DeserializeObject<UserService.Models.User>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.NotNull(_user);
            Assert.Equal("Mukesh Goel", _user.Name);
        }

        [Fact, TestPriority(4)]
        public async Task DeleteUserShouldSuccess()
        {
            string userId = "Sam";
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/user/{userId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(Convert.ToBoolean(stringResponse));
        }

        #endregion positivetests

        #region negativetests
        [Fact, TestPriority(5)]
        public async Task GetByIdShouldReturnNotFound()
        {
            // The endpoint or route of the controller action.
            string userId = "ABC";
            var httpResponse = await _client.GetAsync($"/api/user/{userId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This user id does not exist", stringResponse);
        }

        [Fact, TestPriority(6)]
        public async Task RegisterUserShouldFail()
        {
            UserService.Models.User user = new UserService.Models.User { UserId = "Mukesh", Name = "Mukesh Goel", Contact = "9876543210" };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsync<UserService.Models.User>("/api/user", user, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Conflict, httpResponse.StatusCode);
            Assert.Equal($"This user id already exists", stringResponse);
        }

        
        [Fact, TestPriority(7)]
        public async Task UpdateUserShouldFail()
        {
            string userId = "Sam";
            UserService.Models.User user = new UserService.Models.User { UserId = "Sam", Contact = "1234567890", Name = "Sam Kaul" };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync<UserService.Models.User>($"/api/user/{userId}", user, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This user id does not exist", stringResponse);
        }

        [Fact, TestPriority(8)]
        public async Task DeleteUserShouldFail()
        {
            string userId = "Sam";
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/user/{userId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This user id does not exist", stringResponse);
        }

        #endregion negativetests
    }
}
