using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace NoteService.Models
{
    public class NoteUser
    {
        /*
	 * This class should have two properties (UserId, Notes).Out of these two fields,
	 * the field userId should be annotated with [BsonId]. 
	 */
        [BsonId]
        public string UserId { get; set; }

        public List<Note> Notes = new List<Note>();
    }
}
