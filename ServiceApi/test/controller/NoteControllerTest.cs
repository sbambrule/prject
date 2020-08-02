using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NoteService;
using AuthenticationService;
using NoteService.Models;
using Xunit;
using AuthenticationService.Models;
using Test.InfraSetup;

namespace Test.controller
{
    [Collection("Auth API")]
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class NoteControllerTest : IClassFixture<NoteWebApplicationFactory<NoteService.Startup>>
    {
        private readonly HttpClient _client, _authclient;
        public NoteControllerTest(NoteWebApplicationFactory<NoteService.Startup> factory, AuthWebApplicationFactory<AuthenticationService.Startup> authFactory)
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
            var httpResponse = await _client.GetAsync($"/api/notes/{userId}");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<IEnumerable<Note>>(stringResponse);
            Assert.Contains(notes, n => n.Title == "Sample");
        }

        [Fact, TestPriority(2)]
        public async Task CreateNoteShouldSuccess()
        {
            string userId = "Mukesh";
            Note note = new Note
            {
                Category = new Category { Id = 101, Name = "Sports", CreatedBy = "Sachin", Description = "All about sports", CreationDate = DateTime.Now },
                Content = "Sample Note",
                CreatedBy = "Sachin",
                Reminders = new List<Reminder> { new Reminder { Id = 201, Name = "Sports", CreatedBy = "Sachin", Description = "sports reminder", CreationDate = DateTime.Now, Type = "email" } },
                Title = "Sample",
                Id= 101,
                CreationDate = DateTime.Now
            };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsync<Note>($"/api/notes/{userId}", note, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Note>(stringResponse);
            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.IsAssignableFrom<Note>(response);
        }

        [Fact, TestPriority(4)]
        public async Task UpdateNoteShouldSuccess()
        {
            string userId = "Mukesh";
            int noteId = 101;
            Note note = new Note
            {
                Id = 101,
                Category = new Category { Id = 101, Name = "Sports", CreatedBy = "Mukesh", Description = "All about sports", CreationDate = DateTime.Now },
                Content = "Note-1",
                CreatedBy = "Mukesh",
                Reminders = new List<Reminder> { new Reminder { Id = 201, Name = "Sports", CreatedBy = "Mukesh", Description = "sports reminder", CreationDate = DateTime.Now, Type = "email" } },
                Title = "Sample",
                CreationDate = DateTime.Now
            };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync<Note>($"/api/notes/{userId}/{noteId}", note, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            var response = JsonConvert.DeserializeObject<Note>(stringResponse);
            Assert.IsAssignableFrom<Note>(response);
            Assert.Equal("Note-1", response.Content);
        }

        [Fact, TestPriority(3)]
        public async Task DeleteNoteShouldSuccess()
        {
            string userId = "Sachin";
            int noteId = 101;
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/notes/{userId}/{noteId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(Convert.ToBoolean(stringResponse));
        }

        #endregion positivetests

        #region negativetests

        [Fact, TestPriority(5)]
        public async Task UpdateNoteShouldFailInvalidNoteId()
        {
            string userId = "Mukesh";
            int noteId = 103;
            Note note = new Note
            {
                Id = 103,
                Category = new Category { Id = 101, Name = "Sports", CreatedBy = "Mukesh", Description = "All about sports", CreationDate = DateTime.Now },
                Content = "Note-1",
                CreatedBy = "Mukesh",
                Reminders = new List<Reminder> { new Reminder { Id = 201, Name = "Sports", CreatedBy = "Mukesh", Description = "sports reminder", CreationDate = DateTime.Now, Type = "email" } },
                Title = "Sample",
                CreationDate = DateTime.Now
            };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync<Note>($"/api/notes/{userId}/{noteId}", note, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"NoteId {noteId} for user {userId} does not exist", stringResponse);
        }

        [Fact, TestPriority(6)]
        public async Task DeleteNoteShouldFail()
        {
            string userId = "Sachin";
            int noteId = 103;
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/notes/{userId}/{noteId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"NoteId {noteId} for user {userId} does not exist", stringResponse);
        }
        #endregion negativetests
    }
}
