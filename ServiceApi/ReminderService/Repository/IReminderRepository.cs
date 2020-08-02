using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReminderService.Models;

namespace ReminderService.Repository
{
    public interface IReminderRepository
    {
        Reminder CreateReminder(Reminder reminder);
        bool DeleteReminder(int reminderId);
        bool UpdateReminder(int reminderId, Reminder reminder);
        Reminder GetReminderById(int reminderId);
        List<Reminder> GetAllRemindersByUserId(string userId);
    }
}
