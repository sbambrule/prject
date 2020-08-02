using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UserService.Models;

namespace Test.InfraSetup
{
    public class UserDbFixture:IDisposable
    {
        private IConfigurationRoot configuration;
        public UserContext context;
        public UserDbFixture()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            configuration = builder.Build();
            context = new UserContext(configuration);
            context.Users.DeleteMany(Builders<User>.Filter.Empty);
            context.Users.InsertMany(new List<User>
            {
                new User{ UserId="Mukesh", Name="Mukesh",Contact="9812345670", AddedDate=DateTime.Now},
                new User{ UserId="Sachin", Name="Sachin", Contact="8987653412", AddedDate=DateTime.Now}
            });
        }
        public void Dispose()
        {
            context = null;
        }
    }
}
