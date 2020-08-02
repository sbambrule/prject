using System;
using System.Collections.Generic;
using System.Linq;
using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Repository;

namespace ReminderService.Service
{
    public class ReminderService : IReminderService
    {
        //define a private variable to represent repository
        IReminderRepository reminderRepository = null;
        //Use constructor Injection to inject all required dependencies.

        public ReminderService(IReminderRepository reminderRepository)
        {
            this.reminderRepository = reminderRepository;
        }

        //This method should be used to save a new reminder.
        public Reminder CreateReminder(Reminder reminder)
        {
            var list = reminderRepository.GetAllRemindersByUserId(reminder.CreatedBy);

            if (list != null)
            {
                var count = list.Count(x => x.Name == reminder.Name);
                if (count >0)
                {
                    throw new ReminderNotCreatedException("This reminder already exists");
                }

            }
            var findUser = reminderRepository.GetReminderById(reminder.Id);

            if (findUser != null)
            {
                throw new ReminderNotCreatedException("This reminder already exists");
            }


            var result = reminderRepository.CreateReminder(reminder);
            return result;
        }
        //This method should be used to delete an existing reminder.
        public bool DeleteReminder(int reminderId)
        {
            var result = reminderRepository.DeleteReminder(reminderId);
            if (!result)
            {
                throw new ReminderNotFoundException("This reminder id not found");
            }
            return result;
        }
        // This method should be used to get all reminders by userId
        public List<Reminder> GetAllRemindersByUserId(string userId)
        {
            var result = reminderRepository.GetAllRemindersByUserId(userId);

            return result;
        }
        //This method should be used to get a reminder by reminderId.
        public Reminder GetReminderById(int reminderId)
        {
            var result = reminderRepository.GetReminderById(reminderId);
            if (result == null)
            {
                throw new ReminderNotFoundException("This reminder id not found");
            }
            return result;
        }
        //This method should be used to update an existing reminder.
        public bool UpdateReminder(int reminderId, Reminder reminder)
        {
            var result = reminderRepository.UpdateReminder(reminderId, reminder);

            if (!result)
            {
                throw new ReminderNotFoundException("This reminder id not found");
            }
            return result;
        }
    }
}
