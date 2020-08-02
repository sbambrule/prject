using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace NoteService.Models
{
    public class NoteContext
    {
        //declare variables to connect to MongoDB database
        private readonly IMongoDatabase database;
        MongoClient client;
        public NoteContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration

            string cont = Environment.GetEnvironmentVariable("MONGO_DB");
            client = new MongoClient(cont);
            //client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            
            database = client.GetDatabase(configuration.GetSection("MongoDB:NoteDatabase").Value);
        }

        //Define a MongoCollection to represent the Notes collection of MongoDB based on NoteUser type
        public IMongoCollection<NoteUser> Notes => database.GetCollection<NoteUser>("NoteUsers");
    }
}
