using System;
using System.Collections.Generic;
using System.Linq;
using NoteService.Models;
using NoteService.Repository;
using Test;
using Test.InfraSetup;
using Xunit;

namespace Repository.Test
{
    [TestCaseOrderer("Test.PriorityOrderer", "Test")]
    public class NoteRepositoryTest:IClassFixture<NoteDbFixture>
    {
        NoteDbFixture fixture;
        private readonly INoteRepository repository;

        public NoteRepositoryTest(NoteDbFixture _fixture)
        {
            this.fixture = _fixture;
            repository = new NoteRepository(fixture.context);
        }
         
        [Fact, TestPriority(1)]
        public void CreateNoteShouldReturnTrue()
        {
            Note note = new Note();
            note.Title = "IPL 2018";
            note.Content = "Mumbai Indians vs RCB match scheduled  for 4 PM";
            note.Category = this.GetCategory();
            note.Reminders = this.GetReminder();
            note.CreatedBy = "Mukesh";
            note.Id = 101;

            var actual= repository.CreateNote(note);
            Assert.True(actual);

            List<Note> notes = repository.FindAllNotesByUser("Mukesh");
            Assert.Contains(notes, n => n.Title == "IPL 2018");
        }

        [Fact, TestPriority(2)]
        public void DeleteNoteShouldReturnTrue()
        {
            string userId = "Mukesh";
            int noteId = 101;
            
            var actual = repository.DeleteNote(userId, noteId);
            Assert.True(actual);

            List<Note> notes = repository.FindAllNotesByUser(userId);
            var note = notes.Where(n => n.Id == noteId).FirstOrDefault();

            Assert.Null(note);
        }

        [Fact, TestPriority(3)]
        public void UpdateNoteShouldReturnTrue()
        {
            string userId = "Sachin";
            int noteId = 101;

            Note note = new Note();
            note.Title = "IPL 2018";
            note.Content = "Mumbai Indians vs RCB match scheduled  for 4 PM is cancelled";
            note.Category = this.GetCategory();
            note.Reminders = this.GetReminder();
            note.CreatedBy = "Sachin";
            note.CreationDate = new DateTime();


            var actual = repository.UpdateNote(noteId, userId,note);
            Assert.True(actual);
        }
        
        [Fact, TestPriority(4)]
        public void FindAllShouldReturnAlist()
        {
            string userId = "Mukesh";

            var actual = repository.FindAllNotesByUser(userId);
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<List<Note>>(actual);
        }

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
       
    }
}
