using System.Collections.Generic;
using ReminderService.Models;

namespace ReminderService.Service
{
    public interface IReminderService
    {
        Reminder CreateReminder(Reminder reminder);
        bool DeleteReminder(int reminderId);
        bool UpdateReminder(int reminderId, Reminder reminder);
        Reminder GetReminderById(int reminderId);
        List<Reminder> GetAllRemindersByUserId(string userId);
    }
}
