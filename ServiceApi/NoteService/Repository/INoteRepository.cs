using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoteService.Models;

namespace NoteService.Repository
{
    public interface INoteRepository
    {
        bool CreateNote(Note note);
        bool DeleteNote(string userId, int noteId);
       // bool DeleteAllNotes(string userId);
        bool UpdateNote(int noteId, string userId, Note note);
        //Note GetNoteByNoteId(string userId, int noteId);
       // List<NoteUser> FindByUserId(string userId);
        List<Note> FindAllNotesByUser(string userId);
    }
}
