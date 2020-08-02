using System;
using System.Collections.Generic;
using System.Text;
using CategoryService.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Test.InfraSetup
{
    public class CategoryDbFixture:IDisposable
    {
        private IConfigurationRoot configuration;
        public CategoryContext context;
        public CategoryDbFixture()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            configuration = builder.Build();
            context = new CategoryContext(configuration);
            context.Categories.DeleteMany(Builders<Category>.Filter.Empty);
            context.Categories.InsertMany(new List<Category>
            {
                new Category{Id=101, Name="Sports", CreatedBy="Mukesh", Description="All about sports", CreationDate=DateTime.Now },
                 new Category{Id=102, Name="Politics", CreatedBy="Mukesh", Description="INDIAN politics", CreationDate=DateTime.Now }
            });
        }
        public void Dispose()
        {
            context = null;
        }
    }
}
