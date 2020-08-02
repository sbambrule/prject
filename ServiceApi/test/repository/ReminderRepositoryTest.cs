using System;
using System.Collections.Generic;
using ReminderService.Models;
using ReminderService.Repository;
using Test;
using Test.InfraSetup;
using Xunit;

namespace Repository.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class ReminderRepositoryTest:IClassFixture<ReminderDbFixture>
    {
        ReminderDbFixture fixture;
        private readonly ReminderRepository repository;

        public ReminderRepositoryTest(ReminderDbFixture _fixture)
        {
            this.fixture = _fixture;
            repository = new ReminderRepository(fixture.context);
        }

        [Fact, TestPriority(1)]
        public void CreateReminderShouldReturnReminder()
        {
            Reminder reminder = new Reminder { Name = "Books", CreatedBy = "Sachin", Description = "books reminder", CreationDate = new DateTime(), Type = "email" };

            var actual = repository.CreateReminder(reminder);
            Assert.IsAssignableFrom<Reminder>(actual);
            Assert.Equal(203, actual.Id);
        }

        [Fact, TestPriority(2)]
        public void DeleteReminderShouldReturnTrue()
        {
            int Id = 203;

            var actual = repository.DeleteReminder(Id);
            Assert.True(actual);
        }

        [Fact,TestPriority(3)]
        public void GetAllRemindersShouldReturnAList()
        {
            string userId = "Mukesh";
            var actual = repository.GetAllRemindersByUserId(userId);

            Assert.IsAssignableFrom<List<Reminder>>(actual);
            Assert.NotEmpty(actual);
        }

        [Fact,TestPriority(4)]
        public void GetReminderByIdShouldReturnAReminder()
        {
            var actual = repository.GetReminderById(201);

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Reminder>(actual);
            Assert.Equal("Sports", actual.Name);
        }

        [Fact, TestPriority(5)]
        public void UpdateReminderShouldReturnTrue()
        {
            Reminder reminder = new Reminder { Id = 201, Name = "Sports", CreatedBy = "Mukesh", Description = "sports reminder", CreationDate = DateTime.Now, Type = "sms" };

            var actual = repository.UpdateReminder(201, reminder);
            Assert.True(actual);
        }
        private List<Reminder> GetReminders()
        {
            List<Reminder> reminders = new List<Reminder> {
                new Reminder{Id=201, Name="Sports", CreatedBy="Mukesh", Description="sports reminder", CreationDate=new DateTime(), Type="email" },
                 new Reminder{Id=202, Name="Politics", CreatedBy="Mukesh", Description="politics reminder", CreationDate=new DateTime(),Type="email" }
            };

            return reminders;
        }
    }
}
