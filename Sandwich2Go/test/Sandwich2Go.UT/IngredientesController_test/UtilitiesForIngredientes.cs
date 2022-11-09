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

namespace Sandwich2Go.UT.IngredientesController_test
{
    public static class UtilitiesForIngredientes
    {
        public static void InitializeDbGenresForTests(ApplicationDbContext db)
        {
            db.Alergeno.AddRange(GetAlergenos(0, 3));
            db.SaveChanges();

        }

        public static void ReInitializeDbAlergenosForTests(ApplicationDbContext db)
        {
            db.Alergeno.RemoveRange(db.Alergeno);
            db.SaveChanges();
        }

        public static void InitializeDbIngredientesForTests(ApplicationDbContext db)
        {

            db.Ingrediente.AddRange(GetIngredientes(0, 4));
            //genre id=1 it is already added because it is related to the movies
            db.Alergeno.AddRange(GetAlergenos(2, 2));
            db.SaveChanges();

            db.Users.Add(new Customer { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Name = "Peter", FirstSurname = "Jackson", SecondSurname = "García" });
            db.SaveChanges();
        }

        public static void ReInitializeDbIngredientesForTests(ApplicationDbContext db)
        {
            db.Ingrediente.RemoveRange(db.Ingrediente);
            db.Alergeno.RemoveRange(db.Alergeno);
            db.SaveChanges();
        }

        public static IList<Ingrediente> GetMovies(int index, int numOfIngredientes)
        {
            Alergeno alergeno = GetAlergenos(0, 1).First();
            Alergeno alergeno2 = GetAlergenos(1, 1).First();
            var allIngredientes = new List<Ingrediente>
            {
                new Ingrediente { Id = 1, Nombre = "Pepino", PrecioUnitario= 3, Stock = 8},
                 new Ingrediente { Id = 2, Nombre = "Ketchup", PrecioUnitario= 2, Stock = 7},
                  new Ingrediente { Id = 3, Nombre = "Pan", PrecioUnitario= 4, Stock = 3},
                  new Ingrediente { Id = 4, Nombre = "Lechuga", PrecioUnitario= 5, Stock = 2}

            };

            return allIngredientes.GetRange(index, numOfIngredientes);
        }

        public static IList<Alergeno> GetAlergenos(int index, int numOfAlergenos)
        {
            var allAlergenos = new List<Alergeno>
            {
                new Alergeno { id=1, Name = "Gluten" } ,
                new Alergeno { id=2, Name = "Soja" },
                 new Alergeno { id=3, Name = "Huevo" },
                  new Alergeno { id=4, Name = "Lactosa" }
                };
            //return from the list as much instances as specified in numOfGenres
            return allAlergenos.GetRange(index, numOfAlergenos);
        }


        
    }
}
