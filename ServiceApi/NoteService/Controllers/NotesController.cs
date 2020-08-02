using System;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NoteService.Exceptions;
using NoteService.Models;
using NoteService.Service;

namespace NoteService.Controllers
{
    /*
      As in this assignment, we are working with creating RESTful web service to create microservices, hence annotate
      the class with [ApiController] annotation and define the controller level route as per REST Api standard.
  */

    [EnableCors("AllowMyOrigin")]
    [Authorize]
    public class NotesController : Controller
    {
        /*
        NoteService should  be injected through constructor injection. Please note that we should not create service
        object using the new keyword
        */
        INoteService noteService;
        public NotesController(INoteService noteService)
        {
            this.noteService = noteService;
        }

        /// <summary>
        /// Method to get note information by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/[controller]/{userId}")]
        [HttpGet]
        public IActionResult Get(string userId)
        {
            var o = noteService.GetAllNotesByUserId(userId);
            return Ok(o);
        }

        /// <summary>
        /// Method to delete note information by id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Route("api/[controller]/{userId}/{noteId}/")]
        [HttpDelete]
        public IActionResult Delete(string userId, int noteId)
        {
            var o = noteService.DeleteNote(userId, noteId);
            return Ok(o);
        }

        /// <summary>
        /// Method to add note information by id
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [Route("api/[controller]/{userId}")]
        [HttpPost]
        public IActionResult Post(string userId, [FromBody] Note note)
        {
            note.CreatedBy = userId;
            if (string.IsNullOrEmpty(note.CreatedBy))
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                note.CreatedBy = claimsIdentity.Name;
            }
            var o = noteService.CreateNote(note);
            return StatusCode((int)HttpStatusCode.Created, note);
        }

        /// <summary>
        /// Method to update note information by id
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [Route("api/[controller]/{userId}/{noteId}")]
        [HttpPut]
        public IActionResult Put(string userId, int noteId, [FromBody] Note note)
        {
            if (string.IsNullOrEmpty(note.CreatedBy))
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                note.CreatedBy = claimsIdentity.Name;
            }
            var o = noteService.UpdateNote(noteId, userId, note);
            return Ok(o);
        }
        /*
	    * Define a handler method which will create a specific note by reading the
	    * Serialized object from request body and save the note details in the
	    * database.This handler method should return any one of the status messages
	    * basis on different situations: 
	    * 1. 201(CREATED) - If the note created successfully. 
	    
	    * This handler method should map to the URL "/api/note/{userId}" using HTTP POST method
	    */

        /*
         * Define a handler method which will delete a note from a database.
         * This handler method should return any one of the status messages basis 
         * on different situations: 
         * 1. 200(OK) - If the note deleted successfully from database. 
         * 2. 404(NOT FOUND) - If the note with specified noteId is not found.
         *
         * This handler method should map to the URL "/api/note/{userId}/{noteId}" using HTTP Delete
         */

        /*
         * Define a handler method which will update a specific note by reading the
         * Serialized object from request body and save the updated note details in a
         * database. 
         * This handler method should return any one of the status messages
         * basis on different situations: 
         * 1. 200(OK) - If the note updated successfully.
         * 2. 404(NOT FOUND) - If the note with specified noteId is not found.
         * 
         * This handler method should map to the URL "/api/note/{userId}/{noteId}" using HTTP PUT method.
         */

        /*
         * Define a handler method which will get us the all notes by a userId.
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the note found successfully. 
         * 
         * This handler method should map to the URL "/api/note/{userId}" using HTTP GET method
         */

        /*
         * Define a handler method which will show details of a specific note created by specific 
         * user. This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the note found successfully. 
         * 2. 404(NOT FOUND) - If the note with specified noteId is not found.
         * This handler method should map to the URL "/api/note/{userId}/{noteId}" using HTTP GET method
         * where "id" should be replaced by a valid reminderId without {}
         * 
         */
    }
}
