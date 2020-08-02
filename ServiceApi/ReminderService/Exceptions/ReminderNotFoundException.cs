using System;

namespace ReminderService.Exceptions
{
    public class ReminderNotFoundException:ApplicationException
    {
        public ReminderNotFoundException() { }
        public ReminderNotFoundException(string message) : base(message) { }
    }
}
