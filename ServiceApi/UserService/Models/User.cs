using System;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Models
{
    public class User
    {
        /*
	 * This class should have four properties (UserId,Name,
	 * Contact,AddedDate). Out of these four fields, the field
	 * UserId should be annotated with [BsonId].The value of AddedDate should not be accepted from the user but
	 * should be always initialized with the system date.
	 */
        [BsonId]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
