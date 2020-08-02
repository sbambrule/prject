using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteService.Exceptions
{
    public class NoteAlreadyExistsException:ApplicationException
    {
        public NoteAlreadyExistsException(string message) : base(message) { }
    }
}
