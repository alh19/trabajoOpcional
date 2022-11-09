using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandwich2Go.UT
{
    public static class Utilities
    {
        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()

        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("AppForMovies")
                        .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }
        public static void InitializeDbCustomersForTests(ApplicationDbContext db)
        {

            db.Users.Add(GetUsers(0, 1).First());
            db.SaveChanges();
        }

        public static void ReInitializeDbUsersForTests(ApplicationDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
        }

        public static IList<ApplicationUser> GetUsers(int index, int numOfUsers)
        {
            var allUsers = new List<ApplicationUser>
                {
                   new Customer { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595",  Email = "peter@uclm.com", Name = "Peter", FirstSurname = "Jackson", SecondSurname = "García" }
                };
            //return from the list as much instances as specified in numOfGenres
            return allUsers.GetRange(index, numOfUsers);
        }
    }
}
