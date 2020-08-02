using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace CategoryService.Models
{
    public class CategoryContext
    {
        //declare variable to connect to MongoDB database 
        private readonly IMongoDatabase database;
        MongoClient client;
        public CategoryContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from 

            string cont = Environment.GetEnvironmentVariable("MONGO_DB");
            client = new MongoClient(cont);
            //client = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            database = client.GetDatabase(configuration.GetSection("MongoDB:CategoryDatabase").Value);
        }

        //Define a MongoCollection to represent the Categories collection of MongoDB
        public IMongoCollection<Category> Categories => database.GetCollection<Category>("Categories");

        //Define a MongoCollection to represent the Categories collection of MongoDB
    }
}
