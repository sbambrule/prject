using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuthenticationService.Models
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {            
            this.Database.EnsureCreated();
            //make sure that database is auto generated using EF Core Code first
        }
        public DbSet<User> Users { get; set; }
        //Define a Dbset for User in the database

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users"); 
        }
    }
}
