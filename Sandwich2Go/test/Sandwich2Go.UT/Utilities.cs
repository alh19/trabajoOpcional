using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.UT.SandwichControllers_test;
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
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseInMemoryDatabase("Sandwich2Go")
                .UseInternalServiceProvider(serviceProvider);

            builder.EnableSensitiveDataLogging();
            builder.EnableDetailedErrors();

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

        public static IList<Usuario> GetUsers(int index, int numOfUsers)
        {
            var allUsers = new List<Usuario> {
                new Gerente
                   {
                       Id = "1",
                       UserName = "elena@uclm.com",
                       Email = "elena@uclm.com",
                       Nombre = "Elena",
                       Apellido = "Navarro Martinez",
                       EmailConfirmed = true,
                       Direccion = ""
                   },
                new Cliente {
                        Id = "2",
                        UserName = "gregorio@uclm.com",
                        Email = "gregorio@uclm.com",
                        Nombre = "Gregorio",
                        Apellido = "Diaz Descalzo",
                        EmailConfirmed = true,
                        Direccion = ""
                   }
                   
                };
            //return from the list as much instances as specified in numOfGenres
            return allUsers.GetRange(index, numOfUsers);
        }
    }
}
