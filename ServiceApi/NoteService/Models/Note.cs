using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace NoteService.Models
{
    public class Note
    {
        /*
	 * This class should have seven properies
	 * (Id,Title,Content,CreationDate,
	 * Category,Reminders,CreatedBy). Out of these seven properties Id should
     * be annotated with [BsonId]. The value of CreationDate should not be
	 * accepted from the user but should be always initialized with the system date.
	 */

        [BsonId]
        public int Id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        [JsonIgnore]
        public DateTime CreationDate { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Reminder> Reminders { get; set; }
    }
}
