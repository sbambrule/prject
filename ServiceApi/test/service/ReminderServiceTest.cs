using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ReminderService.Models;
using ReminderService.Repository;
using ReminderService.Exceptions;
using Test;

namespace Service.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class ReminderServiceTest
    {
        #region positive tests
        [Fact, TestPriority(1)]
        public void CreateReminderShouldReturnReminder()
        {
            var mockRepo = new Mock<IReminderRepository>();
            Reminder reminder = new Reminder {Id=203, Name = "Books", CreatedBy = "Mukesh", Description = "books reminder", CreationDate = new DateTime(), Type = "email" };
            mockRepo.Setup(repo => repo.GetAllRemindersByUserId("Mukesh")).Returns(this.GetReminders());
            mockRepo.Setup(repo => repo.CreateReminder(reminder)).Returns(reminder);
            var service = new ReminderService.Service.ReminderService(mockRepo.Object);


            var actual = service.CreateReminder(reminder);
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Reminder>(actual);
        }

        [Fact,TestPriority(2)]
        public void DeleteReminderShouldReturnTrue()
        {
            var mockRepo = new Mock<IReminderRepository>();
            int Id = 203;
            mockRepo.Setup(repo => repo.DeleteReminder(Id)).Returns(true);
            var service = new ReminderService.Service.ReminderService(mockRepo.Object);

            var actual = service.DeleteReminder(Id);

            Assert.True(actual);
        }
        //[Fact,TestPriority(3)]
        //public void GetAllRemindersShouldReturnAList()
        //{
        //    var mockRepo = new Mock<IReminderRepository>();
        //    mockRepo.Setup(repo => repo.GetAllReminders()).Returns(this.GetReminders());
        //    var service = new Service.ReminderService(mockRepo.Object);

        //    var actual = service.GetAllReminders();

        //    Assert.IsAssignableFrom<List<Reminder>>(actual);
        //    Assert.NotEmpty(actual);
        //}

        [Fact, TestPriority(4)]
        public void GetReminderByIdShouldReturnAReminder()
        {
            int Id = 201;
            Reminder reminder = new Reminder { Id = 201, Name = "Sports", CreatedBy = "Mukesh", Description = "sports reminder", CreationDate = new DateTime(), Type = "email" };
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.GetReminderById(Id)).Returns(reminder);
            var service = new ReminderService.Service.ReminderService(mockRepo.Object);

            var actual = service.GetReminderById(Id);

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Reminder>(actual);
        }

        [Fact, TestPriority(5)]
        public void UpdateReminderShouldReturnTrue()
        {
            int Id = 201;
            Reminder reminder = new Reminder { Id = 201, Name = "Sports", CreatedBy = "Mukesh", Description = "sports reminder", CreationDate = new DateTime(), Type = "sms" };
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.GetReminderById(201)).Returns(reminder);
            mockRepo.Setup(repo => repo.UpdateReminder(Id,reminder)).Returns(true);
            var service = new ReminderService.Service.ReminderService(mockRepo.Object);


            var actual = service.UpdateReminder(Id, reminder);
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

        #endregion positive tests

        #region negative tests

        [Fact, TestPriority(6)]
        public void CreateReminderShouldThrowException()
        {
            var mockRepo = new Mock<IReminderRepository>();
            Reminder reminder = new Reminder { Id = 201, Name = "Sports", CreatedBy = "Mukesh", Description = "sports reminder", CreationDate = new DateTime(), Type = "email" };
            mockRepo.Setup(repo => repo.GetAllRemindersByUserId("Mukesh")).Returns(this.GetReminders());
            mockRepo.Setup(repo => repo.GetReminderById(201)).Returns(reminder);
            var service = new ReminderService.Service.ReminderService(mockRepo.Object);


            var actual = Assert.Throws<ReminderNotCreatedException>(() => service.CreateReminder(reminder));
            Assert.Equal("This reminder already exists", actual.Message);
        }

        [Fact, TestPriority(7)]
        public void DeleteReminderShouldThrowException()
        {
            var mockRepo = new Mock<IReminderRepository>();
            int Id = 205;
            mockRepo.Setup(repo => repo.DeleteReminder(Id)).Returns(false);
            var service = new ReminderService.Service.ReminderService(mockRepo.Object);

            var actual = Assert.Throws<ReminderNotFoundException>(() => service.DeleteReminder(Id));

            Assert.Equal("This reminder id not found", actual.Message);
        }
        //[Fact,TestPriority(8)]
        //public void GetAllRemindersShouldReturnEmptyList()
        //{
        //    var mockRepo = new Mock<IReminderRepository>();
        //    mockRepo.Setup(repo => repo.GetAllReminders()).Returns(new List<Reminder>());
        //    var service = new Service.ReminderService(mockRepo.Object);

        //    var actual = service.GetAllReminders();

        //    Assert.IsAssignableFrom<List<Reminder>>(actual);
        //    Assert.Empty(actual);
        //}

        [Fact, TestPriority(9)]
        public void GetReminderByIdShouldThrowException()
        {
            int Id = 205;
            Reminder reminder = null;
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.GetReminderById(Id)).Returns(reminder);
            var service = new ReminderService.Service.ReminderService(mockRepo.Object);

            var actual = Assert.Throws<ReminderNotFoundException>(() => service.GetReminderById(Id));

            Assert.Equal("This reminder id not found", actual.Message);
        }

        [Fact, TestPriority(10)]
        public void UpdateReminderShouldThrowException()
        {
            int Id = 205;
            Reminder reminder = new Reminder { Id = 205, Name = "Books", CreatedBy = "Sachin", Description = "books reminder", CreationDate = new DateTime(), Type = "email" };
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.UpdateReminder(Id, reminder)).Returns(false);
            var service = new ReminderService.Service.ReminderService(mockRepo.Object);


            var actual = Assert.Throws<ReminderNotFoundException>(() => service.UpdateReminder(Id, reminder));
            Assert.Equal("This reminder id not found", actual.Message);
        }

        #endregion negative tests
    }
}
