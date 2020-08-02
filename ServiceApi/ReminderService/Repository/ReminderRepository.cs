using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using ReminderService.Models;

namespace ReminderService.Repository
{
    public class ReminderRepository : IReminderRepository
    {
        //define a private variable to represent ReminderContext
        ReminderContext reminderContext = null;
        public ReminderRepository(ReminderContext _context)
        {
            this.reminderContext = _context;
        }
        //This method should be used to save a new reminder.
        public Reminder CreateReminder(Reminder reminder)
        {
            //reminder Id should be auto generated and must start from 201
            reminder.CreationDate = DateTime.Now;
            var result = this.reminderContext.Reminders.Find(x => true).SortByDescending(d => d.Id).Limit(1).FirstOrDefaultAsync();

            if (result.Result == null || result.Result.Id == 0)
            {
                reminder.Id = 100;
            }
            else
            {
                reminder.Id = result.Result.Id + 1;
            }

            this.reminderContext.Reminders.InsertOne(reminder);
            return reminder;
        }
        //This method should be used to delete an existing reminder.
        public bool DeleteReminder(int reminderId)
        {
            return this.reminderContext.Reminders.DeleteOne<Reminder>(user => user.Id == reminderId).DeletedCount > 0;
        }
        //This method should be used to get all reminders by userId
        public List<Reminder> GetAllRemindersByUserId(string userId)
        {
            return this.reminderContext.Reminders.Find(x => x.CreatedBy == userId).ToList();
        }
        //This method should be used to get a reminder by reminderId
        public Reminder GetReminderById(int reminderId)
        {
            return this.reminderContext.Reminders.Find<Reminder>(x => x.Id == reminderId).SingleOrDefault();
        }
        // This method should be used to update an existing reminder.
        public bool UpdateReminder(int reminderId, Reminder reminder)
        {
            return this.reminderContext.Reminders.ReplaceOne<Reminder>(u => u.Id == reminderId, reminder).ModifiedCount > 0;
        }
    }
}