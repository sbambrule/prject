using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using NoteService.Models;
using NoteService.Repository;
using NoteService.Exceptions;
using Test;

namespace Service.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class NoteServiceTest
    {
        
        [Fact, TestPriority(1)]
        public void CreateNoteShouldReturnTrue()
        {
            var note = this.GetNote();
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.CreateNote(note)).Returns(true);
            var service = new NoteService.Service.NoteService(mockRepo.Object);

            var actual = service.CreateNote(note);
            Assert.True(actual);
        }


        [Fact, TestPriority(2)]
        public void UpdateNoteShouldReturnNote()
        {
            int noteId = 101;
            string userId = "Mukesh";
            Note note = new Note();
            note.Id = 101;
            note.Title = "IPL 2018";
            note.Content = "Mumbai Indians vs RCB match scheduled  for 4 PM is cancelled";
            note.Category = this.GetCategory();
            note.Reminders = this.GetReminder();
            note.CreatedBy = "Mukesh";
            note.CreationDate = DateTime.Now;

            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.UpdateNote(noteId, userId, note)).Returns(true);
            var service = new NoteService.Service.NoteService(mockRepo.Object);

            var actual = service.UpdateNote(noteId,userId, note);
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<Note>(actual);
        }

        [Fact, TestPriority(3)]
        public void DeleteNoteShouldReturnTrue()
        {
            int noteId = 101;
            string userId = "Mukesh";
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.DeleteNote(userId, noteId)).Returns(true);
            var service = new NoteService.Service.NoteService(mockRepo.Object);

            var actual = service.DeleteNote(userId, noteId);

            Assert.True(actual);
        }

        [Fact , TestPriority(4)]
        public void GetAllNotesShouldReturnAList()
        {
            string userId = "Mukesh";
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.FindAllNotesByUser(userId)).Returns(this.GetNotes());
            var service = new NoteService.Service.NoteService(mockRepo.Object);

            var actual = service.GetAllNotesByUserId(userId);
            Assert.NotEmpty(actual);
            Assert.IsAssignableFrom<List<Note>>(actual);
        }

        [Fact, TestPriority(5)]
        public void DeleteShouldThrowException()
        {
            int noteId = 121;
            string userId = "Mukesh";
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.DeleteNote(userId, noteId)).Returns(false);
            var service = new NoteService.Service.NoteService(mockRepo.Object);

            var actual = Assert.Throws<NoteNotFoundExeption>(()=> service.DeleteNote(userId, noteId));
        }

        [Fact, TestPriority(6)]
        public void UpdateShouldThrowException()
        {
            int noteId = 121;
            string userId = "Mukesh";
            Note note = new Note();
            note.Id = 121;
            note.Title = "IPL 2018";
            note.Content = "Mumbai Indians vs RCB match scheduled  for 4 PM is cancelled";
            note.Category = this.GetCategory();
            note.Reminders = this.GetReminder();
            note.CreatedBy = "Mukesh";
            note.CreationDate = new DateTime();

            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.UpdateNote(noteId, userId, note)).Returns(false);
            var service = new NoteService.Service.NoteService(mockRepo.Object);

            var actual = Assert.Throws<NoteNotFoundExeption>(() => service.UpdateNote(noteId, userId, note));
        }

        #region helper methods
        private Category GetCategory()
        {
            Category category = new Category();
            category.Id = 201;
            category.Name = "Cricket";
            category.Description = "IPL 20-20";
            category.CreatedBy = "Mukesh";
            category.CreationDate = new DateTime();
            return category;
        }

        private List<Reminder> GetReminder()
        {
            List<Reminder> reminders = new List<Reminder>();
            Reminder reminder = new Reminder();
            reminder.Id = 301;
            reminder.Name = "Email-reminder";
            reminder.Description = "sending-mails";
            reminder.Type = "email";
            reminder.CreatedBy = "Mukesh";
            reminder.CreationDate = new DateTime();
            reminders.Add(reminder);
            return reminders;
        }

        private Note GetNote()
        {
            Note note = new Note();
            note.Id = 101;
            note.Title = "IPL 2018";
            note.Content = "Mumbai Indians vs RCB match scheduled  for 4 PM";
            note.Category = this.GetCategory();
            note.Reminders = this.GetReminder();
            note.CreatedBy = "Mukesh";
            note.CreationDate = new DateTime();

            return note;
        }
        private List<Note> GetNotes()
        {
            List<Note> notes = new List<Note>();
            notes.Add(this.GetNote());
            return notes;
        }

        #endregion helper methods
    }
}
