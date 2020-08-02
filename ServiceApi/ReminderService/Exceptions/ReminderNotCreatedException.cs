using System;

namespace ReminderService.Exceptions
{
    public class ReminderNotCreatedException:ApplicationException
    {
        public ReminderNotCreatedException() { }
        public ReminderNotCreatedException(string message) : base(message) { }
    }
}
