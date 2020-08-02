using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteService.Exceptions
{
    public class NoteNotFoundExeption:ApplicationException
    {
        public NoteNotFoundExeption() { }
        public NoteNotFoundExeption(string message) : base(message) { }
    }
}
