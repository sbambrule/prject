using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ReminderService.Models
{
    public class Reminder
    {
        /*
	 * This class should have six properties
	 * (Id,Name,Description,Type,
	 * CreatedBy,CreationDate). Out of these six fields, the field
	 * Id should be annotated with [BsonId].The value of CreationDate should not
	 * be accepted from the user but should be always initialized with the system
	 * date.
	 */
        [BsonId]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }
        public string CreatedBy { get; set; }

        [JsonIgnore]
        public DateTime CreationDate { get; set; }
    }
}
