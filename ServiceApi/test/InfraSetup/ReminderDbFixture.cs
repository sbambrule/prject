using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ReminderService.Models;

namespace Test.InfraSetup
{
    public class ReminderDbFixture:IDisposable
    {
        private IConfigurationRoot configuration;
        public ReminderContext context;
        public ReminderDbFixture()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            configuration = builder.Build();
            context = new ReminderContext(configuration);
            context.Reminders.DeleteMany(Builders<Reminder>.Filter.Empty);
            context.Reminders.InsertMany(new List<Reminder>
            {
                new Reminder{Id=201, Name="Sports", CreatedBy="Mukesh", Description="sports reminder", CreationDate=DateTime.Now, Type="email" },
                 new Reminder{Id=202, Name="Politics", CreatedBy="Mukesh", Description="politics reminder", CreationDate=DateTime.Now,Type="email" }
            });
        }
        public void Dispose()
        {
            context = null;
        }
    }
}
