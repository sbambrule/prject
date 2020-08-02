using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace ReminderService.Models
{
    public class ReminderContext
    {
        //declare variables to connect to MongoDB database
        private readonly IMongoDatabase database;
        MongoClient client;
        public ReminderContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration

            string cont = Environment.GetEnvironmentVariable("MONGO_DB");
            client = new MongoClient(cont);
            //client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            database = client.GetDatabase(configuration.GetSection("MongoDB:ReminderDatabase").Value);
        }

        //Define a MongoCollection to represent the Reminders collection of MongoDB

        public IMongoCollection<Reminder> Reminders => database.GetCollection<Reminder>("Reminders");

    }
}

