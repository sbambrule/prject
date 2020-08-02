using System;
using System.Collections.Generic;
using System.Linq;
using NoteService.Exceptions;
using NoteService.Models;
using NoteService.Repository;

namespace NoteService.Service
{
    public class NoteService : INoteService
    {
        //define a private variable to represent repository
        INoteRepository noteRepository = null;
        //Use constructor Injection to inject all required dependencies.

        public NoteService(INoteRepository _noteRepository)
        {
            this.noteRepository = _noteRepository;
        }

        //This method should be used to create a new note.
        public bool CreateNote(Note note)
        {
            var result = noteRepository.CreateNote(note);
            return result;
        }

        //This method should be used to delete an existing note for a user
        public bool DeleteNote(string userId, int noteId)
        {
            
            var result = noteRepository.DeleteNote(userId, noteId);
            if (!result)
            {
                throw new NoteNotFoundExeption($"NoteId {noteId} for user {userId} does not exist");
            }
            return result;
        }

        //This methos is used to retreive all notes for a user
        public List<Note> GetAllNotesByUserId(string userId)
        {
            return noteRepository.FindAllNotesByUser(userId);
        }

        //This method is used to update an existing note for a user
        public Note UpdateNote(int noteId, string userId, Note note)
        {             
            var updateresult = noteRepository.UpdateNote(noteId, userId, note);
            if (!updateresult)
            {
                throw new NoteNotFoundExeption($"NoteId {noteId} for user {userId} does not exist");
            }
            return note;
        }

    }
}
