using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace UserService.Models
{
    public class UserContext
    {
        //declare variable to connect to MongoDB database
        private readonly IMongoDatabase database;
        MongoClient client;
        public UserContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration

            string cont = Environment.GetEnvironmentVariable("MONGO_DB");
            client = new MongoClient(cont);
            //client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            database = client.GetDatabase(configuration.GetSection("MongoDB:UserDatabase").Value);
        }

        //Define a MongoCollection to represent the Users collection of MongoDB
        public IMongoCollection<User> Users => database.GetCollection<User>("Users");

    }
}
