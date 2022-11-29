using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Sandwich2Go.UT.SandwichControllers_test
{
    public static class UtilitiesForSandwichesForOffer
    {

        public static IList<Sandwich> GetSandwiches(int index, int numOfSandwiches)
        {

            var allSandwiches = new List<Sandwich>
            {
                new Sandwich { Id = 1, SandwichName = "Cubano", Precio = 5.50, Desc = "Queso, jamón y mayonesa"},
                new Sandwich { Id = 2, SandwichName = "Mixto", Precio = 3.00, Desc = "Jamón y queso"},
                new Sandwich { Id = 3, SandwichName = "Inglés", Precio = 4.00, Desc = "Jamón, queso y huevo revuelto"}
            };
            return allSandwiches.GetRange(index, numOfSandwiches);
        }

        public static void InitializeDbSandwichesForTests(ApplicationDbContext db)
        {

            db.Sandwich.AddRange(GetSandwiches(0, 2));
            db.SaveChanges();

            db.Users.Add(new Gerente { Id = "1", UserName = "elena@uclm.com", Email = "elena@uclm.com", Nombre = "Elena", Apellido = "Navarro Martinez", EmailConfirmed = true, Direccion = "" });
            db.SaveChanges();
        }

        public static void ReInitializeDbSandwichesForTests(ApplicationDbContext db)
        {
            db.Sandwich.RemoveRange(db.Sandwich);
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
        }
    }
}
