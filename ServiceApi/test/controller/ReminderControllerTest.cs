using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReminderService;
using ReminderService.Models;
using AuthenticationService;
using AuthenticationService.Models;
using Xunit;
using Test.InfraSetup;

namespace Test.controller
{
    [Collection("Auth API")]
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class ReminderControllerTest:IClassFixture<ReminderWebApplicationFactory<ReminderService.Startup>>
    {
        private readonly HttpClient _client, _authclient;
        public ReminderControllerTest(ReminderWebApplicationFactory<ReminderService.Startup> factory, AuthWebApplicationFactory<AuthenticationService.Startup> authFactory)
        {
            //calling Auth API to get JWT
            User user = new User { UserId = "Mukesh", Password = "admin123" };
            _authclient = authFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = _authclient.PostAsync<User>("/api/auth/login", user, formatter);
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
        public async Task GetByUserIdShouldSuccess()
        {
            // The endpoint or route of the controller action.
            string userId = "Mukesh";
            var httpResponse = await _client.GetAsync($"/api/reminder/{userId}");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var reminders = JsonConvert.DeserializeObject<IEnumerable<Reminder>>(stringResponse);
            Assert.Contains(reminders, r => r.Name == "Sports");
        }

        [Fact, TestPriority(2)]
        public async Task GetByIdShouldSuccess()
        {
            // The endpoint or route of the controller action.
            int reminderId =201;
            var httpResponse = await _client.GetAsync($"/api/reminder/{reminderId}");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var reminder = JsonConvert.DeserializeObject<Reminder>(stringResponse);
            Assert.Equal("Sports", reminder.Name);
        }

        [Fact, TestPriority(2)]
        public async Task CreateReminderShouldSuccess()
        {
            Reminder reminder = new Reminder { Name = "SMS", Description = "SMS reminder", Type = "notification", CreatedBy = "Mukesh" };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsync<Reminder>("/api/reminder", reminder, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Reminder>(stringResponse);
            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.IsAssignableFrom<Reminder>(response);
        }

        [Fact, TestPriority(4)]
        public async Task UpdateReminderShouldSuccess()
        {
            int reminderId = 201;
            Reminder reminder = new Reminder { Id = 201, Name = "Email", Description = "Mail sender", Type = "notification", CreatedBy = "Mukesh" };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync<Reminder>($"/api/reminder/{reminderId}", reminder, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(Convert.ToBoolean(stringResponse));
        }

        [Fact, TestPriority(5)]
        public async Task DeleteReminderShouldSuccess()
        {
            int reminderId = 203;
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/reminder/{reminderId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(Convert.ToBoolean(stringResponse));
        }
        #endregion positivetests

        #region negativetests
        [Fact, TestPriority(6)]
        public async Task GetByIdShouldReturnNotFound()
        {
            // The endpoint or route of the controller action.
            int reminderId = 203;
            var httpResponse = await _client.GetAsync($"/api/reminder/{reminderId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This reminder id not found", stringResponse);
        }

        [Fact, TestPriority(7)]
        public async Task UpdateReminderShouldFail()
        {
            int reminderId = 203;
            Reminder reminder = new Reminder { Id = 203, Name = "SMS", Description = "SMS sender", Type = "notification", CreatedBy = "Mukesh" };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync<Reminder>($"/api/reminder/{reminderId}", reminder, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This reminder id not found", stringResponse);
        }

        [Fact, TestPriority(8)]
        public async Task DeleteCategoryShouldFail()
        {
            int reminderId = 203;
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/reminder/{reminderId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This reminder id not found", stringResponse);
        }

        #endregion negativetests
    }
}
