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
            IList<Ingrediente> ingredientes = GetIngredientes(0, 2);

            var allSandwiches = new List<Sandwich>
            {
                new Sandwich { Id = 1, SandwichName = "Cubano", Precio = 5.50, Desc = "Cubano", IngredienteSandwich = new List<IngredienteSandwich>{ new IngredienteSandwich {Id=1, IngredienteId=1, SandwichId=1,Cantidad=1, Ingrediente = ingredientes[0] } } },
                new Sandwich { Id = 2, SandwichName = "Mixto", Precio = 3.00, Desc = "Mixto", IngredienteSandwich = new List<IngredienteSandwich>{ new IngredienteSandwich {Id=2, IngredienteId=2, SandwichId=2,Cantidad=1, Ingrediente = ingredientes[1] } }  },
                new Sandwich { Id = 3, SandwichName = "Inglés", Precio = 4.00, Desc = "Inglés", IngredienteSandwich = new List<IngredienteSandwich>{ new IngredienteSandwich {Id=3, IngredienteId=1, SandwichId=3,Cantidad=1, Ingrediente = ingredientes[0] }, new IngredienteSandwich { Id = 4, IngredienteId = 2, SandwichId = 3, Cantidad = 1, Ingrediente = ingredientes[1] } } }
            };
            return allSandwiches.GetRange(index, numOfSandwiches);
        }
        public static IList<Ingrediente> GetIngredientes(int index, int numOfIngredientes)
        {

            var allIngredientes = new List<Ingrediente>
            {
                new Ingrediente {Id=1,Nombre="Lechuga",PrecioUnitario=2,Stock=8},
                new Ingrediente {Id=2,Nombre="Tomate",PrecioUnitario=3,Stock=9}
            };

            return allIngredientes.GetRange(index, numOfIngredientes);
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